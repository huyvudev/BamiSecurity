using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CR.Constants.Core.Order;

/// <summary>
/// Đại diện cho trạng thái của đơn hàng trong quá trình vận chuyển.
/// </summary>
public enum ShippingStatus
{
    /// <summary>
    /// Đơn hàng đã sẵn sàng để vận chuyển nhưng chưa được gửi đi.
    /// </summary>
    ReadyForShipping = 0,

    /// <summary>
    /// Đơn hàng đang được vận chuyển đến trung tâm hoặc điểm đến.
    /// </summary>
    Shipping = 1,

    /// <summary>
    /// Đơn hàng đang được giao đến địa chỉ cuối cùng.
    /// </summary>
    OutForDelivery = 2,

    /// <summary>
    /// Đơn hàng đã được giao thành công cho người nhận.
    /// </summary>
    Delivered = 3,

    /// <summary>
    /// Giao hàng thất bại, đơn hàng không thể giao đến người nhận.
    /// </summary>
    DeliveryFailed = 4,

    /// <summary>
    /// Đơn hàng đã được trả lại cho người gửi.
    /// </summary>
    Returned = 5,

    /// <summary>
    /// Quá trình vận chuyển đã bị hủy và đơn hàng sẽ không tiếp tục.
    /// </summary>
    Canceled = 6,
}
