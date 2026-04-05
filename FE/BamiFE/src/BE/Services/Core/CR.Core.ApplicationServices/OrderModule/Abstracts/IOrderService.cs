using CR.Core.Dtos.OrderModule.OrderDto;
using CR.DtoBase;

namespace CR.Core.ApplicationServices.OrderModule.Abstracts;

public interface IOrderService
{
    /// <summary>
    /// Quản trị tự tạo order
    /// </summary>
    /// <returns></returns>
    Task<Result<CreateOrderResultDto>> CreateOrder(CreateOrderDto input);

    /// <summary>
    /// Xem danh sách đơn hàng phân trang kèm trạng thái đơn hàng và không có chi tiết
    /// </summary>
    /// <returns></returns>
    Task<Result<PagingResult<OrderBaseDto>>> FindAllOrder(FilterOrderDto input);

    /// <summary>
    /// Xem chi tiết đơn hàng
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Result<OrderDto>> FindById(int id);

    /// <summary>
    /// Xem danh sách đơn hàng đang chờ
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<Result<PagingResult<OrderBaseDto>>> FindAllOrderPending(PagingRequestBaseDto input);

    /// <summary>
    /// Cập nhật đơn hàng
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<Result<UpdateOrderResultDto>> UpdateOrder(UpdateOrderDto input);

    /// <summary>
    /// Cập nhật thông tin item, thêm các trường thông tin bắt buộc về kích cỡ, design front, sku, namespace (là tên của store)
    /// và một số trường không bắt buộc như brand, note. item sau khi đi qua bước duyệt thiết kế thì không cho update nữa
    /// </summary>
    /// <returns></returns>
    Task<Result> UpdateOrderItem();

    /// <summary>
    /// Ghi chú cho item
    /// </summary>
    /// <returns></returns>
    Task<Result> UpdateOrderItemNote();

    /// <summary>
    /// Duyệt design cho item để chuyển sang quy trình sản xuất
    /// </summary>
    /// <param name="orderItemId"></param>
    /// <returns></returns>
    Task<Result> ApproveDesignItem(int orderItemId);

    /// <summary>
    /// Duyệt đơn hàng khi tất cả detail Done
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Result> ApproveOrder(int id);

    Task<Result> Delete(int id);
}
