using System;
using System.Linq;
using MongoDB.Driver;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using Microsoft.Extensions.Configuration;
using Coodesh.Back.End.Challenge2021.CSharp.Domain.Entities;

namespace Coodesh.Back.End.Challenge2021.CSharp.Cron.Context
{
    public class XSincronizeJob
    {
        private const string _UrlCount = "https://api.spaceflightnewsapi.net/v3/articles/count";
        private const string _UrlArticles = "https://api.spaceflightnewsapi.net/v3/articles?_start={0}&_limit={1}";
        private readonly string _ErrosSeparator = Environment.NewLine + new string('#', 80) + Environment.NewLine;
        private const int _Limit = 1000;

        public XSincronizeJob(IConfiguration pConfig, ILogger<XSincronizeJob> pLogger, int pMax = -1)
        {
            var conString = pConfig.GetValue<string>("DataBaseSettings:ConnectionString");
            var dataBaseName = pConfig.GetValue<string>("DataBaseSettings:DataBaseName");
            var collectionName = pConfig.GetValue<string>("DataBaseSettings:CollectionName");
            var settings = MongoClientSettings.FromConnectionString(conString);
            var client = new MongoClient(settings);
            _Logger = pLogger;
            _DB = client.GetDatabase(dataBaseName);
            _Max = pMax;
        }

        private readonly int _Max;
        private readonly IMongoDatabase _DB;
        private ILogger<XSincronizeJob> _Logger;
        private long _Count = 0;

        public void Execute()
        {
            var logs = _DB.GetCollection<XLogs>("Logs");
            XLogs l = new XLogs() { Title = "CronLog", Details = "Just began", ExecAt = DateTime.UtcNow };
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
                _Logger.LogInformation($"Exists {count} articles to process. Wait...");
                Parallel.For(0, slices, i => SearchSlice(i, count, c, erros));
                l.Details = $"MongoDB Bulk Process {Interlocked.Read(ref _Count)} Articles in {st.ElapsedMilliseconds} ms.";
                _Logger.LogInformation($"MongoDB Bulk Process {Interlocked.Read(ref _Count)} Articles in {st.ElapsedMilliseconds} ms.");
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
                    List<XArticle> articles = JsonSerializer.DeserializeAsync<List<XArticle>>(st).Result;
                    if (articles.Count == 0)
                        return;
                    List<WriteModel<XArticle>> bag = new List<WriteModel<XArticle>>();
                    var dbArticles = _DB.GetCollection<XArticle>("Articles");
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

        private void CreateUpdateModel(XArticle pArticle, List<WriteModel<XArticle>> pBag)
        {
            var up = new UpdateManyModel<XArticle>(
                Builders<XArticle>.Filter.Where(p => p.ID == pArticle.ID),
                Builders<XArticle>.Update
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