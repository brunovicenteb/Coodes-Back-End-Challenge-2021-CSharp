using Coodesh.Back.End.Challenge2021.CSharp.Cron.Context;
using System;
using System.Diagnostics;

namespace Coodesh.Back.End.Challenge2021.CSharp.Cron
{
    public class Program
    {
        public static void Main(string[] pArgs)
        {
            string connectionString = pArgs[0]; //mongodb://localhost:27017,
            string dataBaseName = pArgs[1]; //BackEndChallengeDB",
            string collectionName = pArgs[2]; //Articles 
            Console.WriteLine($"ConectionString: {connectionString};");
            Console.WriteLine($"DataBaseName: {dataBaseName};");
            Console.WriteLine($"CollectionName: {collectionName};");
            CronArticleContext ct = new CronArticleContext(connectionString, dataBaseName, collectionName, -1);
            ct.Seed();
            Console.ReadKey();
        }
    }
}