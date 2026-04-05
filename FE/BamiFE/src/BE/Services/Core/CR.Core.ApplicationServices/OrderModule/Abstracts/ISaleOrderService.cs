using CR.Core.Dtos.OrderModule.SaleOrder;
using CR.DtoBase;

namespace CR.Core.ApplicationServices.OrderModule.Abstracts;

/// <summary>
/// Sale tạo đơn hàng
/// </summary>
public interface ISaleOrderService
{
    /// <summary>
    /// Tạo order với file đã upload trực tiếp lên hệ thống
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<Result<CreateSaleOrderResultDto>> CreateOrder(CreateSaleOrderDto input);

    /// <summary>
    /// Lấy danh sách đơn hàng phân trang, bộ lọc có lọc trạng thái đơn hàng
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<Result> FindAllOrder(PagingRequestBaseDto input);

    /// <summary>
    /// Xem chi tiết đơn hàng
    /// </summary>
    /// <param name="orderId"></param>
    /// <returns></returns>
    Task<Result> FindOrderById(int orderId);

    /// <summary>
    /// Tải lênh danh sách order với file từ link
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<Result> UploadOrder(CreateSaleOrderDto input);

    /// <summary>
    /// Đẩy đơn đi sản xuất 
    /// </summary>
    /// <param name="orderId"></param>
    /// <returns></returns>
    Task<Result> PushOrder(int orderId);
}