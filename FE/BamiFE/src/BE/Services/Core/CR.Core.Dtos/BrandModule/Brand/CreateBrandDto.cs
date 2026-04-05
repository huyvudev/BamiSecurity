using System.ComponentModel.DataAnnotations;
using CR.Core.Dtos.BrandModule.Store;

namespace CR.Core.Dtos.BrandModule.Brand;

public class CreateBrandDto
{
    [MaxLength(512)]
    public required string Name { get; set; }
    public IEnumerable<CreateStoreInBrandDto> Stores { get; set; } = [];
}
