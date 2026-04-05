using CR.Constants.Environments;
using CR.Utils.Security;
using CR.WebAPIBase.Filters;
using CR.WebAPIBase.Middlewares;
using Hangfire;
using Hangfire.Redis.StackExchange;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Serilog;
using StackExchange.Profiling.Storage;
using StackExchange.Redis;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

namespace CR.WebAPIBase
{
    /// <summary>
    /// Các hàm mở rộng cho program web api, cấu hình services, middleware pipeline
    /// </summary>
    public static class ProgramExtensions
    {
        public const string DbConnection = "Default";
        public const string Redis = "Redis";
        public const string Jwk = "Jwk";
        public const string CorsPolicy = "cors_policy";

        /// <summary>
        /// Config default services <br/>
        /// </summary>
        public static void ConfigureServices(this WebApplicationBuilder builder)
        {
            var services = builder.Services;
            services.AddOptions();
            services.AddHealthChecks();
            services.AddHttpContextAccessor();
        }

        public static void ConfigureDistributedCache(this WebApplicationBuilder builder)
        {
            try
            {
                builder.Services.AddStackExchangeRedisCache(options =>
                {
                    options.Configuration = builder.Configuration.GetConnectionString(Redis);
                });
            }
            catch
            {
                builder.Services.AddDistributedMemoryCache();
            }
        }

        public static void ConfigureSession(this WebApplicationBuilder builder)
        {
            ConfigurationManager configurationManager = builder.Configuration;
            builder.Services.AddSession(o =>
            {
                o.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                o.Cookie.Name = configurationManager["Session:AuthCookieName"];
                o.Cookie.HttpOnly = true;
                o.IdleTimeout = TimeSpan.FromMinutes(30);
            });
        }

        public static void ConfigureLogging(
            this WebApplicationBuilder builder,
            string queueName,
            string routingKey
        )
        {
            var environment = builder.Environment.EnvironmentName;
            var logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithProperty("Project", "Printing Factory")
                .Enrich.WithProperty("Environment", environment)
                .Enrich.WithProperty("Service", $"{Assembly.GetEntryAssembly()?.GetName().Name}")
                .ReadFrom.Configuration(builder.Configuration)
                .CreateLogger();
            builder.Host.UseSerilog(logger);
        }

        public static void ConfigureAuthentication(this WebApplicationBuilder builder)
        {
            builder.Services.AddAuthorization();
            builder
                .Services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(
                    JwtBearerDefaults.AuthenticationScheme,
                    options =>
                    {
                        var rsaSecurityKey = CryptographyUtils.ReadKey(
                            builder.Configuration.GetValue<string>("IdentityServer:PublicKey")!,
                            builder.Configuration.GetValue<string>("IdentityServer:PrivateKey")!
                        );

                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateAudience = false,
                            ValidateIssuer = false,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = rsaSecurityKey
                        };
                        options.RequireHttpsMetadata = false;

                        options.Events = new JwtBearerEvents
                        {
                            OnMessageReceived = context =>
                            {
                                //lấy token trong header
                                var accessToken = context
                                    .Request.Query.FirstOrDefault(q => q.Key == "access_token")
                                    .Value.ToString();
                                if (string.IsNullOrEmpty(accessToken))
                                {
                                    accessToken = context
                                        .Request.Headers.FirstOrDefault(h =>
                                            h.Key == "access_token"
                                        )
                                        .Value.ToString();
                                }

                                // If the request is for our hub...
                                var path = context.HttpContext.Request.Path;
                                if (
                                    !string.IsNullOrEmpty(accessToken)
                                    && path.StartsWithSegments("/hub")
                                )
                                {
                                    // Read the token out of the query string
                                    context.Token = accessToken;
                                }
                                return Task.CompletedTask;
                            }
                        };
                    }
                );
        }

