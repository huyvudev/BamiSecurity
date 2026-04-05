namespace CR.Constants.Core.Order;

/// <summary>
/// Các trạng thái của lô sản xuất trong quá trình sản xuất.
/// Chú ý trạng thái chưa hoàn thành thì FE sẽ truyền status từ Pending đến QualityCheck
/// </summary>
public enum BatchStatus
{
    /// <summary>
    /// Lô sản xuất đang chờ xử lý, ít nhất một item trong lô chưa bắt đầu gia công.
    /// </summary>
    Pending = 0,

    /// <summary>
    /// Tất cả các item trong lô đã hoàn thành công đoạn in ấn.
    /// </summary>
    Printed = 1,

    /// <summary>
    /// Tất cả các item trong lô đã hoàn thành công đoạn cắt.
    /// </summary>
    Cut = 2,

    /// <summary>
    /// Tất cả các item trong lô đã hoàn thành công đoạn khắc.
    /// </summary>
    Engraved = 3,

    /// <summary>
    /// Đang kiểm tra chất lượng (Quality Check).
    /// </summary>
    QualityCheck = 4,

    /// <summary>
    /// Tất cả các item trong lô đã hoàn thành và có thể chuyển sang giai đoạn tiếp theo (đóng gói, vận chuyển).
    /// </summary>
    Finished = 5
}


