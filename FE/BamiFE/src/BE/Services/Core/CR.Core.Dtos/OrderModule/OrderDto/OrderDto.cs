using CR.Core.Dtos.OrderModule.OrderDetail;

namespace CR.Core.Dtos.OrderModule.OrderDto;

public class OrderDto : OrderBaseDto
{
    public int? BrandId { get; set; }

    public IEnumerable<OrderDetailBaseDto> Details { get; set; } = [];
}
