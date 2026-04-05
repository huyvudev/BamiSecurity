using CR.Constants.Core.Order;

namespace CR.Core.Dtos.BatchModule;

public class CreateBatchDto
{
    public required string CreatorName { get; set; }
    public BatchPriority Priority { get; set; }
    public int PartnerId { get; set; }
    public List<BatchItemDto> Items { get; set; } = [];
}
