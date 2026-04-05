using CR.Core.Dtos.SkuModule.SkuSizePkgMockup;
using System.ComponentModel.DataAnnotations;

namespace CR.Core.Dtos.SkuModule.SkuSize;
public class UpdateSkuSizeDto
{
    public int Id { get; set; }
    /// <summary>
    /// code của size sku không khác gì tên
    /// </summary>
    [MaxLength(512)]
    public required string Name { get; set; }

    /// <summary>
    /// chiều rộng
    /// </summary>
    public double Width { get; set; }

    /// <summary>
    /// độ dày
    /// </summary>
    public double Height { get; set; }

    /// <summary>
    /// Chiều dài
    /// </summary>
    public double Length { get; set; }

    /// <summary>
    /// Khối lượng
    /// </summary>
    public double Weight { get; set; }

    /// <summary>
    /// Khối lượng bổ sung
    /// </summary>
    public double AdditionalWeight { get; set; }

    /// <summary>
    /// Có phả là mặc định hay không
    /// </summary>
    public bool IsDefault { get; set; }

    /// <summary>
    /// Giá gốc
    /// </summary>
    public double BaseCost { get; set; }

    /// <summary>
    /// Giá theo mét
    /// </summary>
    public double? CostInMeters { get; set; }

    /// <summary>
    /// Khối lượng theo thể tích
    /// </summary>
    public double? WeightByVolume { get; set; }

    /// <summary>
    /// Hướng dẫn đóng bằng chữ
    /// </summary>
    [MaxLength(512)]
    public string? PackageDescription { get; set; }

    public int? SkuId { get; set; }

    /// <summary>
    /// Danh sách mockup cần tạo
    /// </summary>
    public List<UpdateSkuSizePkgMockupDto> MockupsList { get; set; } = [];
}
