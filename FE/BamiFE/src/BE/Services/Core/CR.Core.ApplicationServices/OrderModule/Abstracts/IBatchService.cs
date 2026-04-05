using CR.Core.Dtos.BatchModule;
using CR.Core.Dtos.Note;
using CR.DtoBase;

namespace CR.Core.ApplicationServices.OrderModule.Abstracts;

public interface IBatchService
{
    /// <summary>
    /// Tạo lô (các OrderItem cùng SKU)
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<Result> Create(CreateBatchDto input);

    /// <summary>
    /// Cập nhật lô
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<Result> Update(UpdateBatchDto input);

    /// <summary>
    /// Xóa lô
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Result> Delete(int id);

    /// <summary>
    /// Tìm lô theo id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Result<FindBatchDto>> FindById(int id);

    /// <summary>
    /// Danh sách lô
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<Result<PagingResult<BatchDto>>> FindAll(FilterBatchDto input);

    /// <summary>
    /// Cập nhật trạng thái item trong lô
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<Result> UpdateItemStatus(UpdateBatchItemStatusDto input);

    /// <summary>
    /// Lấy danh sách tất cả item trong các lô
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<Result<List<BatchItemDto>>> FindOrderItemByBatchIds(FindItemsByBatchDto input);

    /// <summary>
    /// Cập nhật ghi chú cho lô
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<Result> UpdateNote(UpdateNoteDto input);
}