using MongoDB.Driver;
using Coodes.Back.End.Challenge2021.CSharp.Core.Entities;

namespace Coodes.Back.End.Challenge2021.CSharp.Core.Data
{
    public interface IArticleContext
    {
        IMongoCollection<Article> Articles
        {
            get;
        }
    }
}