using MongoDB.Driver;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Coodesh.Back.End.Challenge2021.CSharp.Entities.Entities;
using Coodesh.Back.End.Challenge2021.CSharp.Core.Interfaces.InterfaceArticle;
using Coodesh.Back.End.Challenge2021.CSharp.Infrastructure.Repository.Generics;

namespace Coodesh.Back.End.Challenge2021.CSharp.Core.Repository.Repositories
{
    public class XRepositoryArticle : XBaseRepository<XArticle>, XIArticle
    {

        public XRepositoryArticle(IConfiguration pConfiguration)
            : base(pConfiguration)
        {
        }
        public override async Task<XArticle> Update(XArticle pObject)
        {
            XArticle t = GetObjectByID(pObject.ID).Result;
            pObject.ObjectID = t.ObjectID;
            var updateResult = await Objects.ReplaceOneAsync(
                o => o.ObjectID == t.ObjectID, replacement: pObject);
            if (updateResult.IsAcknowledged && updateResult.ModifiedCount > 0)
                return await GetObjectByID(pObject.ID);
            return null;
        }
    }
}