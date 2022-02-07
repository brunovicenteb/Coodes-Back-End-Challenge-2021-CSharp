using System;
using Coodesh.Back.End.Challenge2021.CSharp.Cron.Context;
using Microsoft.Extensions.Configuration;

namespace Coodesh.Back.End.Challenge2021.CSharp.Cron
{
    public class Program
    {
        public static void Main(string[] pArgs)
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .AddCommandLine(pArgs)
                .Build();
            var connectionString = config.GetValue<string>("DataBaseSettings:ConnectionString");
            var dataBaseName = config.GetValue<string>("DataBaseSettings:DataBaseName");
            var collectionName = config.GetValue<string>("DataBaseSettings:CollectionName");
            Console.WriteLine($"ConectionString: {connectionString};");
            Console.WriteLine($"DataBaseName: {dataBaseName};");
            Console.WriteLine($"CollectionName: {collectionName};");
            CronArticleContext ct = new CronArticleContext(connectionString, dataBaseName, collectionName, -1);
            ct.Seed();
        }
    }
}