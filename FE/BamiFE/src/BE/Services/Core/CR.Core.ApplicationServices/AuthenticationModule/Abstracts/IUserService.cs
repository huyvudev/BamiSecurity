using CR.Core.ApplicationServices.AuthenticationModule.Dtos.RoleDto;
using CR.Core.ApplicationServices.AuthenticationModule.Dtos.UserActionDtos;
using CR.Core.ApplicationServices.AuthenticationModule.Dtos.UserDto;
using CR.Core.Dtos.AuthenticationModule.UserDto;
using CR.DtoBase;

namespace CR.Core.ApplicationServices.AuthenticationModule.Abstracts
{
    public interface IUserService
    {
        /// <summary>
        /// Tìm user theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Result<UserDto>> GetById(int id);

        /// <summary>
        /// Xem danh sách User CMS (tài khoản SUPER_ADMIN + ADMIN)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<Result<PagingResult<UserDto>>> FindAll(FilterUserPagingDto input);

        /// <summary>
        /// Thêm user
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<Result> CreateUser(CreateUserDto input);

        /// <summary>
        /// Cập nhật thông tin tài khoản
        /// </summary>
        /// <param name="input"></param>
        Task<Result> Update(UpdateUserDto input);

        /// <summary>
        /// Cập nhật trạng thái tài khoản
        /// </summary>
        /// <param name="id"></param>
        Task<Result> ChangeStatus(int id);

        Task<Result> Delete(int id);

        /// <summary>
        /// Set password cho user
        /// </summary>
        /// <param name="input"></param>
        Task<Result> SetPassword(SetPasswordUserDto input);

        /// <summary>
        /// Thay đổi mật khẩu
        /// </summary>
        /// <param name="input"></param>
        Task<Result> ChangePassword(ChangePasswordDto input);

        Task<Result> UpdateUserFullName(UpdateFullNameUserDto input);

        /// <summary>
        /// Lưu thông tin ngày gần nhất + thiết bị khi đăng nhập
        /// </summary>
        /// <param name="userId"></param>
        Task<Result> LoginInfor(int userId);

        /// <summary>
        /// Cập nhật ảnh đại diện cho tài khoản người dùng
        /// </summary>
        /// <param name="s3Key"></param>
        Task<Result> UpdateAvatar(string s3Key);

        /// <summary>
        /// Xem giới hạn số lần nhập
        /// </summary>
        /// <param name="varName"></param>
        /// <returns></returns>
        Task<Result<int>> GetLimitedInputTurn(string varName);

        /// <summary>
        /// Cập nhật trạng thái user
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="userStatus"></param>
        Task<Result> UpdateUserStatus(string userName, int userStatus);

        /// <summary>
        /// Cập nhật trạng thái user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userStatus"></param>
        Task<Result> UpdateUserStatus(int userId, int userStatus);

        /// <summary>
        /// Cập nhật role theo userId
        /// </summary>
        /// <param name="input"></param>
        Task<Result> UpdateRole(UpdateRoleDto input);

        /// <summary>
        /// Thông tin tài khoản từ userId token
        /// </summary>
        /// <returns></returns>
        Task<Result<UserDto>> FindByCurrentId();

        /// <summary>
        /// Đăng kí tài khoản người dùng
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<Result<UserDto>> RegisterUser(UserRegisterDto input);
    }
}
