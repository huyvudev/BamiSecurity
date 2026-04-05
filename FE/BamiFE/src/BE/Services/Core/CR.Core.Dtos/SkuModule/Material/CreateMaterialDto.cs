using System.ComponentModel.DataAnnotations;

namespace CR.Core.Dtos.SkuModule.Material;
public class CreateMaterialDto
{
    /// <summary>
    /// Tên. Ví dụ: Gỗ 5 ly
    /// </summary>
    [MaxLength(512)]
    public required string Name { get; set; }

    /// <summary>
    /// Code. Ví dụ 0205 để tý nữa map với ProductMethod , SkuBase để cho ra mã SKU hoàn chỉnh 1L_WOODSTAND_0205TM
    /// </summary>
    [MaxLength(512)]
    public required string Code { get; set; }

    /// <summary>
    /// Mô tả. Ví dụ Gỗ 5 ly chất lượng cao ,....
    /// </summary>
    [MaxLength(512)]
    public string? Description { get; set; }
}
