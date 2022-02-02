using MongoDB.Driver;
using Coodesh.Back.End.Challenge2021.CSharp.Core.Entities;

namespace Coodesh.Back.End.Challenge2021.CSharp.Core.Data
{
    public interface IArticleContext
    {
        IMongoCollection<Article> Articles
        {
            get;
        }
    }
}