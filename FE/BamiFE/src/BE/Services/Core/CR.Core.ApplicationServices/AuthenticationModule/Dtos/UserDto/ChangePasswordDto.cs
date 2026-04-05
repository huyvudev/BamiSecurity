using CR.DtoBase.Validations;

namespace CR.Core.ApplicationServices.AuthenticationModule.Dtos.UserDto
{
    public class ChangePasswordDto
    {
        [CustomMaxLength(128)]
        public string OldPassword { get; set; } = null!;

        [CustomMaxLength(128)]
        [CustomRequired(AllowEmptyStrings = false)]
        public string NewPassword { get; set; } = null!;
    }
}
