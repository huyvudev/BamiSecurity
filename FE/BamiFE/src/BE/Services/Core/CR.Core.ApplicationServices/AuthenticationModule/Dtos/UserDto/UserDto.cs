using CR.Constants.Authorization.Role;
using CR.Constants.Core.Users;

namespace CR.Core.ApplicationServices.AuthenticationModule.Dtos.UserDto
{
    public class UserDto
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
        public GenderTypes? Gender { get; set; }

        /// <summary>
        /// Avatar người dùng
        /// </summary>
        public string? AvatarImageUri { get; set; }

        /// <summary>
        /// Số điện thoại
        /// </summary>
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Loại tài khoản
        /// </summary>
        public UserTypeEnum UserType { get; set; }

        public int Status { get; set; }
        public string? Email { get; set; }
        public bool IsPasswordTemp { get; set; }
        public IEnumerable<UserRoleDto>? Roles { get; set; }
    }

    public class UserRoleDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        /// <summary>
        /// Trạng thái A/D
        /// </summary>
        /// <see cref="Constants.Authorization.Role.RoleStatus"/>
        public int Status { get; set; }

        /// <summary>
        /// Quyền thuộc Web nào
        /// <see cref="PermissionInWebs"/>
        /// </summary>
        public int PermissionInWeb { get; set; }
    }
}
