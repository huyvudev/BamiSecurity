using CR.RabbitMQ.Configs;
using CR.RunProcess.WorkerBase;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CR.RunProcess.Worker.Java
{
    internal class Program : ProgramBase
    {
        static void Main(string[] args)
        {
            CreateHostBuilder(ConfigureServices).Build().Run();
        }

        static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            services.Configure<RabbitMqConfig>(context.Configuration.GetSection("RabbitMQ"));
        }
    }
}
