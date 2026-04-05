using CR.DtoBase.Validations;

namespace CR.Core.Dtos.OrderModule.SaleOrder;

public class CreateSaleOrderDto
{
    /// <summary>
    /// Tên của người nhận
    /// </summary>
    [CustomMaxLength(512)]
    [CustomRequired(AllowEmptyStrings = false)]
    public string Name { get; set; } = null!;

    /// <summary>
    /// Địa chỉ
    /// </summary>
    [CustomMaxLength(512)]
    [CustomRequired(AllowEmptyStrings = false)]
    public string Address { get; set; } = null!;

    /// <summary>
    /// Thành phố
    /// </summary>
    [CustomMaxLength(512)]
    [CustomRequired(AllowEmptyStrings = false)]
    public string City { get; set; } = null!;

    /// <summary>
    /// Bang
    /// </summary>
    [CustomMaxLength(512)]
    [CustomRequired(AllowEmptyStrings = false)]
    public string State { get; set; } = null!;

    /// <summary>
    /// Mã bưu chính
    /// </summary>
    [CustomMaxLength(512)]
    [CustomRequired(AllowEmptyStrings = false)]
    public string PostalCode { get; set; } = null!;

    /// <summary>
    /// Quốc gia
    /// </summary>
    [CustomMaxLength(512)]
    [CustomRequired(AllowEmptyStrings = false)]
    public string Country { get; set; } = null!;

    /// <summary>
    /// Mã đơn hàng
    /// </summary>
    [CustomMaxLength(512)]
    [CustomRequired(AllowEmptyStrings = false)]
    public string OrderNumber { get; set; } = null!;

    public string Namespace { get; set; } = null!;

    public IEnumerable<CreateSaleOrderDetailDto> Details { get; set; } = [];
}
