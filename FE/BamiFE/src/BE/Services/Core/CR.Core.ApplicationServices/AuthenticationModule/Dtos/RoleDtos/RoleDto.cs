using CR.Constants.Core.Users;
using CR.Constants.RolePermission;
using CR.Core.Domain.Users;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CR.Core.ApplicationServices.AuthenticationModule.Dtos.RoleDto
{
    public class RoleDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Tên Role
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// <see cref="UserTypes"/>
        /// </summary>
        public int UserType { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        public string? Description { get; set; }
        public int PermissionInWeb { get; set; }
        public int Status { get; set; }
        public int TotalUse { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedByUserName { get; set; }
        public List<string> PermissionKeys { get; set; } = new();
    }

    public class RolePermissionDetailDto
    {
        public int Id { get; set; }
        public string PermissionKey { get; set; } = null!;
    }
}

