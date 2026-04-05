using CR.Constants.Core.Users;

namespace CR.Core.ApplicationServices.AuthenticationModule.Dtos.UserDto;

public class CreateUserExcelDto
{
    public required string Email { get; set; }

    /// <summary>
    /// Mật khẩu
    /// </summary>
    public required string Password { get; set; }

    /// <summary>
    /// Tên người dùng
    /// </summary>
    public string? FullName { get; set; }

    /// <summary>
    /// Giới tính
    /// </summary>
    public GenderTypes Gender { get; set; }

    /// <summary>
    /// Mã số (có thể là mã nhân viên hoặc MSSV)
    /// </summary>
    public string? UserCode { get; set; }

    /// <summary>
    /// Số điện thoại
    /// </summary>
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Ngày sinh
    /// </summary>
    public DateTime? DateOfBirth { get; set; }

    /// <summary>
    /// Quê quán
    /// </summary>
    public string? Hometown { get; set; }

    /// <summary>
    /// Số ID (CCCD)
    /// </summary>
    public string? IdCode { get; set; }
}
