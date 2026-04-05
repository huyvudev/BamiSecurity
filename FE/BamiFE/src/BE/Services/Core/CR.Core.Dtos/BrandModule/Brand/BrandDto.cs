using CR.Core.Dtos.BrandModule.Store;

namespace CR.Core.Dtos.BrandModule.Brand;

public class BrandDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public IEnumerable<StoreDto> Stores { get; set; }
}
