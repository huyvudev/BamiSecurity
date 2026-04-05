using CR.Common.MultiTenancy.Dtos;

namespace CR.Common.MultiTenancy;

/// <summary>
/// Các thông tin về tenant chia sẽ giữa các domain
/// </summary>
public interface ITenantSharedService
{
    /// <summary>
    /// Lấy thông tin tenant theo tenantId
    /// </summary>
    /// <param name="tenantId"></param>
    /// <returns></returns>
    Task<TenantInfoSharedDto?> FindTenantInfoById(int tenantId);

    /// <summary>
    /// Lấy danh sách tất cả tenant
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<TenantInfoSharedDto>> GetAllTenant();

    /// <summary>
    /// Lấy domain của tenant
    /// </summary>
    /// <param name="tenantId"></param>
    /// <returns></returns>
    Task<string?> GetDomainWithSchema(int tenantId);
}
