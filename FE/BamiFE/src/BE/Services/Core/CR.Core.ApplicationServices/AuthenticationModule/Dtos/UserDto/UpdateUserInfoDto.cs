namespace CR.Core.ApplicationServices.AuthenticationModule.Dtos.UserDto;

public class UpdateUserInfoDto : UpdateFullNameUserDto
{
    private string? _avatarImageUri;
    public string? AvatarImageUri
    {
        get => _avatarImageUri;
        set => _avatarImageUri = value?.Trim();
    }
}
