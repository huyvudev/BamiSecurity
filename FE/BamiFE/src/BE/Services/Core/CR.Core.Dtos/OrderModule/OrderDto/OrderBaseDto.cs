using CR.Constants.Core.Order;

namespace CR.Core.Dtos.OrderModule.OrderDto;

public class OrderBaseDto
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public required string Address { get; set; }

    public string? Address2 { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public string? Tax { get; set; }

    public required string City { get; set; }
    public OrderStatus Status { get; set; }
    public required string? State { get; set; }

    public required string PostalCode { get; set; }

    public required string Country { get; set; }

    public required string OrderNumber { get; set; }

    public required string Namespace { get; set; }

    public int? BrandId { get; set; }
}
