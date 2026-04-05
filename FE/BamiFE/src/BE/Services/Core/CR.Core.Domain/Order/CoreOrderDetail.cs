using CR.Constants.Common.Database;
using CR.Constants.Core.Order;
using CR.Core.Domain.Brand;
using CR.Core.Domain.Sku;

namespace CR.Core.Domain.Order;

/// <summary>
/// Chi tiết đơn hàng (ghi nhận khi seller đăng tải lên),
/// Với các hình ảnh là link của seller đăng, sau đó clone sang OrderItem
/// </summary>
[Table(nameof(CoreOrderDetail), Schema = DbSchemas.CRCore)]
public class CoreOrderDetail
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// Loại sản phẩm (ví dụ: T-shirt, Hoodie, Mug)
    /// </summary>
    [MaxLength(512)]
    public string? Type { get; set; }

    /// <summary>
    /// Tiêu đề (ví dụ: Women Black T-shirt)
    /// </summary>
    [MaxLength(512)]
    public string? Title { get; set; }

    /// <summary>
    /// Kích cỡ của sản phẩm dạng chuỗi ví dụ 10in, 20in, 10x20in dùng cho lúc nhập dữ liệu
    /// </summary>
    [MaxLength(512)]
    public string? Size { get; set; }

    /// <summary>
    /// Sku của sản phẩm (do seller tự đặt)
    /// </summary>
    [MaxLength(512)]
    public string? SellerSku { get; set; }

    /// <summary>
    /// Màu sắc (ví dụ: White, Red,...)
    /// </summary>
    [MaxLength(512)]
    public string? Color { get; set; }

    /// <summary>
    /// Số lượng, mặc định là 1, một bản ghi OrderDetail tương ứng sinh ra n bản ghi OrderItem với n = <see cref="Quantity"/>
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Trạng thái xử lý chi tiết đơn hàng
    /// </summary>
    public OrderDetailStatus Status { get; set; }

    /// <summary>
    /// Lỗi xử lý khi xử lý file hoặc thông tin còn thiếu
    /// </summary>
    [MaxLength(5000)]
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Chiều rộng
    /// </summary>
    public double? Width { get; set; }

    /// <summary>
    /// Chiều dài
    /// </summary>
    public double? Length { get; set; }

    #region Link với SKU
    /// <summary>
    /// Mã Sku
    /// </summary>
    public int? SkuId { get; set; }

    public CoreSku? Sku { get; set; }
    #endregion

    #region Link với Order
    /// <summary>
    /// Trong Order nào
    /// </summary>
    public int OrderId { get; set; }

    public CoreOrder Order { get; set; } = null!;
    #endregion

    #region Mockup
    /// <summary>
    /// Mockup mặt trước
    /// </summary>
    [MaxLength(512)]
    public string? MockUpFront { get; set; }

    /// <summary>
    /// mockup mặt sau
    /// </summary>
    [MaxLength(512)]
    public string? MockUpBack { get; set; }
    #endregion

    #region Design của sale
    /// <summary>
    /// Thiết kế mặt trước
    /// </summary>
    [MaxLength(512)]
    public string? SaleDesignFront { get; set; }

    /// <summary>
    /// Thiết kế mặt sau
    /// </summary>
    [MaxLength(512)]
    public string? SaleDesignBack { get; set; }

    /// <summary>
    /// Thiết kế tay áo
    /// </summary>
    [MaxLength(512)]
    public string? SaleDesignSleeves { get; set; }

    /// <summary>
    /// Thiết kế mũ
    /// </summary>
    [MaxLength(512)]
    public string? SaleDesignHood { get; set; }
    #endregion

    #region Design của admin
    /// <summary>
    /// Thiết kế mặt trước
    /// </summary>
    [MaxLength(512)]
    public string? DesignFront { get; set; }

    /// <summary>
    /// Thiết kế mặt sau
    /// </summary>
    [MaxLength(512)]
    public string? DesignBack { get; set; }

    /// <summary>
    /// Thiết kế tay áo
    /// </summary>
    [MaxLength(512)]
    public string? DesignSleeves { get; set; }

    /// <summary>
    /// Thiết kế mũ
    /// </summary>
    [MaxLength(512)]
    public string? DesignHood { get; set; }
    #endregion
}
