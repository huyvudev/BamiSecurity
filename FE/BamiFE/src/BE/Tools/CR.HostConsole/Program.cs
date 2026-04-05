using System.Reflection;
using System.Text;
using System.Xml.Linq;
using CR.Constants.RolePermission;
using CR.Constants.RolePermission.Constant;
using CR.Core.Infrastructure.Persistence;
using CR.Utils.DataUtils;
using CR.Utils.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace CR.HostConsole
{
    public class Program
    {
        static void Main(string[] args)
        {
            // Múi giờ IANA
            string ianaTimeZone = "UTC";

            // Lấy thông tin múi giờ
            TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById(ianaTimeZone);
            //string key = CryptographyUtils.GenerateAesKeyHex();
            //string iv = CryptographyUtils.GenerateAesIVHex();
            //RsaGenerate.Generate();
            //Console.WriteLine(RandomNumberUtils.GenerateRandomHexString(29));
            IHost host = CreateHostBuilder(args).Build();
            //CertificateProtectedKey.GenerateCertificate(
            //    $"{Directory.GetCurrentDirectory()}/../../../../../Services/Authentication/CR.Authentication.API/private_key.Development.pem"
            //);
            //CertificateProtectedKey.ConvertX509ToPkcs12($"{Directory.GetCurrentDirectory()}/../../../../../Services/Authentication/CR.Authentication.API/k8s-ca.Development.crt", "certificate.pfx");
            //RunGenXml();

            AntiXss.Test();
        }

        public static string? ReplaceMultiple(
            string? input,
            List<string?> replacements,
            List<string?> values
        )
        {
            if (string.IsNullOrEmpty(input) || replacements.Count != values.Count)
            {
                return input;
            }
            StringBuilder resultBuilder = new(input);
            // Perform replacements
            for (int i = 0; i < replacements.Count; i++)
            {
                if (!replacements[i].IsNullOrEmpty() && !values[i].IsNullOrEmpty())
                {
                    if (input.Contains(replacements[i]!))
                    {
                        resultBuilder.Replace(replacements[i]!, values[i]);
                    }
                }
            }
            // Convert StringBuilder to string and return
            return resultBuilder.ToString();
        }

        public static void RunGenXml()
        {
            GenXml(
                $"{Directory.GetCurrentDirectory()}/../../../../../Services/Authentication/CR.Authentication.ApplicationServices/Common/Localization/SourceFiles/en.xml"
            );
            GenXml(
                $"{Directory.GetCurrentDirectory()}/../../../../../Services/Authentication/CR.Authentication.ApplicationServices/Common/Localization/SourceFiles/vi.xml"
            );
        }

        public static void GenXml(string xmlPath)
        {
            XElement element = XElement.Load(xmlPath);
            var dicValues = element
                .Elements("texts")
                .Elements("text")
                .ToDictionary(
                    e => e.Attribute("name")!.Value,
                    e => e.Attribute("value")?.Value ?? e.Value
                );

            var xml = new StringBuilder();

            xml.Append("\n\n");
            foreach (
                var config in from config in PermissionConfig.CoreConfigs
                where !dicValues.ContainsKey(config.Key)
                select config
            )
            {
                xml.Append($"<text name=\"{config.Value.LName}\">~</text>\n");
            }
            xml.Append("\n\n");

            var cs = new StringBuilder();
            var permissionKeys = typeof(PermissionKeys)
                .GetFields(
                    BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy
                )
                .Where(fi => fi.IsLiteral && !fi.IsInitOnly)
                .Select(o => o.Name)
                .ToList();

            foreach (var key in permissionKeys.Where(n => n.StartsWith("User")))
            {
                cs.Append(
                    $"{{PermissionKeys.{key}, new(nameof(PermissionKeys.{key}), PermissionIcons.IconDefault)}},\n"
                );
            }
            cs.Append("\n\n");
            foreach (var key in permissionKeys.Where(n => n.StartsWith("Core")))
            {
                cs.Append(
                    $"{{PermissionKeys.{key}, new(nameof(PermissionKeys.{key}), PermissionIcons.IconDefault)}},\n"
                );
            }
            cs.Append("\n\n");
            string directory = Directory.GetCurrentDirectory() + "/xml/";
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            File.WriteAllText(directory + Path.GetFileName(xmlPath), xml.ToString());
            File.WriteAllText(
                directory + Path.GetFileNameWithoutExtension(xmlPath) + ".cs",
                cs.ToString()
            );
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
                        string assemblyName = Assembly.GetExecutingAssembly().GetName().Name;

                        //nếu có cấu hình redis
                        string redisConnectionString =
                            hostContext.Configuration.GetConnectionString("Redis");

                        string connectionString = hostContext.Configuration.GetConnectionString(
                            "Default"
                        );

                        //entity framework
                        services.AddDbContext<CoreDbContext>(options =>
                        {
                            options.UseSqlServer(
                                connectionString,
                                options =>
                                {
                                    //mysqlOptions.SchemaBehavior(MySqlSchemaBehavior.Ignore); // Change schema behavior
                                    options.MigrationsAssembly("CR.HostConsole");
                                }
                            );
                            options.UseOpenIddict();
                        });

                        services.AddHttpContextAccessor();
                    }
                );
    }
}
