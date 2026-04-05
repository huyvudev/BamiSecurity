namespace CR.Constants.Core.Order;

public enum FilePrintStatus
{
    /// <summary>
    /// Chờ xử lý
    /// </summary>
    Pending = 0,

    /// <summary>
    /// Đang xử lý
    /// </summary>
    Processing = 1,

    /// <summary>
    /// Đợi duyệt
    /// </summary>
    Review = 2,

    /// <summary>
    /// Đã xong một phần
    /// </summary>
    Done = 3,

    /// <summary>
    /// Đã xong
    /// </summary>
    FullyDone = 4,

    /// <summary>
    /// Tạm dùng
    /// </summary>
    OnHold = 5,

    /// <summary>
    /// Tạo lại
    /// </summary>
    Retry = 6,

    /// <summary>
    /// Lỗi
    /// </summary>
    Error = 7,

    /// <summary>
    /// Đã hủy
    /// </summary>
    Cancelled = 8,
}
