using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Coodesh.Back.End.Challenge2021.CSharp.Core.Entities;
using Coodesh.Back.End.Challenge2021.CSharp.Core.Data;

namespace Coodesh.Back.End.Challenge2021.CSharp.Api
{
    public class ArticleContext : BaseArticleContext
    {
        public ArticleContext(IConfiguration pConfiguration)
        {
            var client = new MongoClient(pConfiguration.GetValue<string>
                ("DataBaseSettings:ConnectionString"));
            var database = client.GetDatabase(pConfiguration.GetValue<string>
                ("DataBaseSettings:DataBaseName"));
            Articles = database.GetCollection<Article>(pConfiguration.GetValue<string>
                ("DataBaseSettings:CollectionName"));
        }
    }
}