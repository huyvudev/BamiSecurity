using System.ComponentModel.DataAnnotations;

namespace CR.Core.Dtos.BrandModule.Store;

public class CreateStoreDto
{
    [MaxLength(512)]
    public required string Name { get; set; }
    public int BrandId { get; set; }
}
