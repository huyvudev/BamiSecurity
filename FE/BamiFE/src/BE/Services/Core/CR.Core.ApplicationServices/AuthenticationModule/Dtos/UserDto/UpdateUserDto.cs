using CR.Constants.Core.Users;
using CR.DtoBase.Validations;

namespace CR.Core.ApplicationServices.AuthenticationModule.Dtos.UserDto
{
    public class UpdateUserDto
    {
        public int Id { get; set; }

        /// <summary>
        /// Thời gian xóa tài khoản theo Status = 3(LOCK)
        /// </summary>
        public DateTime? LockedStatus { get; set; }

        /// <summary>
        /// Tên đăng nhập
        /// </summary>
        private string _username = null!;
        public string Username
        {
            get => _username;
            set => _username = value.Trim();
        }

        /// <summary>
        /// Mật khẩu
        /// </summary>

        /// <summary>
        /// Tên người dùng
        /// </summary>
        private string? _fullName;
        public string? FullName
        {
            get => _fullName;
            set => _fullName = value?.Trim();
        }

        /// <summary>
        /// Loại tài khoản
        /// </summary>
        [IntegerRange(
            AllowableValues = [UserTypes.CUSTOMER, UserTypes.TENANT_ADMIN, UserTypes.ADMIN]
        )]
        public int? UserType { get; set; }

        private string? _avatarS3key;

        /// <summary>
        /// Ảnh đại diện của người dùng
        /// </summary>
        public string? AvatarS3key
        {
            get => _avatarS3key;
            set => _avatarS3key = value?.Trim();
        }

        private string? _userCode;

        /// <summary>
        /// Mã số của người dùng (có thể là mã nhân viên hoặc MSSV)
        /// </summary>
        public string? UserCode
        {
            get => _userCode;
            set => _userCode = value?.Trim();
        }

        private string? _phoneNumber;

        /// <summary>
        /// Số điện thoại của người dùng
        /// </summary>
        public string? PhoneNumber
        {
            get => _phoneNumber;
            set => _phoneNumber = value?.Trim();
        }

        /// <summary>
        /// Ngày sinh của người dùng
        /// </summary>
        public DateTime? DateOfBirth { get; set; }

        private string? _hometown;

        /// <summary>
        /// Quê quán của người dùng
        /// </summary>
        public string? Hometown
        {
            get => _hometown;
            set => _hometown = value?.Trim();
        }


        private string? _email;

        /// <summary>
        /// Email
        /// </summary>
        public string? Email
        {
            get => _email;
            set => _email = value?.Trim();
        }

        private string? _nationalId;

        /// <summary>
        /// Số ID (CCCD) của người dùng
        /// </summary>
        public string? IdCode
        {
            get => _nationalId;
            set => _nationalId = value?.Trim();
        }

        public string? PinCode { get; set; }

        /// <summary>
        /// Có phải Mã pin tạm thời không
        /// </summary>
        public bool IsTempPin { get; set; }

        /// <summary>
        /// Có phải là mật khẩu tạm, Yêu cầu thay đổi mật khẩu mới ngay khi đăng nhập
        /// </summary>
        public bool IsPasswordTemp { get; set; }
    }
}
