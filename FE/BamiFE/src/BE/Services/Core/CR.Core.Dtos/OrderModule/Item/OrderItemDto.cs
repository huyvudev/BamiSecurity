using CR.Constants.Core.Order;

namespace CR.Core.Dtos.OrderModule.Order;

public class OrderItemDto
{
    public int? Id { get; set; }
    public int? ItemIndex { get; set; }
    public OrderItemStatus? Status { get; set; }
    public string? Note { get; set; }
    public int? OrderDetailId { get; set; }
    public int? OrderId { get; set; }
    public OrderStatus? OrderStatus { get; set; }
    public OrderDetailStatus? OrderDetailStatus { get; set; }
    public string? OrderNumber { get; set; }
    public string? Namespace { get; set; }
    public int? IdSku { get; set; }
    public string? Code { get; set; }
    public int? IdBrand { get; set; }
    public string? BrandName { get; set; }
    public string? Size { get; set; }
    public double? Width { get; set; }
    public double? Length { get; set; }
    public string? ErrorMessage { get; set; }

    public string? MockUpFront { get; set; }

    public string? MockUpBack { get; set; }

    public string? DesignFront { get; set; }

    public string? DesignBack { get; set; }

    public string? DesignSleeves { get; set; }

    public string? DesignHood { get; set; }
    public DateTime? CreatedDate { get; set; }
    public string? State { get; set; }
    /// <summary>
    /// Tổng số lượng item có trong đơn hàng chứa item này
    /// </summary>
    public int? NumberItems { get; set; }
}
