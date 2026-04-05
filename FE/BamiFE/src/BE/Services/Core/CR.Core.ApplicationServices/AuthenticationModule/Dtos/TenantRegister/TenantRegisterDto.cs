using System.ComponentModel.DataAnnotations;
using CR.ApplicationBase.Localization;
using CR.DtoBase.Validations;

namespace CR.Core.ApplicationServices.AuthenticationModule.Dtos.TenantRegister;

public class TenantRegisterDto
{
    private string _tenantName = null!;

    [CustomRequired(AllowEmptyStrings = false)]
    [CustomMaxLength(128)]
    [CustomMinLength(3)]
    public string TenantName
    {
        get => _tenantName;
        set => _tenantName = value.Trim();
    }

    private string _tenantDomain = null!;

    /// <summary>
    /// tên miền của tenant chỉ cần truyền vào sub domain ví dụ abc thì sẽ được hiểu là abc.helioslms.com,
    /// không phân biệt hoa thường chỉ chấp nhận ký tự a-z, 0-9 và dấu gạch ngang
    /// </summary>
    [CustomRequired(AllowEmptyStrings = false)]
    [CustomMaxLength(50)]
    [CustomMinLength(3)]
    [RegularExpression(@"^[a-zA-Z0-9-]+$")]
    public string TenantDomain
    {
        get => _tenantDomain;
        set => _tenantDomain = value.ToLower().Trim();
    }

    private string _email = null!;

    [Email]
    [CustomMaxLength(128)]
    [CustomRequired(AllowEmptyStrings = false)]
    public string Email
    {
        get => _email;
        set => _email = value.ToLower().Trim();
    }

    private string _fullName = null!;

    [CustomMaxLength(256)]
    [CustomRequired(AllowEmptyStrings = false)]
    public string FullName
    {
        get => _fullName;
        set => _fullName = value.Trim();
    }

    /// <summary>
    /// Mật khẩu
    /// </summary>
    [CustomMaxLength(100)]
    [CustomRequired(AllowEmptyStrings = false)]
    public string Password { get; set; } = null!;

    private string? _phoneNumber = null!;

    [CustomRequired(AllowEmptyStrings = false)]
    [System.ComponentModel.DataAnnotations.Phone]
    [CustomMaxLength(20)]
    public string? PhoneNumber
    {
        get => _phoneNumber;
        set => _phoneNumber = value?.Trim();
    }

    private string? _language;
    /// <summary>
    /// Ngôn ngữ mặc định của tenant
    /// </summary>
    [ConstStrings(typeof(LocalizationNames))]
    public string? Language
    {
        get => _language;
        set => _language = value?.Trim();
    }
}
