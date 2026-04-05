using CR.ApplicationBase.Localization;
using CR.Authentication.ApplicationServices.Common.Localization;
using CR.Constants.Common.Database;
using CR.Core.ApplicationServices.AuthenticationModule.Abstracts;
using CR.Core.ApplicationServices.AuthenticationModule.Implements;
using CR.Core.ApplicationServices.BrandModule.Abstracts;
using CR.Core.ApplicationServices.BrandModule.Implements;
using CR.Core.ApplicationServices.Common;
using CR.Core.ApplicationServices.ItemModule.Abstracts;
using CR.Core.ApplicationServices.ItemModule.Implements;
using CR.Core.ApplicationServices.OrderModule.Abstracts;
using CR.Core.ApplicationServices.OrderModule.Implements;
using CR.Core.ApplicationServices.PartnerModule.Abstracts;
using CR.Core.ApplicationServices.PartnerModule.Implements;
using CR.Core.ApplicationServices.PartnerModule.Implements;
using CR.Core.ApplicationServices.SkuModule.Material.Abstracts;
using CR.Core.ApplicationServices.SkuModule.Material.Implements;
using CR.Core.ApplicationServices.SkuModule.ProductionMethod.Abstacts;
using CR.Core.ApplicationServices.SkuModule.ProductionMethod.Implements;
using CR.Core.ApplicationServices.SkuModule.Sku.Abstracts;
using CR.Core.ApplicationServices.SkuModule.Sku.Implements;
using CR.Core.ApplicationServices.SkuModule.SkuBase.Abstracts;
using CR.Core.ApplicationServices.SkuModule.SkuSize.Abstracts;
using CR.Core.ApplicationServices.SkuModule.SkuSizePkgMockup.Abstracts;
using CR.Core.Infrastructure.Exceptions;
using CR.Core.Infrastructure.Persistence;
using CR.InfrastructureBase.LoadFile;
using CR.WebAPIBase;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CR.Core.ApplicationServices.Configs
{
    public static class CoreConfigStartUp
    {
        public static void ConfigureCore(this WebApplicationBuilder builder, string? assemblyName)
        {
            builder.Services.AddDbContext<CoreDbContext>(
                options =>
                {
                    //options.UseInMemoryDatabase("DbDefault");
                    options.UseSqlServer(
                        builder.GetConnectionString("Default"),
                        options =>
                        {
                            options.MigrationsAssembly(assemblyName);
                            options.MigrationsHistoryTable(
                                DbSchemas.TableMigrationsHistory,
                                DbSchemas.CRCore
                            );
                        }
                    );
                    options.UseOpenIddict();
                },
                ServiceLifetime.Scoped
            );

            builder.ConfigureImageLoader();

            builder.Services.AddSingleton<ICoreLocalization, CoreLocalization>();
            builder.Services.AddSingleton<ICoreMapErrorCode, CoreMapErrorCode>();
            builder.Services.AddSingleton<IMapErrorCode, CoreMapErrorCode>();

            //builder.Services.AddScoped<IOtpService, OtpService>();
            builder.Services.AddScoped<IPermissionService, PermissionService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IRoleService, RoleService>();
            builder.Services.AddScoped<INotificationTokenService, NotificationTokenService>();
            builder.Services.AddScoped<IUserAuthenticationService, UserAuthorizationService>();
            builder.Services.AddScoped<IManagerTokenService, ManagerTokenService>();
            builder.Services.AddScoped<IBrandService, BrandService>();
            builder.Services.AddScoped<IStoreService, StoreService>();
            builder.Services.AddScoped<IPartnerService, PartnerService>();
            builder.Services.AddScoped<IPartnerTypeService, PartnerTypeService>();
            builder.Services.AddScoped<ISkuBaseService, SkuBaseService>();
            builder.Services.AddScoped<ISkuService, SkuService>();
            builder.Services.AddScoped<IMaterialService, MaterialService>();
            builder.Services.AddScoped<IProductMethodService, ProductMethodService>();
            builder.Services.AddScoped<ISkuSizeService, SkuSizeService>();
            builder.Services.AddScoped<ISkuSizePkgMockupService, SkuSizePkgMockupService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<ISaleOrderService, SaleOrderService>();
            builder.Services.AddScoped<IOrderStatusService, OrderStatusService>();
            builder.Services.AddScoped<IHandleOrderService, HandleOrderService>();
            builder.Services.AddScoped<IBatchService, BatchService>();
            builder.Services.AddScoped<IOrderDetailService, OrderDetailService>();
            builder.Services.AddScoped<IOrderItemService, OrderItemService>();
        }
    }
}
