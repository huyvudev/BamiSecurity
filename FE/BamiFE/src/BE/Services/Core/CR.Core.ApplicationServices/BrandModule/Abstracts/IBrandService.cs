using CR.Core.Dtos.BrandModule.Brand;
using CR.DtoBase;

namespace CR.Core.ApplicationServices.BrandModule.Abstracts
{
    public interface IBrandService
    {
        /// <summary>
        /// Tìm brand theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Result<BrandDto>> GetById(int id);

        /// <summary>
        /// Xem danh sách brand
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<Result<PagingResult<BrandDto>>> FindAll(PagingRequestBaseDto input);

        /// <summary>
        /// Thêm brand
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<Result<CreateBrandResultDto>> CreateBrand(CreateBrandDto input);

        /// <summary>
        /// Cập nhật thông tin brand
        /// </summary>
        /// <param name="input"></param>
        Task<Result<UpdateBrandResultDto>> Update(UpdateBrandDto input);

        Task<Result> Delete(int id);
    }
}
