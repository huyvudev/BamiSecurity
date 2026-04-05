using CR.Constants.Core.Order;
using CR.DtoBase.Validations;

namespace CR.Core.Dtos.OrderModule.OrderDetail;

public class OrderDetailBaseDto
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public string? Type { get; set; }

    public string? Title { get; set; }

    public string? Size { get; set; }

    public string? SellerSku { get; set; }

    public string? Color { get; set; }

    public int Quantity { get; set; } = 1;

    public OrderDetailStatus Status { get; set; }

    public string? Code { get; set; }
    public string? ErrorMessage { get; set; }

    #region Mockup

    public string? MockUpFront { get; set; }

    public string? MockUpBack { get; set; }
    #endregion

    #region Design Sale
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
