using CR.Core.Dtos.SkuModule.SkuSize;

namespace CR.Core.Dtos.SkuModule.Sku;
public class FindSkuDto : SkuDto
{
    public List<FindSkuSizeDto> Sizes { get; set; } = [];
}
