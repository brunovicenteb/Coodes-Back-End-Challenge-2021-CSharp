using Coodesh.Back.End.Challenge2021.CSharp.Cron.Context;
using System;
using System.Diagnostics;

namespace Coodesh.Back.End.Challenge2021.CSharp.Cron
{
    public class Program
    {
        public static void Main(string[] pArgs)
        {
            string connectionString = "mongodb+srv://brunovicenteb-Coodes-Back-End-Challenge-2021-CSharp:pDLvrVa4m0LUDmKc@cluster0.udphe.mongodb.net/myFirstDatabase?retryWrites=true&w=majority";
            string dataBaseName = "BackEndChallengeDB";
            string collectionName = "Articles";
            if (pArgs != null && pArgs.Length == 3)
            {
                connectionString = pArgs[0];
                dataBaseName = pArgs[1];
                collectionName = pArgs[2];
            }
            Console.WriteLine($"ConectionString: {connectionString};");
            Console.WriteLine($"DataBaseName: {dataBaseName};");
            Console.WriteLine($"CollectionName: {collectionName};");
            CronArticleContext ct = new CronArticleContext(connectionString, dataBaseName, collectionName, -1);
            ct.Seed();
        }
    }
}