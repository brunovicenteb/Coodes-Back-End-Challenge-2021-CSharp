using System;
using System.IO;
using System.Reflection;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Coodesh.Back.End.Challenge2021.CSharp.Infra.Cache;
using Coodesh.Back.End.Challenge2021.CSharp.Service.Caching;
using Coodesh.Back.End.Challenge2021.CSharp.Service.Services;
using Coodesh.Back.End.Challenge2021.CSharp.Infra.Repository;
using Coodesh.Back.End.Challenge2021.CSharp.Domain.Interfaces;
using Coodesh.Back.End.Challenge2021.CSharp.Toolkit.Interfaces;

namespace Coodesh.Back.End.Challenge2021.CSharp.Api.Startups
{
    public static class ArticleStartup
    {
        public static void ConfigureServiceArticle(this IServiceCollection pServices)
        {
            pServices.AddSingleton<XICache, XReddisCache>();
            pServices.AddScoped<XIArticleRepository, XArticleRepository>();
            pServices.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", CreteOpenApiInfo());
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                opt.IncludeXmlComments(xmlPath);
                xmlFile = $"Coodesh.Back.End.Challenge2021.CSharp.Domain.xml";
                xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                opt.IncludeXmlComments(xmlPath);
            });
            RegisterServices(pServices);
            EnableDecorator(pServices);
        }

        private static void RegisterServices(IServiceCollection pServices)
        {
            pServices.AddScoped<XIArticleService, XArticleService>();
        }

        private static void EnableDecorator(IServiceCollection pServices)
        {
            pServices.Decorate<XIArticleService, XArticleServiceDecorator>();
        }

        public static void ConfigureArticle(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CoodeshAPI v1"));
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", (c) => c.Response.WriteAsync("Back-end Challenge 2021 - Space Flight New"));
                endpoints.MapDefaultControllerRoute();
            });
        }

        private static OpenApiInfo CreteOpenApiInfo()
        {
            return new OpenApiInfo
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
            };
        }
    }
}