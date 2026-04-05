using CR.Core.Dtos.BrandModule.Store;
using CR.DtoBase;

namespace CR.Core.ApplicationServices.BrandModule.Abstracts;

public interface IStoreService
{
    /// <summary>
    /// Tìm store theo Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Result<StoreDto>> GetById(int id);

    /// <summary>
    /// Xem danh sách store
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<Result<PagingResult<StoreDto>>> FindAll(PagingRequestBaseDto input);

    /// <summary>
    /// Thêm store
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<Result<CreateStoreResultDto>> CreateStore(CreateStoreDto input);

    /// <summary>
    /// Cập nhật thông tin store
    /// </summary>
    /// <param name="input"></param>
    Task<Result<UpdateStoreResultDto>> Update(UpdateStoreDto input);

    Task<Result> Delete(int id);
}
