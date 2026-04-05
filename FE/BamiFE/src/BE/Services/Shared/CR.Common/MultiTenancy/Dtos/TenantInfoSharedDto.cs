namespace CR.Common.MultiTenancy.Dtos;

public class TenantInfoSharedDto
{
    public int Id { get; set; }
    public string TenantName { get; set; } = null!;
    public string Language { get; set; } = null!;
    public string TimeZone { get; set; } = null!;
}
