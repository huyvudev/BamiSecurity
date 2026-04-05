namespace CR.Constants.Core.Order;

/// <summary>
/// Đại diện cho trạng thái tổng thể của đơn hàng trong quy trình sản xuất.
/// Khi xử lý logic trạng thái phải xử lý theo hướng ưu tiên từ trên xuống dưới nếu gặp điều kiện nào
/// (điều kiện ít nhất một của từng status) thì dừng và trả về status đó luôn
/// </summary>
public enum OrderStatus
{
    /// <summary>
    /// Đơn hàng đang chờ xử lý import (do thông tin chưa đầy đủ hoặc file bị lỗi không tải về được).
    /// Khi nào đơn hàng được xử lý thành công cho mọi item thì mới chuyển tất cả sang bước thiết kế (clone từ order detail sang order item)
    /// </summary>
    PendingImported = 0,

    /// <summary>
    /// Đẩy đơn sang cho sản xuất (khi tất cả các file thành công và bắt đầu muốn sản xuất)
    /// </summary>
    PushedOrder = 1,

    /// <summary>
    /// Đơn hàng đang chờ thiết kế (ít nhất một mục trong đơn hàng chưa có thiết kế).
    /// </summary>
    PendingDesign = 2,

    /// <summary>
    /// Tất cả các mục trong đơn hàng đã hoàn tất thiết kế và sẵn sàng để sản xuất.
    /// </summary>
    ReadyForProduction = 3,

    /// <summary>
    /// Ít nhất một mục trong đơn hàng đang được sản xuất.
    /// </summary>
    InProduction = 4,

    /// <summary>
    /// Tất cả các mục trong đơn hàng đã hoàn tất sản xuất và đã đóng gói kèm in label.
    /// </summary>
    Packaged = 5,

    /// <summary>
    /// Đơn hàng đã bị hủy và sẽ không tiếp tục thực hiện.
    /// </summary>
    Canceled = 6
}
