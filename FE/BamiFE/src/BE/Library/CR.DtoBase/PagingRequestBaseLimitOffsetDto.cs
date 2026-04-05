using CR.DtoBase.Validations;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CR.DtoBase;

/// <summary>
/// Request phân trang theo limit và offset
/// </summary>
public class PagingRequestBaseLimitOffsetDto
{
    /// <summary>
    /// Lấy bao nhiêu bản ghi
    /// </summary>
    [FromQuery(Name = "limit")]
    [Range(0,500)]
    public int Limit { get; set; } = 10;

    /// <summary>
    /// Bỏ qua bao nhiêu bản ghi
    /// </summary>
    [FromQuery(Name = "offSet")]
    public int OffSet { get; set; } = 0;
}
