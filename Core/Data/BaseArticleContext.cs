using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Coodesh.Back.End.Challenge2021.CSharp.Core.Entities;

namespace Coodesh.Back.End.Challenge2021.CSharp.Core.Data
{
    public abstract class BaseArticleContext : IArticleContext
    {
        public IMongoCollection<Article> Articles
        {
            get;
            protected set;
        }
    }
}