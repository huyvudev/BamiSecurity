using CR.Constants.Common.Database;
using CR.Constants.Core.Users;
using CR.Core.Domain.AuthToken;
using CR.EntitiesBase.Entities;
using Microsoft.EntityFrameworkCore;

namespace CR.Core.Domain.Users
{
    /// <summary>
    /// User
    /// </summary>
    [Table(nameof(User), Schema = DbSchemas.Default)]
    [Index(
        nameof(Username),
        nameof(CreatedDate),
        nameof(Status),
        nameof(UserType),
        Name = $"IX_{nameof(User)}"
    )]
    public class User : IUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(128)]
        [Unicode(false)]
        public string Username { get; set; } = null!;

        /// <summary>
        /// Giới tính của người dùng
        /// </summary>
        public GenderTypes? Gender { get; set; }

        /// <summary>
        /// Mã số của người dùng (có thể là mã nhân viên hoặc MSSV)
        /// </summary>
        [Unicode(false)]
        [MaxLength(20)]
        public string? UserCode { get; set; }

        /// <summary>
        /// Số điện thoại của người dùng
        /// </summary>
        [Unicode(false)]
        [MaxLength(20)]
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Ngày sinh của người dùng
        /// </summary>
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Quê quán của người dùng
        /// </summary>
        [MaxLength(500)]
        public string? Hometown { get; set; }

        /// <summary>
        /// Số ID (CCCD) của người dùng
        /// </summary>
        [Unicode(false)]
        [MaxLength(125)]
        public string? IdCode { get; set; }

        [Required]
        [MaxLength(128)]
        [Unicode(false)]
        public string Password { get; set; } = null!;

        [MaxLength(128)]
        [Unicode(false)]
        public string? Email { get; set; }

        [MaxLength(256)]
        public string? FullName { get; set; }

        /// <summary>
        /// Loại user <see cref="UserTypeEnum"/>
        /// </summary>
        public UserTypeEnum UserType { get; set; }

        /// <summary>
        /// Trạng thái user <see cref="UserStatus"/>
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Thời gian xóa tài khoản theo Status = 3(LOCK)
        /// </summary>
        public DateTime? LockedStatus { get; set; }

        /// <summary>
        /// Mã pin
        /// </summary>
        [MaxLength(128)]
        [Unicode(false)]
        public string? PinCode { get; set; }

        /// <summary>
        /// Có phải Mã pin tạm thời không
        /// </summary>
        public bool IsTempPin { get; set; }

        /// <summary>
        /// Có phải là mật khẩu tạm, Yêu cầu thay đổi mật khẩu mới ngay khi đăng nhập
        /// </summary>
        public bool IsPasswordTemp { get; set; }

        /// <summary>
        /// Lần đầu đăng nhập vào App
        /// Mặc định là false. True khi tạo tài khoản trên Cms chọn là mật khẩu tạm !IsPasswordTemp
        /// </summary>
        public bool IsFirstTime { get; set; }

        [MaxLength(256)]
        public string? OperatingSystem { get; set; }

        [MaxLength(256)]
        public string? Browser { get; set; }
        public DateTime? LastLogin { get; set; }

        /// <summary>
        /// Ảnh đại diện
        /// </summary>
        [MaxLength(2048)]
        public string? AvatarImageUri { get; set; }

        /// <summary>
        /// S3 Key
        /// </summary>
        [MaxLength(2024)]
        public string? S3Key { get; set; }

        /// <summary>
        /// requestId Otp
        /// </summary>
        [MaxLength(128)]
        public string? OtpRequestId { get; set; }

        /// <summary>
        /// Thời gian gửi lại Otp khi quá số lần gửi Otp
        /// </summary>
        public DateTime? ResendOtpDate { get; set; }

        public int LoginFailCount { get; set; }
        public DateTime DateTimeLoginFailCount { get; set; }
        /// <summary>
        /// Mã bí mật khi xác nhận quên mật khẩu
        /// </summary>
        [MaxLength(128)]
        public string? SecretPasswordCode { get; set; }

        /// <summary>
        /// Thời gian hết hạn mã bí mật khi xác nhận quên mật khẩu
        /// </summary>
        public DateTime? SecretPasswordExpiryDate { get; set; }

        /// <summary>
        /// Vai trò
        /// </summary>
        public List<UserRole> UserRoles { get; set; } = [];
        public List<NotificationToken> NotificationTokens { get; set; } = [];

        /// <summary>
        /// Id đối tượng thuê cho tài khoản thuê
        /// </summary>
        public int? TenantId { get; set; }

        /// <summary>
        /// Domain đăng ký tenant
        /// </summary>
        [MaxLength(128)]
        public string? TenantDomainRegister { get; set; }

        /// <summary>
        /// Tên tenant đăng ký
        /// </summary>
        [MaxLength(128)]
        public string? TenantNameRegister { get; set; }

        /// <summary>
        /// Ngôn ngữ của tenant đăng ký
        /// </summary>
        [MaxLength(10)]
        [Unicode(false)]
        public string? TenantLanguage { get; set; }
        /// <summary>
        /// Thời gian khóa user (Trường hợp khi đăng nhập sai quá 5 lần sẽ khóa 1 tiếng)
        /// </summary>
        public DateTime? TimeLockUser { get; set; }

        #region audit
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public int? DeletedBy { get; set; }
        public bool Deleted { get; set; }
        #endregion
    }
}
