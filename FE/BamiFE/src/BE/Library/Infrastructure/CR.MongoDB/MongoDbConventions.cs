using MongoDB.Bson.Serialization.Conventions;

namespace CR.MongoDB;

public static class MongoDbConventions
{
    public static void RegisterIgnoreIfNullConvention()
    {
        var conventionPack = new ConventionPack { new IgnoreIfNullConvention(true) };
        ConventionRegistry.Register("IgnoreNulls", conventionPack, t => true);
    }

    public static void RegisterCamelCaseConvention()
    {
        var conventionPack = new ConventionPack { new CamelCaseElementNameConvention() };
        ConventionRegistry.Register("CamelCase", conventionPack, t => true);
    }
}
