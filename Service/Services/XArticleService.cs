using AutoMapper;
using Coodesh.Back.End.Challenge2021.CSharp.Domain.Entities;
using Coodesh.Back.End.Challenge2021.CSharp.Domain.Interfaces;

namespace Coodesh.Back.End.Challenge2021.CSharp.Service.Services
{
    public class XArticleService : XBaseService<XArticle>, XIArticleService
    {
        public XArticleService(XIArticleRepository pRepository, IMapper pMapper)
            : base(pRepository, pMapper)
        {
        }

        protected override string EntityName => "Article";
    }
}