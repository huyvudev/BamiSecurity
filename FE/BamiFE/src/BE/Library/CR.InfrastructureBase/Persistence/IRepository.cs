using CR.DtoBase;

namespace CR.InfrastructureBase.Persistence
{
    public interface IRepository<TEntity, TFilter>
        where TEntity : class
        where TFilter : class
    {
        void SetXRequestId(string? requestId);
        Task<List<TEntity>> GetAsync();
        Task<TEntity?> GetAsync(string id);
        Task<List<TEntity>> GetAsync(TFilter filter);
        Task<TEntity?> GetFirstOrDefaultAsync(TFilter filter);
        Task DeleteAsync(TFilter filter);
        Task<TEntity> CreateAsync(TEntity entity);
        Task UpdateAsync(string id, TEntity entity);
        Task RemoveAsync(string id);
        Task<PagingResult<TEntity>> GetPaginatedAsync(int pageNumber, int pageSize);
        Task<bool> AnyAsync(TFilter filter);
    }
}
