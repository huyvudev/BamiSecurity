using System.Reflection;
using CR.Authentication.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CR.HostConsole
{
    public static class Program
    {
        static void Main(string[] args)
        {
            IHost host = CreateHostBuilder(args).Build();
        }

        public static string StringFormat(string template, params object[] values)
        {
            return string.Format(template, values);
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration(app =>
                {
                    app.AddJsonFile("appsettings.json");
                })
                .ConfigureServices(
                    (hostContext, services) =>
                    {
                        string? connectionString = hostContext.Configuration.GetConnectionString(
                            "Default"
                        );
                        services.AddDbContext<AuthenticationDbContext>(options =>
                        {
                            options.UseSqlServer(
                                connectionString,
                                options =>
                                {
                                    options.MigrationsAssembly("CR.Migrations");
                                }
                            );
                            options.UseOpenIddict();
                        });
                    }
                );
    }
}
