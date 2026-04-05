using CR.DtoBase.Validations;

namespace CR.Core.ApplicationServices.AuthenticationModule.Dtos.UserActionDtos
{
    public class AppChangePasswordFirstTimeDto
    {
        /// <summary>
        /// Mật khẩu thay đổi
        /// </summary>
        [CustomRequired(AllowEmptyStrings = false)]
        public required string Password { get; set; }
    }
}
