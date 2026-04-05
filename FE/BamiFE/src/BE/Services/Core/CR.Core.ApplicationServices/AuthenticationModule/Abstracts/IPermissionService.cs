using CR.Core.ApplicationServices.AuthenticationModule.Dtos.PermissionDto;

namespace CR.Core.ApplicationServices.AuthenticationModule.Abstracts
{
    public interface IPermissionService
    {
        /// <summary>
        /// Kiểm tra permissionKey của người dùng hiện tại
        /// </summary>
        /// <returns></returns>
        bool CheckPermission(params string[] permissionKeys);

        /// <summary>
        /// Lấy danh sách tên quyền theo web
        /// </summary>
        /// <param name="permissionConfig"></param>
        /// <returns></returns>
        IEnumerable<PermissionDetailDto> FindAllPermission(int permissionConfig);

        ///// <summary>
        ///// Lấy số nhóm quyền, số người dùng theo website
        ///// </summary>
        ///// <returns></returns>
        //IEnumerable<PermissionInWebDto> FindByPermissionInWeb();

        /// <summary>
        /// Lấy permissionKey của người dùng hiện tại (lọc theo website)
        /// </summary>
        /// <param name="permissionInWeb"></param>
        /// <returns></returns>
        IEnumerable<string> GetPermissionInWeb(int? permissionInWeb);

        /// <summary>
        /// Lấy permissionKey của người dùng bấtt kì
        /// </summary>
        /// <param name="permissionInWeb"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        IEnumerable<string> GetPermissionInternalService(int? permissionInWeb, int userId);
    }
}
