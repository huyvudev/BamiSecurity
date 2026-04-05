using CR.Constants.Core.Users;
using CR.DtoBase.Validations;

namespace CR.Core.ApplicationServices.AuthenticationModule.Dtos.UserRegister;

/// <summary>
/// User đăng ký vào tenant
/// </summary>
public class RegisterIntoTenantDto
{
    private string _email = null!;

    /// <summary>
    /// Tên tài khoản
    /// </summary>
    [Email]
    [CustomMaxLength(128)]
    [CustomRequired(AllowEmptyStrings = false)]
    public string Email
    {
        get => _email;
        set => _email = value.Trim();
    }

    /// <summary>
    /// Mật khẩu
    /// </summary>
    [CustomMaxLength(100)]
    [CustomRequired(AllowEmptyStrings = false)]
    public string Password { get; set; } = null!;

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
    /// Giới tính của người dùng
    /// </summary>
    [ListEnumDataType(typeof(GenderTypes))]
    public GenderTypes? Gender { get; set; }

    private string? _userCode;

    /// <summary>
    /// Mã số của người dùng (có thể là mã nhân viên hoặc MSSV)
    /// </summary>
    [CustomMaxLength(500)]
    public string? UserCode
    {
        get => _userCode;
        set => _userCode = value?.Trim();
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

    private string? _hometown;

    /// <summary>
    /// Quê quán của người dùng
    /// </summary>
    [CustomMaxLength(500)]
    public string? Hometown
    {
        get => _hometown;
        set => _hometown = value?.Trim();
    }

    private string? _idCode;

    /// <summary>
    /// Số ID (CCCD) của người dùng
    /// </summary>
    [CustomMaxLength(125)]
    public string? IdCode
    {
        get => _idCode;
        set => _idCode = value?.Trim();
    }
}
