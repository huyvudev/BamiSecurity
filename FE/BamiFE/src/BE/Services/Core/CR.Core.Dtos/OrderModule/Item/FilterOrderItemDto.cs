using System.ComponentModel.DataAnnotations;
using CR.Constants.Core.Order;
using CR.DtoBase;

namespace CR.Core.Dtos.OrderModule.Item;

public class FilterOrderItemDto : PagingRequestBaseDto
{
    public int? ItemIndex { get; set; }
    public OrderItemStatus? Status { get; set; }

    public int? OrderDetailId { get; set; }
    public string? NameSpace { get; set; }
    public int? OrderId { get; set; }
    public int? IdSku { get; set; }
    public int? IdBrand { get; set; }
    [DataType(DataType.Date)]
    public DateTime? StartDate { get; set; }
    [DataType(DataType.Date)]
    public DateTime? EndDate { get; set; }
    public string? OrderNumber { get; set; }
}
