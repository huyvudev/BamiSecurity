var builder = DistributedApplication.CreateBuilder(args);

var redis = builder.AddRedis("redis", 6379);

var rabbitUsername = builder.AddParameter("RabbitMQUsername", secret: true);
var rabbitPassword = builder.AddParameter("RabbitMQPassword", secret: true);
var rabbitmq = builder.AddRabbitMQ("messaging", rabbitUsername, rabbitPassword, port: 5672);

var mongo = builder.AddMongoDB("mongo", 27017);
var mongodb = mongo.AddDatabase("mongodb", "Classroom-test");

var seq = builder.AddSeq("seq", 5341).ExcludeFromManifest();

var project = builder
    .AddProject<Projects.CR_Core_API>("cr-core-api")
    .WithReference(redis)
    .WithReference(rabbitmq)
    .WithReference(mongodb)
    .WithReference(seq);

project.WithEnvironment("ASPNETCORE_ENVIRONMENT", "Aspire");

await builder.Build().RunAsync();
