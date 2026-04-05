using CR.RabbitMQ.Configs;
using CR.RunProcess.Worker.Csharp.Consumers;
using CR.RunProcess.WorkerBase;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CR.RunProcess.Worker.Csharp
{
    internal class Program : ProgramBase
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            CreateHostBuilder(ConfigureServices).Build().Run();
        }

        static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            //services.Configure<RabbitMqConfig>(context.Configuration.GetSection("RabbitMQ"));
            //services.AddHostedService<CodeRunnerConsumer>();
        }
    }
}
