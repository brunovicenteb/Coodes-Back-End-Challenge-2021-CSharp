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
using MongoDB.Bson;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;
using Coodesh.Back.End.Challenge2021.CSharp.Cron.Entities;
using System.Text;

namespace Coodesh.Back.End.Challenge2021.CSharp.Cron.Context
{
    public class CronArticleContext : BaseArticleContext
    {
        private const string _UrlCount = "https://api.spaceflightnewsapi.net/v3/articles/count";
        private const string _UrlArticles = "https://api.spaceflightnewsapi.net/v3/articles?_start={0}&_limit={1}";
        private readonly string _ErrosSeparator = Environment.NewLine + new string('#', 80) + Environment.NewLine;
        private const int _Limit = 1000;

        public CronArticleContext(string pConnectionString, string pDataBaseName, string pCollectionName, int pMax = -1)
        {
            var settings = MongoClientSettings.FromConnectionString(pConnectionString);
            var client = new MongoClient(settings);
            _DB = client.GetDatabase(pDataBaseName);
            _Max = pMax;
        }

        private readonly int _Max;
        private readonly IMongoDatabase _DB;
        private long _Count = 0;

        public void Seed()
        {
            var logs = _DB.GetCollection<Logs>("Logs");
            Logs l = new Logs() { Title = "CronLog", Details = "Just began", ExecAt = DateTime.UtcNow };
            logs.InsertOne(l);
            var erros = new ConcurrentStack<Exception>();
            using (HttpClient c = new HttpClient())
            {
                Stopwatch st = Stopwatch.StartNew();
                int count = 0;
                using (var countStr = c.GetStreamAsync(_UrlCount).Result)
                    count = JsonSerializer.DeserializeAsync<int>(countStr).Result;
                if (_Max > -1)
                    count = Math.Min(count, _Max);
                int slices = (int)Math.Ceiling(Convert.ToDecimal(count) / _Limit);
                Console.WriteLine($"Exists {count} articles to process. Wait...");
                Parallel.For(0, slices, i => SearchSlice(i, count, c, erros));
                l.Details = $"MongoDB Bulk Process {Interlocked.Read(ref _Count)} Articles in {st.ElapsedMilliseconds} ms.";
                Console.WriteLine($"MongoDB Bulk Process {Interlocked.Read(ref _Count)} Articles in {st.ElapsedMilliseconds} ms.");
            }
            if (erros.Count > 0)
                l.Details += $"[{erros.Count} Error(s)]{Environment.NewLine}{string.Join(_ErrosSeparator, erros.Select(o => o.Message))}";
            logs.ReplaceOne(o => o.ObjectID == l.ObjectID, l);
        }

        private void SearchSlice(int pIterator, int pTotal, HttpClient pClient, ConcurrentStack<Exception> pErros)
        {
            try
            {
                int start = pIterator * _Limit;
                int limit = Math.Min(start + _Limit, pTotal) - start;
                string url = string.Format(_UrlArticles, start, limit);
                using (var st = pClient.GetStreamAsync(url).Result)
                {
                    List<Article> articles = JsonSerializer.DeserializeAsync<List<Article>>(st).Result;
                    if (articles.Count == 0)
                        return;
                    List<WriteModel<Article>> bag = new List<WriteModel<Article>>();
                    var dbArticles = _DB.GetCollection<Article>("Articles");
                    articles.ForEach(a => CreateUpdateModel(a, bag));
                    var result = dbArticles.BulkWrite(bag);
                    Interlocked.Add(ref _Count, bag.Count);
                }
            }
            catch (Exception ex)
            {
                pErros.Push(ex);
                Console.WriteLine($"Exception Iterator{pIterator} => {ex.Message}");
            }
        }

        private void CreateUpdateModel(Article pArticle, List<WriteModel<Article>> pBag)
        {
            var up = new UpdateManyModel<Article>(
                Builders<Article>.Filter.Where(p => p.ID == pArticle.ID),
                Builders<Article>.Update
                    .SetOnInsert(p => p.Featured, pArticle.Featured)
                    .SetOnInsert(p => p.Title, pArticle.Title)
                    .SetOnInsert(p => p.Url, pArticle.Url)
                    .SetOnInsert(p => p.ImageUrl, pArticle.ImageUrl)
                    .SetOnInsert(p => p.NewsSite, pArticle.NewsSite)
                    .SetOnInsert(p => p.Summary, pArticle.Summary)
                    .SetOnInsert(p => p.PublishedAt, pArticle.PublishedAt)
                    .SetOnInsert(p => p.UpdatedAt, pArticle.UpdatedAt)
                    .SetOnInsert(p => p.Launches, pArticle.Launches)
                    .SetOnInsert(p => p.Events, pArticle.Events)
            //.Set(p => p.PublishedAt, DateTime.UtcNow)
            //.Set(p => p.UpdatedAt, challengeData.UtcNow)
            //.SetOnInsert(p => p.DateCreated, DateTime.UtcNow)
            //.Set(p => p.DateUpdated, DateTime.UtcNow)
            );
            up.IsUpsert = true;
            pBag.Add(up);
        }
    }
}