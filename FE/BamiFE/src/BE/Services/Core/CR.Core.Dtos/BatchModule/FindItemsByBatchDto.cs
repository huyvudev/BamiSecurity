using Microsoft.AspNetCore.Mvc;

namespace CR.Core.Dtos.BatchModule;
public class FindItemsByBatchDto
{
    [FromQuery(Name = "id")]
    public List<int> BatchIds { get; set; } = [];
}
