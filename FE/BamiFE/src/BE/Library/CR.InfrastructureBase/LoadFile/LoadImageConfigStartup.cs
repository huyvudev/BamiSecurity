using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace CR.InfrastructureBase.LoadFile;

public static class LoadImageConfigStartup
{
    public static void ConfigureImageLoader(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IImageLoader, ImageLoader>();
    }
}
