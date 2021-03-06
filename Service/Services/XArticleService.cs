using Coodesh.Back.End.Challenge2021.CSharp.Domain.Entities;
using Coodesh.Back.End.Challenge2021.CSharp.Domain.Interfaces;

namespace Coodesh.Back.End.Challenge2021.CSharp.Service.Services
{
    public class XArticleService : XBaseService<XArticle>, XIArticleService
    {
        public XArticleService(XIArticleRepository pRepository)
            : base(pRepository)
        {
        }

        protected override string EntityName => "Article";
    }
}