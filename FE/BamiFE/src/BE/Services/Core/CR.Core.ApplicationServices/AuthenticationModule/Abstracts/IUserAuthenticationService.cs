using CR.Core.Domain.Users;

namespace CR.Core.ApplicationServices.AuthenticationModule.Abstracts
{
    /// <summary>
    /// Xác thực tài khoản
    /// </summary>
    public interface IUserAuthenticationService
    {
        /// <summary>
        /// Tìm kiếm User theo Id (dùng khi gọi authorization)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<User> FindUserAuthorizationById(int id);

        /// <summary>
        /// Validate user admin
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<User> ValidateAdminUser(string username, string password);

        /// <summary>
        /// Validate app user
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<User> ValidateAppUser(string username, string password);
    }
}
