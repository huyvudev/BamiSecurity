using CR.DtoBase;
using Microsoft.AspNetCore.Mvc;

namespace CR.Core.ApplicationServices.AuthenticationModule.Dtos.RoleDto
{
    public class FilterRoleDto : PagingRequestBaseDto
    {
        [FromQuery(Name = "userType")]
        public int? UserType { get; set; }

        [FromQuery(Name = "status")]
        public int? Status { get; set; }
    }
}
