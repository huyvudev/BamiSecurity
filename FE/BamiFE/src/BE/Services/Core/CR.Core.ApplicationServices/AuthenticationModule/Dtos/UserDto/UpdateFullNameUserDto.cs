namespace CR.Core.ApplicationServices.AuthenticationModule.Dtos.UserDto
{
    public class UpdateFullNameUserDto
    {
        private string? _fullName;
        public string? FullName
        {
            get => _fullName;
            set => _fullName = value?.Trim();
        }
    }
}
