using System;
using System.IO;
using System.Reflection;
using Coodes.Back.End.Challenge2021.CSharp.Core.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace Coodes.Back.End.Challenge2021.CSharp.Api
{
    public class Startup
    {
        public IConfiguration Configuration
        {
            get;
        }

        public Startup(IConfiguration pConfiguration)
        {
            Configuration = pConfiguration;
        }

        public void ConfigureServices(IServiceCollection pServices)
        {
            pServices.AddScoped<IArticleContext, ArticleContext>();
            pServices.AddControllers();
            pServices.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Coodesh Back-end Challenge API",
                    Version = "v1",
                    Description = "This is a REST API that will use data from the Space Flight News project, " +
                        "a public API with information related to spaceflight. " +
                        "The project was created so that Coodesh has practical conditions to assess the skills " +
                        "of candidate Bruno Belchior for the vacancy of Back-end Developer.",
                    Contact = new OpenApiContact
                    {
                        Name = "Bruno Belchior",
                        Email = "brunovicenteb@gmail.com",
                        Url = new Uri("https://github.com/brunovicenteb")
                    }
                });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                opt.IncludeXmlComments(xmlPath);
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CoodeshAPI v1"));
            app.UseDeveloperExceptionPage();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", (c) => c.Response.WriteAsync("Back-end Challenge 2021 🏅 - Space Flight New",Encoding.UTF8));
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}