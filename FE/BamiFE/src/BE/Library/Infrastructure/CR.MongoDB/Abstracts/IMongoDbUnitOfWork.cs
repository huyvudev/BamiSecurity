namespace CR.MongoDB.Abstracts;

public interface IMongoDbUnitOfWork : IDisposable
{
    void BeginTransaction();
    Task CommitAsync();
    Task AbortAsync();
}
