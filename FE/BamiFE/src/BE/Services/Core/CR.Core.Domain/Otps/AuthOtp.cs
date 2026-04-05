using CR.Constants.Common.Database;
using CR.Core.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace CR.Core.Domain.Otps
{
    [Table(nameof(AuthOtp), Schema = DbSchemas.Default)]
    [Index(
        nameof(UserId),
        nameof(IsUsed),
        Name = $"IX_{nameof(AuthOtp)}"
    )]
    public class AuthOtp
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Otp code được mã hóa
        /// </summary>
        [MaxLength(128)]
        [Unicode(false)]
        public required string OtpCode { get; set; }

        /// <summary>
        /// Thời gian hết hạn
        /// </summary>
        public DateTime ExpireTime { get; set; }

        /// <summary>
        /// Check đã được dùng
        /// </summary>
        public bool IsUsed { get; set; }

        /// <summary>
        /// Opt cho người nào
        /// </summary>
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        /// <summary>
        /// Số lần verify
        /// </summary>
        public int VerifyTime { get; set; }
    }
}
