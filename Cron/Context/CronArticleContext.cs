using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Coodesh.Back.End.Challenge2021.CSharp.Core.Entities;
using Coodesh.Back.End.Challenge2021.CSharp.Core.Data;
using System;

namespace Coodesh.Back.End.Challenge2021.CSharp.Cron.Context
{
    public class CronArticleContext : BaseArticleContext
    {
        private const string _UrlCount = "https://api.spaceflightnewsapi.net/v3/articles/count";
        private const string _UrlArticles = "https://api.spaceflightnewsapi.net/v3/articles?_start={0}&_limit={1}";
        private const int _Limit = 500;
        public CronArticleContext(string pConnectionString, string pDataBaseName, string pCollectionName, int pMax = -1)
        {
            var client = new MongoClient(pConnectionString);
            var database = client.GetDatabase(pDataBaseName);
            Articles = database.GetCollection<Article>(pCollectionName);
            _Max = pMax;
        }

        private readonly int _Max;

        public int Seed()
        {
            using (HttpClient c = new HttpClient())
            {
                int count = 0;
                int totalArticles = 0;
                using (var countStr = c.GetStreamAsync(_UrlCount).Result)
                    count = totalArticles = JsonSerializer.DeserializeAsync<int>(countStr).Result;
                if (_Max > -1)
                    totalArticles = Math.Min(totalArticles, _Max);
                int slice = (int)Math.Ceiling(Convert.ToDecimal(count) / _Limit);
                for (int i = 0; i < slice; i++)
                {
                    int start = i * _Limit;
                    int limit = Math.Min(totalArticles, _Limit);
                    totalArticles -= limit;
                    string url = string.Format(_UrlArticles, start, limit);
                    using (var st = c.GetStreamAsync(url).Result)
                    {
                        List<Article> articles = JsonSerializer.DeserializeAsync<List<Article>>(st).Result;
                        Articles.InsertMany(articles);
                        Console.Write($"{i}:{limit} ");
                    }
                }
                return count;
            }
        }
    }
}