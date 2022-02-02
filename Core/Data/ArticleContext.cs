﻿using Coodes.Back.End.Challenge2021.CSharp.Core.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Coodes.Back.End.Challenge2021.CSharp.Core.Data
{
    public class ArticleContext : IArticleContext
    {
        public ArticleContext(IConfiguration pConfiguration)
        {
            var client = new MongoClient(pConfiguration.GetValue<string>
                ("DataBaseSettings:ConnectionString"));
            var database = client.GetDatabase(pConfiguration.GetValue<string>
                ("DataBaseSettings:DataBaseName"));
            Articles = database.GetCollection<Article>(pConfiguration.GetValue<string>
                ("DataBaseSettings:CollectionName"));
            Seed(Articles);
        }

        public IMongoCollection<Article> Articles
        {
            get;
        }

        private void Seed(IMongoCollection<Article> pArticles)
        {
            if (pArticles.Find(o => true).Any())
                return;
            List<Article> articles = GetRemoteArticles().Result;
            pArticles.InsertManyAsync(articles);
        }

        private async Task<List<Article>> GetRemoteArticles()
        {
            using (HttpClient c = new HttpClient())
            {
                var st = c.GetStreamAsync("https://api.spaceflightnewsapi.net/v3/articles?_limit=100");
                var articles = await JsonSerializer.DeserializeAsync<List<Article>>(await st);
                return articles;
            }
        }
    }
}