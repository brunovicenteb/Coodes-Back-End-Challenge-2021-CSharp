using System;
using Coodesh.Back.End.Challenge2021.CSharp.Cron.Context;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Coodesh.Back.End.Challenge2021.CSharp.Cron
{
    public class Program
    {
        /* CommandLine example to override config:
           /DataBaseSettings:ConnectionString=mongodb://localhost:27017/?readPreference=primary&appname=MongoDB%20Compass&directConnection=true&ssl=false */
        public static void Main(string[] pArgs)
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .AddCommandLine(pArgs)
                .Build();
            var sc = new ServiceCollection();
            sc.AddLogging(builder => builder
                .AddConsole()
                .AddDebug());
            sc.AddSingleton(config);
            sc.AddSingleton<XSincronizeJob>();
            var sb = sc.BuildServiceProvider();
            var sj = sb.GetService<XSincronizeJob>();
            sj.Execute();
        }
    }
}