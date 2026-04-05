using CR.Constants.Core.Order;

namespace CR.Core.Dtos.BatchModule;
public class UpdateBatchItemStatusDto
{
    public List<BatchItemDto> Items { get; set; } = [];
    public OrderItemStatus ItemStatus { get; set; }
}
