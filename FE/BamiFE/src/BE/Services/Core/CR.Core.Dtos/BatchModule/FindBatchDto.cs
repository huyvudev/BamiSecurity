using CR.Constants.Core.Order;
using CR.Core.Dtos.OrderModule.Order;

namespace CR.Core.Dtos.BatchModule;
public class FindBatchDto
{
    public int Id { get; set; }
    public string? CreatorName { get; set; }
    public BatchPriority? Priority { get; set; }
    public string? Sku { get; set; }
    public int PartnerId { get; set; }
    public string? PartnerName { get; set; }
    public BatchStatus? Status { get; set; }
    public DateTime? PrintDate { get; set; }
    public DateTime? CutDate { get; set; }
    public DateTime? EngravedDate { get; set; }
    public DateTime? FinishDate { get; set; }
    public List<OrderItemDto> OrderItems { get; set; } = [];
    public DateTime? CreatedDate { get; set; }
    public int? CreatedBy { get; set; }
    public string? Note { get; set; }
}
