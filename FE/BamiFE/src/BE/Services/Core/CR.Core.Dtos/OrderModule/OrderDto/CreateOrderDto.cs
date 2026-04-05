using CR.Core.Dtos.OrderModule.OrderDetail;
using CR.DtoBase.Validations;

namespace CR.Core.Dtos.OrderModule.OrderDto;

public class CreateOrderDto
{
    /// <summary>
    /// Tên của người nhận
    /// </summary>
    [CustomMaxLength(512)]
    public required string Name { get; set; } = null!;

    /// <summary>
    /// Địa chỉ
    /// </summary>
    [CustomMaxLength(512)]
    public required string Address { get; set; } = null!;

    /// <summary>
    /// Địa chỉ 2
    /// </summary>
    [CustomMaxLength(512)]
    public string? Address2 { get; set; }

    /// <summary>
    /// Phone
    /// </summary>
    [CustomMaxLength(512)]
    public string? Phone { get; set; }

    /// <summary>
    /// Email
    /// </summary>
    [CustomMaxLength(512)]
    public string? Email { get; set; }

    /// <summary>
    /// Tax
    /// </summary>
    [CustomMaxLength(512)]
    public string? Tax { get; set; }

    /// <summary>
    /// Thành phố
    /// </summary>
    [CustomMaxLength(512)]
    public string? City { get; set; } = null!;

    /// <summary>
    /// Bang
    /// </summary>
    [CustomMaxLength(512)]
    public string? State { get; set; } = null!;

    /// <summary>
    /// Mã bưu chính
    /// </summary>
    [CustomMaxLength(512)]
    public string? PostalCode { get; set; } = null!;

    /// <summary>
    /// Quốc gia
    /// </summary>
    [CustomMaxLength(512)]
    public string? Country { get; set; } = null!;

    /// <summary>
    /// Mã đơn hàng
    /// </summary>
    [CustomMaxLength(512)]
    public required string OrderNumber { get; set; } = null!;

    public IEnumerable<CreateOrderDetailBaseDto> Details { get; set; } = [];

    public int? BrandId { get; set; }

    /// <summary>
    /// namesapce lấy từ tên của store chọc từ brand
    /// </summary>
    [CustomMaxLength(512)]
    public required string Namespace { get; set; } = null!;
}
