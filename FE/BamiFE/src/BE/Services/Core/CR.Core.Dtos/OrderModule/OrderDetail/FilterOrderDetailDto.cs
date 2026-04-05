using CR.Constants.Core.Order;
using CR.DtoBase;

namespace CR.Core.Dtos.OrderModule.OrderDetail;
public class FilterOrderDetailDto : PagingRequestBaseDto
{
    public int? OrderId { get; set; }
    public string? Title {  get; set; }
    
    public string? Size { get; set; }
    public string? SellerSku { get; set; }
    public string? Color { get; set; }
    public int? Quantity { get; set; }
    public OrderDetailStatus? Status { get; set; }
    public string? Code { get; set; }
    public string? ErrorMessage { get; set; }
    public double? Width { get; set; }
    public double? Length { get; set; }
    public int? SkuId { get; set; }
    public string? DesignFront { get; set; }
    public string? DesignBack { get; set; }
    public string? DesignSleeves { get; set; }
    public string? DesignHood { get; set; }


}
