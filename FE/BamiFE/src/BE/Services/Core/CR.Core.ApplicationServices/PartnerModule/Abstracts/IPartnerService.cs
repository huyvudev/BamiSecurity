using CR.Core.Dtos.PartnerModule.Partner;
using CR.Core.Dtos.PartnerModule.PartnerType;
using CR.DtoBase;

namespace CR.Core.ApplicationServices.PartnerModule.Abstracts
{
    public interface IPartnerService
    {
        /// <summary>
        /// Tìm Partner theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Result<PartnerDto>> GetById(int id);

        /// <summary>
        /// Xem danh sách Partner
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<Result<PagingResult<PartnerDto>>> FindAll(FilterPartnerDto input);

        /// <summary>
        /// Thêm Partner
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<Result<CreatePartnerResultDto>> CreatePartner(CreatePartnerDto input);

        /// <summary>
        /// Cập nhật thông tin Partner
        /// </summary>
        /// <param name="input"></param>
        Task<Result<UpdatePartnerResultDto>> Update(UpdatePartnerDto input);

        Task<Result> Delete(int id);
    }
}
