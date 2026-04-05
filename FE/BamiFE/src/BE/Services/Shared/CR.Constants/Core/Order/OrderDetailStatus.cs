namespace CR.Constants.Core.Order;

/// <summary>
/// Trạng thái ghi nhận thông tin chi tiết đơn hàng
/// </summary>
public enum OrderDetailStatus
{
    /// <summary>
    /// Chờ xử lý
    /// </summary>
    Pending = 0,
    /// <summary>
    /// Đang xử lý (xử lý file hoặc các trường thông tin)
    /// </summary>
    Processing = 1,
    /// <summary>
    /// Thất bại
    /// </summary>
    Failed = 2,
    /// <summary>
    /// Thành công
    /// </summary>
    Done = 3,
    /// <summary>
    /// Huỷ
    /// </summary>
    Cancelled = 4,
    /// <summary>
    /// đã duyệt
    /// </summary>
    Approved =5,

}
