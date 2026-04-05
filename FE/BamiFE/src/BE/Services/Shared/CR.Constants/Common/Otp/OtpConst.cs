namespace CR.Constants.Common.Otp;
/// <summary>
/// Hằng số Otp
/// </summary>
public static class OtpConst
{
    /// <summary>
    /// Thời gian hết hạn otp (đơn vị giây)
    /// </summary>
    public const int OtpLifeTime = 100;

    /// <summary>
    /// Số lần verity tối đa
    /// </summary>
    public const int OtpMaxTimesVerify = 3;
    /// <summary>
    /// Số lần gửi otp tối đa
    /// </summary>
    public const int OtpMaxSendTimes = 3;

    /// <summary>
    /// Thời gian gửi lại Otp (15 phút)
    /// </summary>
    public const int TimeForResendOtp = 15;
}
