using CR.ApplicationBase.Localization;
using CR.Common.Filters;
using CR.Common.Localization;
using CR.Constants.Common.Database;
using CR.Constants.Environments;
using CR.Constants.RabbitMQ;
using CR.Core.API.HostedServices;
using CR.Core.API.Models;
using CR.Core.ApplicationServices.Configs;
using CR.Core.ApplicationServices.Middlewares;
using CR.Core.Infrastructure.Persistence;
using CR.IdentityServerBase.Middlewares;
using CR.IdentityServerBase.StartUp;
using CR.S3Bucket.Configs;
using CR.WebAPIBase;
using CR.WebAPIBase.Filters;
using CR.WebAPIBase.Middlewares;
using Hangfire;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

namespace CR.Core.API
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            if (builder.Environment.EnvironmentName == EnvironmentNames.Aspire)
            {
                builder.AddServiceDefaults();
            }
            builder.Services.AddControllers();
            builder.ConfigureServices();
            builder.ConfigureLogging(RabbitQueues.LogCore, RabbitRoutingKeys.LogCore);

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
            });
            builder.Services.Configure<FormOptions>(o =>
            {
                o.MultipartBodyLengthLimit = int.MaxValue;
            });

            builder
                .Services.AddControllersWithViews(options =>
                {
                    options.Filters.Add<ExceptionFilter>();
                    options.Filters.Add<CustomValidationErrorAttribute>();
                })
                .AddRazorRuntimeCompilation();

            builder.ConfigureSwagger();
            builder.ConfigureAuthentication();
            builder.ConfigureCors();
            //builder.ConfigureMiniProfile("/api/core/mini-profile");

            builder.Services.AddSingleton<LocalizationBase, CRLocalization>();

            builder.ConfigureCore(typeof(Program).Namespace);
            builder.Services.Configure<AuthWorkerConfiguration>(
                builder.Configuration.GetSection("IdentityServer:Clients")
            );
            builder.Services.AddCommonIdentityServer<CoreDbContext, CRAuthWorker>(
                builder.Configuration
            );
            builder.ConfigureOpenIddict<CoreDbContext>();
            builder.ConfigureHangfire(DbSchemas.CRCore);
            builder.ConfigureS3();

            var app = builder.Build();
            if (app.Environment.EnvironmentName == EnvironmentNames.Aspire)
            {
                app.MapDefaultEndpoints();
            }
            app.UseRequestId();
            // Configure the HTTP request pipeline.
            if (EnvironmentNames.DevelopEnv.Contains(app.Environment.EnvironmentName))
            {
                app.UseDeveloperExceptionPage();
                app.UseSwaggerConfig("api/core/swagger");
                //app.UseMiniProfiler();
            }

            if (app.Environment.EnvironmentName != EnvironmentNames.Development)
            {
                app.UpdateMigrations<CoreDbContext>();
            }
            //app.UseHttpsRedirection();
            app.UseCors(ProgramExtensions.CorsPolicy);
            app.UseForwardedHeaders();
            app.UseStaticFiles(
                new StaticFileOptions
                {
                    FileProvider = new PhysicalFileProvider(
                        Path.Combine(builder.Environment.ContentRootPath, "wwwroot")
                    ),
                    RequestPath = "/api"
                }
            );
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseHangfireDashboard(
                "/api/core/hangfire",
                new DashboardOptions
                {
                    Authorization =
                        app.Environment.EnvironmentName != EnvironmentNames.Development
                        && app.Environment.EnvironmentName != EnvironmentNames.Test
                            ? new[] { new HangfireAuthorizationFilter() }
                            : []
                }
            );
            app.UseRequestLocalizationCustom();
            app.UseCheckAuthorizationToken();
            app.UseCheckUser();
            app.MapControllers();
            app.MapHealthChecks("/api/core/health");
            app.Run();
        }
    }
}
