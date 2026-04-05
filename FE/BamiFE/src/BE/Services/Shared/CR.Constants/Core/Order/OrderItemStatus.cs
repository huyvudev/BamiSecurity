namespace CR.Constants.Core.Order;

public enum OrderItemStatus
{

    /// <summary>
    /// Chưa in
    /// </summary>
    Unprinted = 0,

    /// <summary>
    /// Đã đẩy
    /// </summary>
    Pushed = 1,

    /// <summary>
    /// Đã in
    /// </summary>
    Printed = 2,

    /// <summary>
    /// Đã cắt
    /// </summary>
    Cut = 3,

    /// <summary>
    /// Đã khắc
    /// </summary>
    Engraved = 4,

    /// <summary>
    /// Đã kiểm tra chất lượng
    /// </summary>
    QualityChecked = 5,

    /// <summary>
    /// Đã đóng gói
    /// </summary>
    Packaged = 6,

    /// <summary>
    /// Đã ship
    /// </summary>
    Shipped = 7,

    /// <summary>
    /// Hủy ship
    /// </summary>
    CancelShip = 8,
}
