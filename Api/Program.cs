using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Coodesh.Back.End.Challenge2021.CSharp.Api
{
    public class Program
    {

        public static void Main(string[] args)
        {
            IConfigurationRoot cfg = GetConfiguration();
            ConfiguraLog(cfg);
            try
            {
                Log.Information("Starting WebApi");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Api not started. Catastrophic error.");
                throw;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static void ConfiguraLog(IConfigurationRoot configuration)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
        }

        private static IConfigurationRoot GetConfiguration()
        {
            string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{env}.json", optional: true)
                .Build();
            return configuration;
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
              .UseSerilog()
              .ConfigureWebHostDefaults(wb =>
              {
                  wb.UseStartup<Startup>();
                  wb.UseUrls("http://*:" + GetPort());
              });

        private static string GetPort()
        {
            return Environment.GetEnvironmentVariable("PORT") ?? "80";
        }
    }
}