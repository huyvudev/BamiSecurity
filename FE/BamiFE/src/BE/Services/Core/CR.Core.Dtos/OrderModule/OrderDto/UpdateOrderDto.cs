using CR.DtoBase.Validations;

namespace CR.Core.Dtos.OrderModule.OrderDto;

public class UpdateOrderDto :OrderBaseDto
{
    public int? BrandId { get; set; }

}
