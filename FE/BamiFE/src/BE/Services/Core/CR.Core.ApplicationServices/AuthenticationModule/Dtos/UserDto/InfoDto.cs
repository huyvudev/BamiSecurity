namespace CR.Core.ApplicationServices.AuthenticationModule.Dtos.UserDto
{
    public class InfoDto
    {
        /// <summary>
        /// Username
        /// </summary>
        public required string Username { get; set; } = null!;

        /// <summary>
        /// Tên đầy đủ
        /// </summary>
        public string? FullName { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Lần đầu đăng nhập
        /// </summary>
        public bool IsFirstTime { get; set; }

        /// <summary>
        /// Mật khẩu tạm thời
        /// </summary>
        public bool IsPasswordTemp { get; set; }

        /// <summary>
        /// Pin tạm thời
        /// </summary>
        public bool IsTempPin { get; set; }

        /// <summary>
        /// Mã người dùng
        /// </summary>
        public string? UserCode { get; set; }

        /// <summary>
        /// Lần đăng nhập cuối
        /// </summary>
        public DateTime? LastLogin { get; set; }

        /// <summary>
        /// Hệ điều hành login cuối
        /// </summary>
        public string? OperatingSystem { get; set; }

        /// <summary>
        /// Trình duyệt login cuối
        /// </summary>
        public string? Browser { get; set; }

        /// <summary>
        /// Avatar
        /// </summary>
        public string? AvatarImageUri { get; set; }
    }
}
