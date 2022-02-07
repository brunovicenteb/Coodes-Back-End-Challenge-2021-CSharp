using MongoDB.Driver;
using Coodesh.Back.End.Challenge2021.CSharp.Entities.Entities;

namespace Coodesh.Back.End.Challenge2021.CSharp.Infrastructure.Configuration
{
    public class XContextBase
    {
        public IMongoCollection<XArticle> Articles
        {
            get;
        }
    }
}