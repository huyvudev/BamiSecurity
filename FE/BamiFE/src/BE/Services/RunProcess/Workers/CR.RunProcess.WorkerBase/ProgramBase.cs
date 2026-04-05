using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CR.RunProcess.WorkerBase
{
    public abstract class ProgramBase
    {
        protected static IHostBuilder CreateHostBuilder(Action<HostBuilderContext, IServiceCollection> configureDelegate) =>
            Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((context, config) =>
                {
                    // Xóa các cấu hình mặc định
                    config.Sources.Clear();

                    // Thêm tệp cấu hình mặc định
                    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

                    // Xác định môi trường hiện tại từ biến môi trường
                    var env = context.HostingEnvironment.EnvironmentName;

                    // Thêm tệp cấu hình dựa trên môi trường hiện tại (nếu có)
                    config.AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true);

                    // Thêm các cấu hình từ biến môi trường
                    config.AddEnvironmentVariables();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    configureDelegate(hostContext, services);
                });
    }
}
