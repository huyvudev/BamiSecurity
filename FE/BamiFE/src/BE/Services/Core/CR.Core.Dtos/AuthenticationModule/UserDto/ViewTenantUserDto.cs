using CR.Constants.Core.Users;

namespace CR.Core.Dtos.AuthenticationModule.UserDto;

public class ViewTenantUserDto
{
    /// <summary>
    /// Id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Tên đăng nhập
    /// </summary>
    public required string Username { get; set; }

    /// <summary>
    /// Tên người dùng
    /// </summary>
    public string? FullName { get; set; }

    /// <summary>
    /// Avatar người dùng
    /// </summary>
    public string? AvatarImageUri { get; set; }

    /// <summary>
    /// Giới tính của người dùng
    /// </summary>
    public GenderTypes? Gender { get; set; }

    /// <summary>
    /// Mã số của người dùng (có thể là mã nhân viên hoặc MSSV)
    /// </summary>
    public string? UserCode { get; set; }

    /// <summary>
    /// Số điện thoại của người dùng
    /// </summary>
    public string? PhoneNumber { get; set; }
    public int Status { get; set; }
    /// <summary>
    /// Loại tài khoản
    /// </summary>
    public int UserType { get; set; }
    public string? Email { get; set; }
}
