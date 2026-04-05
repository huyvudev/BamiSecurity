using CR.Constants.Common;
using CR.Constants.Common.Otp;
using CR.Constants.Common.SysVar;
using CR.Constants.ErrorCodes;
using CR.Core.ApplicationServices.Common;
using CR.Core.ApplicationServices.OtpModule.Abstracts;
using CR.Core.Domain.Otps;
using CR.Core.Infrastructure.Exceptions;
using CR.DtoBase;
using CR.DtoBase.Notification;
using CR.InfrastructureBase;
using CR.InfrastructureBase.Notification;
using CR.Utils.DataUtils;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CR.Core.ApplicationServices.OtpModule.Implements;

public class OtpService : CoreServiceBase, IOtpService
{
    private readonly ICreateNotiJob _createNotiJob;
    public OtpService(
        ILogger<OtpService> logger,
        IHttpContextAccessor httpContext,
        ICreateNotiJob createNotiJob,
        ICoreLocalization localization
    )
        : base(logger, httpContext, localization)
    {
        _createNotiJob = createNotiJob;
    }

    public async Task<Result> ResendOtp(int userId)
    {
        _logger.LogInformation($"{nameof(ResendOtp)}: userId = {userId}");
        var result = await SendOtp(userId);
        if (result.IsFailure)
        {
            return result;
        }
        return Result.Success();
    }

    public async Task<Result> SendOtp(int userId)
    {
        int? tenantId = _httpContext.GetCurrentTenantId();
        _logger.LogInformation($"{nameof(SendOtp)}: userId = {userId}, tenantId = {tenantId}");
        var now = DateTimeUtils.GetDate();
        var timeAfterNow = DateTimeUtils.GetDate().AddMinutes(OtpConst.TimeForResendOtp);

        var user = await _dbContext
            .Users.Where(e => !e.Deleted && e.Id == userId && e.TenantId == tenantId)
            .FirstOrDefaultAsync();
        if (user == null)
        {
            _logger.LogError($"Không tìm thấy user ({userId})");
            return Result.Success();
        }

        //Nếu chưa gửi cho người này thì sẽ thêm
        var sendOtpDb = await _dbContext.SendOtps.FirstOrDefaultAsync(e =>
            e.Username == user.Username
        );
        if (sendOtpDb == null)
        {
            _dbContext.SendOtps.Add(
                new SendOtp
                {
                    Username = user.Username,
                    LastSentDateTime = now,
                    TimeLimitCanVerifyOtp = timeAfterNow,
                    SendCount = 1,
                }
            );
        }

        //Reset sau khi gửi
        if (sendOtpDb != null && sendOtpDb.TimeLimitCanVerifyOtp < now)
        {
            sendOtpDb.SendCount = 0;
            sendOtpDb.LastSentDateTime = now;
            sendOtpDb.TimeLimitCanVerifyOtp = timeAfterNow;
        }
        //Check nếu còn thời gian gửi (trong khoảng 15p sau khi gửi)
        else if (
            sendOtpDb != null
            && sendOtpDb.TimeLimitCanVerifyOtp >= now
            && sendOtpDb.SendCount >= OtpConst.OtpMaxSendTimes
        )
        {
            if (sendOtpDb.SendCount == OtpConst.OtpMaxSendTimes)
            {
                sendOtpDb.LastSentDateTime = now;
                sendOtpDb.SendCount++;
                await _dbContext.SaveChangesAsync();
            }
            var remainTime = sendOtpDb.LastSentDateTime.AddMinutes(OtpConst.TimeForResendOtp) - now;

            return Result.Failure(
                CoreErrorCode.CoreSendOtpMultipleRequest,
                this.GetCurrentMethodInfo(),
                new { ResendOtpSecondTime = remainTime.TotalSeconds }
            );
        }
        now = DateTimeUtils.GetDate();
        //Tăng số lần gửi và set thời gian gửi
        if (sendOtpDb != null)
        {
            sendOtpDb.SendCount++;
            sendOtpDb.LastSentDateTime = now;
        }

        //Đánh dấu các otp chưa được dùng => đã dùng
        var otps = _dbContext.AuthOtps.Where(e => e.UserId == userId && !e.IsUsed);
        foreach (var item in otps)
        {
            item.IsUsed = true;
        }
        //Gen otp
        var otpCode = GenerateOTP.GenerateOtp(6);

        //Lấy config thời gian otp trong sys var
        var otpConfig = await _dbContext.SysVars.FirstOrDefaultAsync(e =>
            e.GrName == GrNames.OTP && e.VarName == VarNames.SECOND
        );
        if (int.TryParse(otpConfig?.VarValue, out int otpExpire))
        {
            otpExpire = OtpConst.OtpLifeTime;
        }

        //Lưu otp
        await _dbContext.AuthOtps.AddAsync(
            new AuthOtp
            {
                OtpCode = otpCode,
                ExpireTime = DateTimeUtils.GetDate().AddSeconds(otpExpire),
                IsUsed = false,
                UserId = userId
            }
        );
        await _dbContext.SaveChangesAsync();
        //Gửi mail
        var job = new NotiJob<object>
        {
            Data = new { Otp = otpCode, OtpExpire = otpExpire },
            EventKey = EventKeys.CustomerSendOtp,
            TenantId = tenantId,
            RecipientIds = new[] { userId }
        };
        await _createNotiJob.CreateJob<NotiJob<object>, object>(job);

        return Result.Success();
    }

