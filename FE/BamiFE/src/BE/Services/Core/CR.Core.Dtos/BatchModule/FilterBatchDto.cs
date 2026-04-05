using CR.Constants.Core.Order;
using CR.DtoBase;

namespace CR.Core.Dtos.BatchModule;

public class FilterBatchDto : PagingRequestBaseDto
{
    /// <summary>
    /// Trạng thái lô
    /// </summary>
    public BatchStatus? Status { get; set; }

    /// <summary>
    /// Loại sản phẩm
    /// </summary>
    public int? SkuId { get; set; }

    /// <summary>
    /// Độ ưu tiên
    /// </summary>
    public BatchPriority? Priority { get; set; }

    /// <summary>
    /// Người tạo lô
    /// </summary>
    public string? CreatorName { get; set; }

    /// <summary>
    /// Ngày tạo lô (Tìm kiếm trong khoảng thời gian)
    /// </summary>
    public DateTime? CreateDateStart { get; set; }
    public DateTime? CreateDateEnd { get; set; }

    /// <summary>
    /// Ngày in lô (Tìm kiếm trong khoảng thời gian)
    /// </summary>
    public DateTime? PrintDateStart { get; set; }
    public DateTime? PrintDateEnd { get; set; }

    /// <summary>
    /// Ngày cắt (Tìm kiếm trong khoảng thời gian)
    /// </summary>
    public DateTime? CutDateStart { get; set; }
    public DateTime? CutDateEnd { get; set; }

    /// <summary>
    /// Ngày khắc (Tìm kiếm trong khoảng thời gian)
    /// </summary>
    public DateTime? EngravedDateStart { get; set; }
    public DateTime? EngravedDateEnd { get; set; }

    /// <summary>
    /// Ngày hoàn thiện (Tìm kiếm trong khoảng thời gian)
    /// </summary>
    public DateTime? FinishDateStart { get; set; }
    public DateTime? FinishDateEnd { get; set; }

    /// <summary>
    /// Đối tác in ép
    /// </summary>
    public int? PartnerId { get; set; }

    /// <summary>
    /// Brand
    /// </summary>
    public int? BrandId { get; set; }

    /// <summary>
    /// Nguyên vật liệu
    /// </summary>
    public int? MaterialId { get; set; }

    /// <summary>
    /// Ghi chú
    /// </summary>
    public string? Note { get; set; }

    /// <summary>
    /// Tag
    /// </summary>
    public int? TagId { get; set; }

    // Các trường thêm nếu cần thiết
    // Trạng thái gộp lô
    // Đã giao
    // Thời gian quá hạn in
    // Thời gian quá hạn cắt
    // Thời gian quá hạn khắc
    // Thời gian quá hạn hoàn thiện
    // Thời gian item quá hạn
    // Địa điểm sản xuất
    // Tên lô
}
