using CR.Constants.Core.Order;
using CR.DtoBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CR.Core.ApplicationServices.OrderModule.Abstracts;

/// <summary>
/// Logic trạng thái order theo order item
/// </summary>
public interface IOrderStatusService
{
    /// <summary>
    /// Tính toán trạng thái của đơn hàng dựa trên mức độ ưu tiên theo <see cref="OrderStatus"/>.
    /// Hàm này được gọi khi cần tính toán lại order status dựa trên trạng thái của các item <see cref="OrderItemStatus"/>
    /// </summary>
    /// <param name="orderId">ID của đơn hàng cần tính toán trạng thái</param>
    /// <returns>Trạng thái mới của đơn hàng (OrderStatus)</returns>
    Task<Result<OrderStatus>> CalculateOrderStatus(int orderId);
}
