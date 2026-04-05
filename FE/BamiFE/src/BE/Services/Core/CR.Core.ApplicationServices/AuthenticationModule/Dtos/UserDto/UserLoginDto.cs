using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CR.Core.ApplicationServices.AuthenticationModule.Dtos.UserDto
{
    public class UserLoginDto
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
        /// Loại tài khoản
        /// </summary>
        public int? UserType { get; set; }

        public int Status { get; set; }

        /// <summary>
        /// Ngày sinh
        /// </summary>
        public DateTime? DateOfBirth { get; set; }
        public bool IsPasswordTemp { get; set; }
        public IEnumerable<string>? RoleNames { get; set; }
        public IEnumerable<int>? RoleIds { get; set; }
        public DateTime DateTimeLoginFailCount { get; set; }
        public int LoginFailCount { get; set; }
    }
}
