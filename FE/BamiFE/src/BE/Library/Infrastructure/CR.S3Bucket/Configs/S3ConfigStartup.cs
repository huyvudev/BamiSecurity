using CR.S3Bucket.Localization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace CR.S3Bucket.Configs
{
    public static class S3ConfigStartup
    {
        public static void ConfigureS3(this WebApplicationBuilder builder)
        {
            builder.Services.Configure<S3Config>(builder.Configuration.GetSection("S3Config"));
            builder.Services.AddSingleton<IS3Localization, S3Localization>();
            builder.Services.AddSingleton<IS3MapErrorCode, S3ManagerFileMapErrorCode>();
            builder.Services.AddSingleton<IS3ManagerFile, S3ManagerFile>();
            builder.Services.AddSingleton<IS3BucketName, S3BucketName>();
        }
    }
}
