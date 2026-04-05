using CR.DtoBase.Validations;
using System.ComponentModel.DataAnnotations;

namespace CR.Core.Dtos.OrderModule.SaleOrder;

public class CreateSaleOrderDetailDto
{
    /// <summary>
    /// Loại sản phẩm (ví dụ: T-shirt, Hoodie, Mug)
    /// </summary>
    [CustomMaxLength(512)]
    public string? Type { get; set; }

    /// <summary>
    /// Tiêu đề (ví dụ: Women Black T-shirt)
    /// </summary>
    [CustomMaxLength(512)]
    public string? Title { get; set; }

    /// <summary>
    /// Kích cỡ của sản phẩm dạng chuỗi ví dụ 10in, 20in, 10x20in dùng cho lúc nhập dữ liệu
    /// </summary>
    [CustomMaxLength(512)]
    public string? Size { get; set; }

    /// <summary>
    /// Sku của sản phẩm (do seller tự đặt)
    /// </summary>
    [CustomMaxLength(512)]
    public string? SellerSku { get; set; }

    /// <summary>
    /// Màu sắc (ví dụ: White, Red,...)
    /// </summary>
    [CustomMaxLength(512)]
    public string? Color { get; set; }

    /// <summary>
    /// Số lượng, mặc định là 1, một bản ghi OrderDetail tương ứng sinh ra n bản ghi OrderItem với n = <see cref="Quantity"/>
    /// </summary>
    [Range(1, int.MaxValue)]
    public int Quantity { get; set; } = 1;

    #region Mockup
    /// <summary>
    /// Mockup mặt trước
    /// </summary>
    [CustomMaxLength(512)]
    public required string MockUpFront { get; set; }

    /// <summary>
    /// mockup mặt sau
    /// </summary>
    [CustomMaxLength(512)]
    public string? MockUpBack { get; set; }
    #endregion

    #region Design
    /// <summary>
    /// Thiết kế mặt trước
    /// </summary>
    [CustomMaxLength(512)]
    public string? SaleDesignFront { get; set; }

    /// <summary>
    /// Thiết kế mặt sau
    /// </summary>
    [CustomMaxLength(512)]
    public string? SaleDesignBack { get; set; }

    /// <summary>
    /// Thiết kế tay áo
    /// </summary>
    [CustomMaxLength(512)]
    public string? SaleDesignSleeves { get; set; }

    /// <summary>
    /// Thiết kế mũ
    /// </summary>
    [CustomMaxLength(512)]
    public string? SaleDesignHood { get; set; }
    #endregion
}
