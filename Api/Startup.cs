using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Coodesh.Back.End.Challenge2021.CSharp.Api.Startups;
using Microsoft.Extensions.Logging;

namespace Coodesh.Back.End.Challenge2021.CSharp.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection pServices)
        {
            pServices.AddControllers();
            pServices.ConfigureServiceArticle();
        }

        public void Configure(IApplicationBuilder pApp, IWebHostEnvironment pEnv, ILoggerFactory pLog)
        {
            pApp.UseDeveloperExceptionPage();
            pApp.UseRouting();
            pApp.ConfigureArticle(pEnv);
            var logger = pLog.CreateLogger<Startup>();
            logger.LogInformation("#############################################################");
            logger.LogInformation("###                Executando Configure                   ###");
            logger.LogInformation("#############################################################");
        }
    }
}