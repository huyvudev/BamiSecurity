using CR.Core.Dtos.Note;
using CR.Core.Dtos.OrderModule.Item;
using CR.Core.Dtos.OrderModule.Order;
using CR.DtoBase;

namespace CR.Core.ApplicationServices.ItemModule.Abstracts
{
    public interface IOrderItemService
    {
        /// <summary>
        /// Xem danh sách các item phân trang sắp xếp sao cho các item cùng đơn hàng xếp cạnh nhau theo thứ tự
        /// item index giảm dần, ngoài ra các item phải xếp theo thứ tự thời gian tạo giảm dần
        /// </summary>
        /// <returns></returns>
        Task<Result<PagingResult<OrderItemDto>>> FindAllOrderItem(FilterOrderItemDto input);

        /// <summary>
        /// Xem chi tiết item
        /// </summary>
        /// <param name="orderItemId"></param>
        /// <returns></returns>
        Task<Result<OrderItemDto>> FindOrderItemById(int orderItemId);

        /// <summary>
        /// Xem chi tiết item theo template
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<Result<OrderItemDto>> FindOrderItemByTemplate(FindOrderItemDto input);

        /// <summary>
        /// Cập nhật ghi chú cho item
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<Result> UpdateNote(UpdateNoteDto input);
    }
}