        /// <summary>
        /// miniProfilerBasePath + /results-index
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="miniProfilerBasePath"></param>
        public static void ConfigureMiniProfile(
            this WebApplicationBuilder builder,
            string miniProfilerBasePath
        )
        {
            builder
                .Services.AddMiniProfiler(options =>
                {
                    options.RouteBasePath = miniProfilerBasePath; //profiler/results-index
                    (options.Storage as MemoryCacheStorage)!.CacheDuration = TimeSpan.FromMinutes(
                        20
                    );
                    options.SqlFormatter =
                        new StackExchange.Profiling.SqlFormatters.InlineFormatter();
                })
                .AddEntityFramework();
        }

        public static void ConfigureSwagger(this WebApplicationBuilder builder)
        {
            builder.Services.AddSwaggerGen(option =>
            {
                option.OperationFilter<AddCommonParameterSwagger>();

                option.SwaggerDoc(
                    "v1",
                    new OpenApiInfo
                    {
                        Title = Assembly.GetEntryAssembly()?.GetName().Name,
                        Version = "v1"
                    }
                );

                option.AddSecurityDefinition(
                    JwtBearerDefaults.AuthenticationScheme,
                    new OpenApiSecurityScheme
                    {
                        Description =
                            "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                        Name = "Authorization",
                        BearerFormat = "JWT",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey
                    }
                );

                option.AddSecurityRequirement(
                    new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = JwtBearerDefaults.AuthenticationScheme
                                }
                            },
                            Array.Empty<string>()
                        }
                    }
                );

                // Set the comments path for the Swagger JSON and UI.**
                var xmlFile = Path.Combine(
                    AppContext.BaseDirectory,
                    $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"
                );
                if (File.Exists(xmlFile))
                {
                    option.IncludeXmlComments(xmlFile);
                }
                var projectDependencies = Assembly
                    .GetEntryAssembly()!
                    .CustomAttributes.SelectMany(c =>
                        c.ConstructorArguments.Select(ca => ca.Value?.ToString())
                    )
                    .Where(o => o != null)
                    .ToList();
                foreach (var assembly in projectDependencies)
                {
                    var otherXml = Path.Combine(AppContext.BaseDirectory, $"{assembly}.xml");
                    if (File.Exists(otherXml))
                    {
                        option.IncludeXmlComments(otherXml, includeControllerXmlComments: true);
                    }
                }
                option.CustomSchemaIds(x => x.FullName);
                option.ExampleFilters();
            });
            builder.Services.AddSwaggerExamplesFromAssemblies(Assembly.GetEntryAssembly());
        }

        public static void UseSwaggerConfig(this WebApplication app, string prefixPath)
        {
            app.UseSwagger(option =>
            {
                option.RouteTemplate = $"{prefixPath}/{{documentName}}/swagger.json";
            });
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint(
                    $"/{prefixPath}/v1/swagger.json",
                    $"{Assembly.GetEntryAssembly()?.GetName().Name} v1"
                );
                options.RoutePrefix = prefixPath;
                options.DocExpansion(DocExpansion.None);
            });
        }

        public static void ConfigureEndpoint(this WebApplication app)
        {
            app.UseHangfireDashboard();
            app.MapHealthChecks("/health");
            app.MapHangfireDashboard("/hangfire");
            app.MapControllers();
        }

        public static void ConfigureCors(this WebApplicationBuilder builder)
        {
            string allowOrigins = builder.Configuration.GetSection("AllowedOrigins")!.Value!;
            //File.WriteAllText("cors.now.txt", $"CORS: {allowOrigins}");
            Console.WriteLine(allowOrigins);
            var origins = allowOrigins
                .Split(';')
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .ToArray();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(
                    CorsPolicy,
                    builder =>
                    {
                        builder
                            .SetIsOriginAllowedToAllowWildcardSubdomains()
                            .WithOrigins(origins)
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            //.AllowCredentials() //cho phép gưi cookies tới domain khác (same-origin policy)
                            .WithExposedHeaders("Content-Disposition");
                    }
                );
            });
        }

        private static ConfigurationOptions RedisConfig(this WebApplicationBuilder builder)
        {
            string configStrings = builder.Configuration["Redis:Config"]!;
            ConfigurationOptions configOptions = ConfigurationOptions.Parse(configStrings);
            configOptions.CheckCertificateRevocation = false;
            configOptions.CertificateValidation += (sender, cert, chain, errors) =>
            {
                return true;
            };
            string? certPath = builder.Configuration["Redis:Ssl:CertPath"];
            if (certPath != null)
            {
                configOptions.CertificateSelection += delegate
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), certPath);
                    var cert = new X509Certificate2(path);
                    return cert;
                };
            }
            return configOptions;
        }

        public static void ConfigureHangfire(
            this WebApplicationBuilder builder,
            string schemaName
        )
        {
            var connectionString = builder.GetConnectionString("Default");
            var redisConfig = !string.IsNullOrEmpty(builder.Configuration["Redis:Config"]);
            ConnectionMultiplexer? redisConnection = null;
            RedisStorageOptions? redisStorageOption = null;
            if (redisConfig)
            {
                redisConnection = ConnectionMultiplexer.Connect(builder.RedisConfig());
                redisStorageOption = new RedisStorageOptions
                {
                    Prefix = $"{{hangfire-{Assembly.GetEntryAssembly()?.GetName().Name}}}:",
                };

                JobStorage.Current = new RedisStorage(redisConnection, redisStorageOption);
            }
            else
            {
                JobStorage.Current = new SqlServerStorage(
                    connectionString,
                    new SqlServerStorageOptions { SchemaName = schemaName, }
                );
            }
            builder.Services.AddHangfire(configuration =>
            {
                configuration
                    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                    .UseSerilogLogProvider()
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings();

                if (redisConfig && redisConnection != null && redisStorageOption != null)
                {
                    configuration.UseRedisStorage(redisConnection, redisStorageOption);
                }
                else
                {
                    configuration.UseSqlServerStorage(
                        connectionString,
                        new SqlServerStorageOptions { SchemaName = schemaName, }
                    );
                }

                configuration.UseSerializerSettings(new Newtonsoft.Json.JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                });
            });

            builder.Services.AddHangfireServer(
                (service, options) =>
                {
                    options.ServerName = Assembly.GetEntryAssembly()?.GetName().Name;
                    options.WorkerCount = 200;
                    if (redisConfig)
                    {
                        options.SchedulePollingInterval = TimeSpan.FromMilliseconds(1000);
                    }
                }
            );
        }

        /// <summary>
        /// Lấy chuỗi kết nối
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="name">Tên chuỗi kết nối</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static string GetConnectionString(this WebApplicationBuilder builder, string name)
        {
            return builder.Configuration.GetConnectionString(name)
                ?? throw new InvalidOperationException(
                    $"Không tìm thấy connection string \"{name}\" trong appsettings.json"
                );
        }

        public static void UpdateMigrations<TDbContext>(this WebApplication app)
            where TDbContext : DbContext
        {
            using var scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<TDbContext>();
            if (db.Database.GetPendingMigrations().Any())
            {
                db.Database.Migrate();
            }
        }

        public static void ConfigureOpenIddict<TDbContext>(this WebApplicationBuilder builder)
            where TDbContext : DbContext
        {
            builder
                .Services.AddOpenIddict()
                // Register the OpenIddict core components.
                .AddCore(options =>
                {
                    // Configure OpenIddict to use the Entity Framework Core stores and models.
                    // Note: call ReplaceDefaultEntities() to replace the default entities.
                    options.UseEntityFrameworkCore().UseDbContext<TDbContext>();
                });
        }

        public static void ConfigureDataProtection(this WebApplicationBuilder builder)
        {
            var protectKeyCer = builder
                .Configuration.GetSection("IdentityServer:ProtectKeyCer")
                .Value;
            X509Certificate2 certificate = new(File.ReadAllBytes(protectKeyCer!));
            builder
                .Services.AddDataProtection()
                .SetApplicationName("CR")
                .ProtectKeysWithCertificate(certificate);
        }
    }
}
