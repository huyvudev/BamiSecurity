using CR.DtoBase;

namespace CR.Core.ApplicationServices.OtpModule.Abstracts;

public interface IOtpService
{
    Task<Result> SendOtp(int userId);
    Task<Result> ResendOtp(int userId);
    Task<Result> VerifyOtp(string otp, int userId);
}
