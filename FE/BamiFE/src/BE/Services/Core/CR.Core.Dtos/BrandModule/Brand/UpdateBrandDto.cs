using System.ComponentModel.DataAnnotations;

namespace CR.Core.Dtos.BrandModule.Brand;

public class UpdateBrandDto 
{
    public int Id { get; set; }
    [MaxLength(512)]
    public required string Name { get; set; }
}
