using Microsoft.AspNetCore.Mvc;

namespace CR.Core.Dtos.OrderModule.Item;
public class FindOrderItemDto
{
    [FromQuery(Name = "orderId")]
    public int OrderId { get; set; }

    [FromQuery(Name = "itemIndex")]
    public int ItemIndex { get; set; }
}
