using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace CR.MongoDB;

public static class RepositoryUtils
{
    public static string ToJson<TEntity>(this FilterDefinition<TEntity> filter)
        where TEntity : class
    {
        return filter
            .Render(
                BsonSerializer.SerializerRegistry.GetSerializer<TEntity>(),
                BsonSerializer.SerializerRegistry
            )
            .ToString();
    }
}
