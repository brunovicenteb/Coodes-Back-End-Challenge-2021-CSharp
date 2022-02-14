﻿using AutoMapper;
using Coodesh.Back.End.Challenge2021.CSharp.Domain.Entities;
using Coodesh.Back.End.Challenge2021.CSharp.Domain.Interfaces;
using Coodesh.Back.End.Challenge2021.CSharp.Toolkit.Interfaces;

namespace Coodesh.Back.End.Challenge2021.CSharp.Service.Services
{
    public class XArticleService : XBaseService<XArticle>, XIArticleService
    {
        public XArticleService(XIArticleRepository pRepository, XICache pCache, IMapper pMapper)
            : base(pRepository, pCache, pMapper)
        {
        }

        protected override string EntityName => "Article";
    }
}