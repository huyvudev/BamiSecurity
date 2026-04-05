using CR.Constants.Common.Database;
using CR.Constants.Core.Order;
using CR.Core.Domain.Order;
using CR.Core.Domain.Partner;
using CR.Core.Domain.Sku;
using CR.EntitiesBase.Interfaces;

namespace CR.Core.Domain.Batch;

[Table(nameof(CoreBatch), Schema = DbSchemas.CRCore)]
public class CoreBatch : ICreatedBy
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// Tên người tạo ( nhập tay )
    /// </summary>
    public required string CreatorName { get; set; }

    /// <summary>
    /// Độ ưu tiên
    /// </summary>
    public BatchPriority Priority { get; set; }

    public int? SkuId { get; set; }
    /// <summary>
    /// SKU ( lấy từ các item trong lô, 1 lô chỉ có chứa item chung 1 sku )
    /// </summary>
    public CoreSku? Sku { get; set; }

    /// <summary>
    /// Trạng thái lô
    /// </summary>
    public BatchStatus Status { get; set; }

    /// <summary>
    /// Ngày in
    /// </summary>
    public DateTime? PrintDate { get; set; }

    /// <summary>
    /// Ngày cắt
    /// </summary>
    public DateTime? CutDate { get; set; }

    /// <summary>
    /// Ngày khắc
    /// </summary>
    public DateTime? EngravedDate { get; set; }

    /// <summary>
    /// Ngày hoàn thiện
    /// </summary>
    public DateTime? FinishDate { get; set; }

    /// <summary>
    /// Ghi chú
    /// </summary>
    public string? Note { get; set; }

    /// <summary>
    /// Đối tác
    /// </summary>
    public int PartnerId { get; set; }
    public CorePartner Partner { get; set; } = null!;

    public List<CoreOrderItem> OrderItems { get; set; } = [];
    #region audit
    public DateTime? CreatedDate { get; set; }
    public int? CreatedBy { get; set; }
    #endregion
}
