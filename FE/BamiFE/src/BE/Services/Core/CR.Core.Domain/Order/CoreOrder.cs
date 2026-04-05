using CR.Constants.Common.Database;
using CR.Constants.Core.Order;
using CR.Core.Domain.Brand;
using Microsoft.EntityFrameworkCore;

namespace CR.Core.Domain.Order;

[Table(nameof(CoreOrder), Schema = DbSchemas.CRCore)]
public class CoreOrder
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// Tên của người nhận
    /// </summary>
    [MaxLength(512)]
    public required string Name { get; set; }

    /// <summary>
    /// Địa chỉ 1
    /// </summary>
    [MaxLength(512)]
    public required string Address { get; set; }

    /// <summary>
    /// Địa chỉ 2
    /// </summary>
    [MaxLength(512)]
    public string? Address2 { get; set; }


    /// <summary>
    /// Địa chỉ 2
    /// </summary>
    [MaxLength(512)]
    public string? Phone { get; set; }

    /// <summary>
    /// Email
    /// </summary>
    [MaxLength(512)]
    public string? Email { get; set; }

    /// <summary>
    /// Tax
    /// </summary>
    [MaxLength(512)]
    public string? Tax { get; set; }

    /// <summary>
    /// Thành phố
    /// </summary>
    [MaxLength(512)]
    public string? City { get; set; }

    /// <summary>
    /// Bang
    /// </summary>
    [MaxLength(512)]
    public string? State { get; set; }

    /// <summary>
    /// Mã bưu chính
    /// </summary>
    [MaxLength(512)]
    public string? PostalCode { get; set; }

    /// <summary>
    /// Quốc gia
    /// </summary>
    [MaxLength(512)]
    public string? Country { get; set; }

    /// <summary>
    /// Trạng thái
    /// </summary>
    public OrderStatus Status { get; set; }

    /// <summary>
    /// Mã đơn hàng
    /// </summary>
    [MaxLength(512)]
    public required string OrderNumber { get; set; }

    /// <summary>
    /// Id job chạy ngầm để xử lý ghi nhận thông tin
    /// </summary>
    [MaxLength(512)]
    [Unicode(false)]
    public string? BackgroundJobId { get; set; }

    #region Link với OrderItem
    /// <summary>
    /// Chi tiết đơn hàng ghi nhận thông tin từ seller
    /// </summary>
    public List<CoreOrderDetail> OrderDetails { get; } = [];

    /// <summary>
    /// Các item sản xuất
    /// </summary>
    public List<CoreOrderItem> OrderItems { get; } = [];
    #endregion

    #region Link với Tag
    public List<CoreTag> Tags { get; } = [];
    #endregion

    #region Link với Brand
    /// <summary>
    /// Id của Brand nếu có, sẽ được nhập ở bước tiền xử lý
    /// </summary>
    public int? BrandId { get; set; }
    public CoreBrand? Brand { get; set; }
    /// <summary>
    /// namesapce lấy từ tên của store chọc từ brand
    /// </summary>
    public required string Namespace { get; set; } = null!;
    #endregion
}
