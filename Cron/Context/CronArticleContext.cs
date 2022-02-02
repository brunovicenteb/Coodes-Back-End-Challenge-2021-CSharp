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

namespace Coodesh.Back.End.Challenge2021.CSharp.Cron.Context
{
    public class CronArticleContext : BaseArticleContext
    {
        private const string _UrlCount = "https://api.spaceflightnewsapi.net/v3/articles/count";
        private const string _UrlArticles = "https://api.spaceflightnewsapi.net/v3/articles?_start={0}&_limit={1}";
        private const int _Limit = 100;
        public CronArticleContext(string pConnectionString, string pDataBaseName, string pCollectionName, int pMax = -1)
        {
            var client = new MongoClient(pConnectionString);
            _DB = client.GetDatabase(pDataBaseName);
            Articles = _DB.GetCollection<Article>(pCollectionName);
            _Max = pMax;
        }

        private readonly int _Max;
        private readonly IMongoDatabase _DB;

        public void Seed()
        {
            using (HttpClient c = new HttpClient())
            {
                int count = 0;
                using (var countStr = c.GetStreamAsync(_UrlCount).Result)
                    count = JsonSerializer.DeserializeAsync<int>(countStr).Result;
                Console.WriteLine($"Exists {count} documentos to process");
                if (_Max > -1)
                    count = Math.Min(count, _Max);
                int slice = (int)Math.Ceiling(Convert.ToDecimal(count) / _Limit);
                var listWrites = new List<WriteModel<Article>>();
                for (int i = 0; i < slice; i++)
                {
                    Console.WriteLine($"Start cytle {i}.");
                    int start = i * _Limit;
                    int sliceAmmount = Math.Min(count, _Limit);
                    count -= sliceAmmount;
                    string url = string.Format(_UrlArticles, start, sliceAmmount);
                    using (var st = c.GetStreamAsync(url).Result)
                    {
                        List<Article> articles = JsonSerializer.DeserializeAsync<List<Article>>(st).Result;
                        articles.ForEach(a => CreateUpdateModel(a, listWrites));
                    }
                    Console.WriteLine($"Finish cytle {i}.");
                }
                Console.WriteLine($"Before Bulk.");
                var result = Articles.BulkWrite(listWrites);
                Console.WriteLine($"Bulk Report");
                Console.WriteLine($"       Processeds: {result.ProcessedRequests.Count}");
                Console.WriteLine($"       Inserteds: {result.InsertedCount}");
                Console.WriteLine($"       Updateds: {result.ModifiedCount}");
            }
        }

        private void CreateUpdateModel(Article pArticle, List<WriteModel<Article>> pList)
        {
            var up = new UpdateManyModel<Article>(
                Builders<Article>.Filter.Where(p => p.ID == pArticle.ID),
                Builders<Article>.Update
                    .Set(p => p.Featured, pArticle.Featured)
                    .Set(p => p.Title, pArticle.Title)
                    .Set(p => p.Url, pArticle.Url)
                    .Set(p => p.ImageUrl, pArticle.ImageUrl)
                    .Set(p => p.NewsSite, pArticle.NewsSite)
                    .Set(p => p.Summary, pArticle.Summary)
            //.Set(p => p.PublishedAt, DateTime.UtcNow)
            //.Set(p => p.UpdatedAt, challengeData.UtcNow)
            //.SetOnInsert(p => p.DateCreated, DateTime.UtcNow)
            //.Set(p => p.DateUpdated, DateTime.UtcNow)
            );
            up.IsUpsert = true;
            pList.Add(up);
        }

        //public async Task RunBulkInserts()
        //{
        //    var collectionToModify = Articles;
        //    var listWrites = new List<WriteModel<Article>>();
        //    var filterToGetAllDocuments = Builders<Article>.Filter.Empty;
        //    var options = new FindOptions<Article, Article>
        //    {
        //        BatchSize = 1000
        //    };

        //    using (var cursor = await collectionToModify.FindAsync(filterToGetAllDocuments, options))
        //    {
        //        while (await cursor.MoveNextAsync())
        //        {
        //            var batch = cursor.Current;

        //            foreach (var doc in batch)
        //            {
        //                var filterForUpdate = Builders<Article>.Filter.Eq(o => o.ID, doc.ID);
        //                var updateDefinition = Builders<Article>.Update.Set(x => x.NickName, nickName);
        //                listWrites.Add(new UpdateOneModel<ExampleModel>(filterForUpdate, updateDefinition));
        //            }

        //            await collectionToModify.BulkWriteAsync(listWrites);
        //            listWrites.Clear();
        //        }
        //    }

        //}
    }
}