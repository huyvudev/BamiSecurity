using CR.Constants.Common.Database;
using CR.Constants.Core.Order;
using CR.Core.Domain.Batch;
using CR.Core.Domain.Brand;
using CR.Core.Domain.FilePrint;
using CR.Core.Domain.Sku;
using CR.EntitiesBase.Interfaces;

namespace CR.Core.Domain.Order;

/// <summary>
/// Nhận từ Order detail thêm vào sau khi có thiết kế
/// </summary>
[Table(nameof(CoreOrderItem), Schema = DbSchemas.CRCore)]
public class CoreOrderItem : ICreatedBy
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    #region Link với Order
    /// <summary>
    /// Trong Order nào
    /// </summary>
    public int OrderId { get; set; }

    public CoreOrder Order { get; set; } = null!;
    #endregion

    /// <summary>
    /// Số thứ tự order ở trong item ví dụ order có cái id là 123456
    /// thì cái template của orderItem sẽ là #123456_1 cái _1 là NumberIndex biểu thì item đầu tiên trong order,
    /// giá trị barcode sẽ chính là order + index dạng 123456_1
    /// </summary>
    public int ItemIndex { get; set; }

    /// <summary>
    /// Trạng thái
    /// </summary>
    public OrderItemStatus Status { get; set; }

    /// <summary>
    /// Ghi chú
    /// </summary>
    [MaxLength(512)]
    public string? Note { get; set; }

    /// <summary>
    /// Trong lô nào, nếu null là chưa phân lô
    /// </summary>
    public int? BatchId { get; set; }
    public CoreBatch? Batch { get; set; }
    #region Link với filePrint
    /// <summary>
    /// file in (file design)
    /// </summary>
    public CoreFilePrint? FilePrint { get; set; }
    #endregion

    #region Link với OrderDetail
    public int OrderDetailId { get; set; }
    public CoreOrderDetail OrderDetail { get; set; } = null!;
    #endregion

    #region audit
    public DateTime? CreatedDate { get; set; }
    public int? CreatedBy { get; set; }
    #endregion
}