    public async Task<Result> VerifyOtp(string otp, int userId)
    {
        _logger.LogInformation($"{nameof(VerifyOtp)}: otp = {otp}, userId = {userId}");

        var otpDefault = await _dbContext.SysVars.FirstOrDefaultAsync(e =>
            e.GrName == GrNames.OTP && e.VarName == VarNames.DEFAULT_OTP
        );
        var tenantId = 1;

        var user = await _dbContext
            .Users.Where(e => !e.Deleted && e.Id == userId && e.TenantId == tenantId)
            .FirstOrDefaultAsync();
        if (user == null)
        {
            _logger.LogError($"Lỗi");
            return Result.Success();
        }

        var otpFind = await _dbContext
            .AuthOtps.Where(e => !e.IsUsed && e.UserId == userId)
            .OrderByDescending(e => e.ExpireTime)
            .FirstOrDefaultAsync();
        if (otpFind == null)
        {
            _logger.LogError($"{nameof(VerifyOtp)}: Verify Otp fail: userId = {userId}");
            return Result.Failure(CoreErrorCode.CoreVerifyOtpNotFound, this.GetCurrentMethodInfo());
        }
        var now = DateTimeUtils.GetDate();
        otpFind.VerifyTime++;
        var sendOtpDb = await _dbContext.SendOtps.FirstOrDefaultAsync(e =>
            e.Username == user.Username
        );

        //Otp đã được sử dụng
        if (otpFind.IsUsed && otpFind.OtpCode == otp)
        {
            return Result.Failure(CoreErrorCode.CoreOtpIsVerified, this.GetCurrentMethodInfo());
        }
        ///Otp hết hạn
        if (otpFind.ExpireTime < now || sendOtpDb!.SendCount > OtpConst.OtpMaxSendTimes)
        {
            return Result.Failure(CoreErrorCode.CoreVerifyOtpTimeout, this.GetCurrentMethodInfo());
        }
        ///OTP đã hết lượt xác thực
        if (otpFind.VerifyTime >= OtpConst.OtpMaxTimesVerify && otpFind.OtpCode != otp)
        {
            otpFind.VerifyTime++;
            otpFind.IsUsed = true;
            await _dbContext.SaveChangesAsync();
            return Result.Failure(CoreErrorCode.CoreVerifyOtpTurnEnd, this.GetCurrentMethodInfo());
        }

        if (
            (
                otpFind.OtpCode.ToLower() == otp.ToLower()
                && otpFind.ExpireTime >= now
                && otpFind.VerifyTime <= OtpConst.OtpMaxTimesVerify
            ) || (otpDefault != null && otpDefault.VarValue == otp)
        )
        {
            var otpUsed = _dbContext.AuthOtps.Where(e => e.UserId == userId && e.ExpireTime <= now);
            _dbContext.RemoveRange(otpUsed);
            otpFind.IsUsed = true;
            if (sendOtpDb != null)
            {
                sendOtpDb.SendCount = 1;
                sendOtpDb.LastSentDateTime = now;
                sendOtpDb.TimeLimitCanVerifyOtp = now;
            }
        }
        //Otp không hợp lệ
        else if (otpFind.OtpCode != otp)
        {
            var remainVerifyTime = OtpConst.OtpMaxTimesVerify - otpFind.VerifyTime;
            await _dbContext.SaveChangesAsync();

            return Result.Failure(
                CoreErrorCode.CoreOtpIsInvalid,
                this.GetCurrentMethodInfo(),
                listParam: remainVerifyTime.ToString()
            );
        }
        else if (
            otpFind.OtpCode == otp
            && otpFind.ExpireTime >= now
            && otpFind.VerifyTime > OtpConst.OtpMaxTimesVerify
        )
        {
            return Result.Failure(CoreErrorCode.CoreVerifyOtpTurnEnd, this.GetCurrentMethodInfo());
        }
        await _dbContext.SaveChangesAsync();
        //verify xong xóa các otp của user đã dùng đi
        await _dbContext.SaveChangesAsync();
        await Task.CompletedTask;
        return Result.Success();
    }
}
