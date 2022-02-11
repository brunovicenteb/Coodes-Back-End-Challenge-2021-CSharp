using Microsoft.Extensions.Configuration;
using Coodesh.Back.End.Challenge2021.CSharp.Domain.Entities;
using Coodesh.Back.End.Challenge2021.CSharp.Domain.Interfaces;

namespace Coodesh.Back.End.Challenge2021.CSharp.Infra.Repository
{
    public class XArticleRepository : XBaseRepository<XArticle>, XIArticleRepository
    {
        public XArticleRepository(IConfiguration pConfiguration)
            : base(pConfiguration)
        {
        }

        protected override void UpdateData(XArticle pUpdated, XArticle pOriginal)
        {
            pUpdated.ObjectID = pOriginal.ObjectID;
        }
    }
}