using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CR.Constants.Authorization.Tenant;
using CR.Constants.Core.Users;

namespace CR.Core.Dtos.AuthenticationModule.UserDto;

public class TenantUserDto
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

    /// <summary>
    /// Ngày sinh của người dùng
    /// </summary>
    public DateTime? DateOfBirth { get; set; }

    /// <summary>
    /// Quê quán của người dùng
    /// </summary>
    public string? Hometown { get; set; }

    /// <summary>
    /// Số ID (CCCD) của người dùng
    /// </summary>
    public string? IdCode { get; set; }

    /// <summary>
    /// Loại tài khoản
    /// </summary>
    public int? UserType { get; set; }

    public int Status { get; set; }
    public string? Email { get; set; }

    public bool IsPasswordTemp { get; set; }
    public IEnumerable<TenantRoleFix>? Roles { get; set; }
}

public class TenantUserRoleDto
{
    public string? RoleName { get; set; }
    public TenantRoleFix RoleType { get; set; }
}
