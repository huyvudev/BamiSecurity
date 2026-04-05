using CR.DtoBase;
using Microsoft.AspNetCore.Mvc;

namespace CR.Core.ApplicationServices.AuthenticationModule.Dtos.UserDto
{
    public class FilterUserPagingDto : PagingRequestBaseDto
    {
        [FromQuery(Name = "username")]
        public string? Username { get; set; }

        [FromQuery(Name = "fullname")]
        public string? FullName { get; set; }

        [FromQuery(Name = "status")]
        public int? Status { get; set; }

        [FromQuery(Name = "usertype")]
        public int? UserType { get; set; }
    }
}
