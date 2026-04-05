using CR.Core.Dtos.SkuModule.SkuSizePkgMockup;

namespace CR.Core.Dtos.SkuModule.SkuSize;
public class FindSkuSizeDto : SkuSizeDto
{
    public IEnumerable<SkuSizePkgMockupDto> mockUpsList { get; set; } = [];
}
