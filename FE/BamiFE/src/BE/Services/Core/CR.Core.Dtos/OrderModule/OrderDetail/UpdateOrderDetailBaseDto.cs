using System.ComponentModel.DataAnnotations;

namespace CR.Core.Dtos.OrderModule.OrderDetail;

public class UpdateOrderDetailBaseDto : CreateOrderDetailBaseDto
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    [MaxLength(5000)]
    public string? ErrorMessage { get; set; }
}
