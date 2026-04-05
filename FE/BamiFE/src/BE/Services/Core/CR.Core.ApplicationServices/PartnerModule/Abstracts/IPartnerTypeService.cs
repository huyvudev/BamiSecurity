using CR.Core.Dtos.PartnerModule.PartnerType;
using CR.DtoBase;

namespace CR.Core.ApplicationServices.PartnerModule.Abstracts
{
    public interface IPartnerTypeService
    {
        /// <summary>
        /// Tìm PartnerType theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Result<PartnerTypeDto>> GetById(int id);

        /// <summary>
        /// Xem danh sách PartnerType
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<Result<PagingResult<PartnerTypeDto>>> FindAll(PagingRequestBaseDto input);

        /// <summary>
        /// Thêm PartnerType
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<Result<CreatePartnerTypeResultDto>> CreatePartnerType(CreatePartnerTypeDto input);

        /// <summary>
        /// Cập nhật thông tin PartnerType
        /// </summary>
        /// <param name="input"></param>
        Task<Result<UpdatePartnerTypeResultDto>> Update(UpdatePartnerTypeDto input);

        Task<Result> Delete(int id);
    }
}
