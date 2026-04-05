using CR.Constants.Core.Users;
using CR.DtoBase.Validations;

namespace CR.Core.Dtos.AuthenticationModule.UserDto
{
    public class CreateUserDto
    {
        private string _username = null!;

        /// <summary>
        /// Tên tài khoản
        /// </summary>
        [CustomMaxLength(128)]
        [CustomRequired(AllowEmptyStrings = false)]
        public required string Username
        {
            get => _username;
            set => _username = value.Trim();
        }

        /// <summary>
        /// Mật khẩu
        /// </summary>
        [CustomMaxLength(128)]
        [CustomRequired(AllowEmptyStrings = false)]
        public required string Password { get; set; }

        /// <summary>
        /// Có phải là mật khẩu tạm, Yêu cầu thay đổi mật khẩu mới ngay khi đăng nhập
        /// </summary>
        public bool IsPasswordTemp { get; set; } = true;

        /// <summary>
        /// Tên người dùng
        /// </summary>
        private string? _fullName;

        [CustomMaxLength(256)]
        public string? FullName
        {
            get => _fullName;
            set => _fullName = value?.Trim();
        }

        /// <summary>
        /// Email
        /// </summary>
        private string? _email;

        [CustomMaxLength(256)]
        [Email]
        public string? Email
        {
            get => _email;
            set => _email = value?.Trim();
        }

        private string? _avatarS3key;

        /// <summary>
        /// Ảnh đại diện của người dùng
        /// </summary>
        public string? AvatarS3key
        {
            get => _avatarS3key;
            set => _avatarS3key = value?.Trim();
        }

        /// <summary>
        /// Giới tính của người dùng
        /// </summary>
        [ListEnumDataType(typeof(GenderTypes))]
        public GenderTypes Gender { get; set; }

        private string _userCode = null!;

        /// <summary>
        /// Mã số của người dùng (có thể là mã nhân viên hoặc MSSV)
        /// </summary>
        [CustomMaxLength(500)]
        public string UserCode
        {
            get => _userCode;
            set => _userCode = value.Trim();
        }

        private string? _phoneNumber;

        /// <summary>
        /// Số điện thoại của người dùng
        /// </summary>
        [CustomMaxLength(20)]
        public string? PhoneNumber
        {
            get => _phoneNumber;
            set => _phoneNumber = value?.Trim();
        }

        /// <summary>
        /// Ngày sinh của người dùng
        /// </summary>
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Role gán cho user
        /// </summary>
        public int[]? RoleIds { get; set; }
    }
}
