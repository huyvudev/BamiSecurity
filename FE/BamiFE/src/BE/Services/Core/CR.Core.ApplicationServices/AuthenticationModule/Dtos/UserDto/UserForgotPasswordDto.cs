using CR.DtoBase.Validations;
using System.ComponentModel.DataAnnotations;

namespace CR.Core.ApplicationServices.AuthenticationModule.Dtos.UserDto;

/// <summary>
/// Quên mật khẩu
/// </summary>
public class UserForgotPasswordDto
{
    private string _email = null!;

    /// <summary>
    /// Email
    /// </summary>
    [CustomRequired]
    [CustomMaxLength(128)]
    public string Email
    {
        get => _email;
        set => _email = value.Trim();
    }

    private string _code = null!;

    /// <summary>
    /// Mã secret quên mật khẩu
    /// </summary>
    [CustomRequired]
    [CustomMaxLength(128)]
    public string Code
    {
        get => _code;
        set => _code = value.Trim();
    }

    private string _newPassword = null!;

    /// <summary>
    /// Mật khẩu mới
    /// </summary>
    [CustomRequired]
    [CustomMaxLength(128)]
    public string NewPassword
    {
        get => _newPassword;
        set => _newPassword = value.Trim();
    }

    private string _confirmPassword = null!;

    /// <summary>
    /// Xác nhận mật khẩu mới
    /// </summary>
    [CustomRequired]
    [CustomMaxLength(128)]
    public string ConfirmPassword
    {
        get => _confirmPassword;
        set => _confirmPassword = value.Trim();
    }
}
