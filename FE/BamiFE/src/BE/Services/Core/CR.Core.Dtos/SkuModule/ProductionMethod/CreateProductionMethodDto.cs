using System.ComponentModel.DataAnnotations;

namespace CR.Core.Dtos.SkuModule.ProductionMethod;
public class CreateProductionMethodDto
{
    /// <summary>
    /// Tên ví dụ Trắng Màu
    /// </summary>
    [MaxLength(512)]
    public required string Name { get; set; }

    /// <summary>
    /// code ví dụ TM  để tý nữa map với Material , SkuBase để cho ra mã SKU hoàn chỉnh 1L_WOODSTAND_0205TM
    /// </summary>
    [MaxLength(512)]
    public required string Code { get; set; }
}
