using CR.DtoBase.Validations;

namespace CR.Core.ApplicationServices.AuthenticationModule.Dtos.UserActionDtos
{
    /// <summary>
    /// Thiết lập mật khẩu khi đăng ký tài khoản
    /// </summary>
    public class UserRegisterDto
    {
        private string _username = null!;

        [Email]
        [CustomRequired(AllowEmptyStrings = false)]
        public required string Username
        {
            get => _username;
            set => _username = value.Trim();
        }

        private string _email = null!;

        [Email]
        [CustomRequired(AllowEmptyStrings = false)]
        public required string Email
        {
            get => _email;
            set => _email = value.Trim();
        }
        private string _userCode = null!;

        /// <summary>
        /// Mã số của người dùng (có thể là mã nhân viên hoặc MSSV)
        /// </summary>
        public string UserCode
        {
            get => _userCode;
            set => _userCode = value.Trim();
        }
    }
}
