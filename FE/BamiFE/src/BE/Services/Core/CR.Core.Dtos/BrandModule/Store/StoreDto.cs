using CR.Core.Domain.Brand;
using CR.Core.Dtos.BrandModule.Brand;
using CR.Core.Dtos.OrderModule.Order;

namespace CR.Core.Dtos.BrandModule.Store;

public class StoreDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int BrandId { get; set; }

   
}
