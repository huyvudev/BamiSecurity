using CR.DtoBase;
using Microsoft.AspNetCore.Mvc;

namespace CR.Core.Dtos.PartnerModule.PartnerType;

public class FilterPartnerDto : PagingRequestBaseDto
{
    [FromQuery(Name = "partnerTypeId")]
    public int? PartnerTypeId { get; set; }
}
