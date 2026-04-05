using Microsoft.AspNetCore.Http;

namespace CR.Core.ApplicationServices.AuthenticationModule.Dtos.UserActionDtos
{
    public class AppUpdateAvatarDto
    {
        /// <summary>
        /// File ảnh đại diện
        /// </summary>
        public IFormFile? AvatarImage { get; set; }
    }
}
