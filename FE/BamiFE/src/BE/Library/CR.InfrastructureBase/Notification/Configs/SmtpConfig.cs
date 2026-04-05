namespace CR.Noti.ApplicationServices.Configs;

public class SmtpConfig
{
    /// <summary>
    /// Key mã hoá AES ở dạng hexa string.
    /// Dùng cho mã hoá password
    /// </summary>
    public required string AesKey { get; set; }
}
