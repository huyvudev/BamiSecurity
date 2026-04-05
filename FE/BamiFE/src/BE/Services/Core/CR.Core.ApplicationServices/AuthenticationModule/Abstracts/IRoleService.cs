using CR.Core.ApplicationServices.AuthenticationModule.Dtos.RoleDto;
using CR.DtoBase;

namespace CR.Core.ApplicationServices.AuthenticationModule.Abstracts
{
    public interface IRoleService
    {
        /// <summary>
        /// Thêm Role
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<Result> Add(CreateRolePermissionDto input);

        /// <summary>
        /// Cập nhật Role
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<Result<RoleDto>> Update(UpdateRolePermissionDto input);

        /// <summary>
        /// Find Role
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Result<RoleDto>> FindById(int id);

        /// <summary>
        /// Xóa Role
        /// </summary>
        /// <param name="id"></param>
        Task<Result> Delete(int id);

        /// <summary>
        /// Xem danh sách Role
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<Result<PagingResult<RoleDto>>> FindAll(FilterRoleDto input);

        /// <summary>
        /// Khoá role
        /// </summary>
        /// <param name="id"></param>
        Task<Result> ChangeStatus(int id);

        /// <summary>
        /// Lấy danh sách web user có quyền
        /// </summary>
        /// <returns></returns>
        Task<Result<IEnumerable<int>>> FindAllWebByUser();

        /// <summary>
        /// Lấy danh sách role theo userId
        /// </summary>
        /// <returns></returns>
        Task<Result<IEnumerable<RoleDto>>> FindRoleByUser(int userId);

        /// <summary>
        /// Danh sách tất cả role không chia quyền theo web
        /// </summary>
        /// <returns></returns>
        Task<Result<IEnumerable<RoleDto>>> GetAll();
    }
}
