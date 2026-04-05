using CR.DtoBase.Validations;

namespace CR.Core.ApplicationServices.AuthenticationModule.Dtos.UserDto;

public class ChangeEmailDto
{
    private string _email = null!;
    [Email]
    [CustomMaxLength(128)]
    [CustomRequired(AllowEmptyStrings = false)]
    public string Email
    {
        get => _email;
        set => _email = value.Trim();
    }

    
}

public class VerifyChangeEmailDto : ChangeEmailDto
{
    private string? _otp;
    public string? Otp
    {
        get => _otp;
        set => _otp = value?.Trim();
    }
}