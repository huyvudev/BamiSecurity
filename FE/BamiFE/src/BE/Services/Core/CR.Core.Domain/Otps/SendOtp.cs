using CR.Constants.Common.Database;
using Microsoft.EntityFrameworkCore;

namespace CR.Core.Domain.Otps;

[Table(nameof(SendOtp), Schema = DbSchemas.Default)]
[Index(nameof(Username), IsUnique = false, Name = $"IX_{nameof(SendOtp)}")]
public class SendOtp
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// Username verify
    /// </summary>
    [Unicode(false)]
    [MaxLength(128)]
    public required string Username { get; set; }

    /// <summary>
    /// Số lần gửi đếm từ 0, khi đạt giới hạn thì sẽ chặn
    /// </summary>
    public int SendCount { get; set; } = 0;

    /// <summary>
    /// Lần điểm gửi otp
    /// </summary>
    public DateTime LastSentDateTime { get; set; }

    /// <summary>
    /// Khoảng thời gian có chặn 3 lần gửi otp
    /// </summary>
    public DateTime TimeLimitCanVerifyOtp { get; set; }
}
