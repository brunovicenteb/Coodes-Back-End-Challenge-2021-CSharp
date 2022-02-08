using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Coodesh.Back.End.Challenge2021.CSharp.Entities.Entities;
using Coodesh.Back.End.Challenge2021.CSharp.Application.Interfaces;
using Coodesh.Back.End.Challenge2021.CSharp.Core.Interfaces.InterfaceArticle;

namespace Coodesh.Back.End.Challenge2021.CSharp.Application.OpenApp
{
    public class XAppArticle : XIArticleApp
    {
        public XAppArticle(XIArticle pArticle)
        {
            _Article = pArticle;
        }

        private readonly XIArticle _Article;

        public async Task<long> Count()
        {
            return await _Article.Count();
        }

        public async Task<XArticle> Add(XArticle pArticle)
        {
            return await _Article.Add(pArticle);
        }

        public async Task<bool> Delete(int pObjectID)
        {
            return await _Article.Delete(pObjectID);
        }

        public async Task<XArticle> GetObjectByID(int pObjectID)
        {
            return await _Article.GetObjectByID(pObjectID);
        }

        public async Task<XArticle> Update(XArticle pArticle)
        {
            return await _Article.Update(pArticle);
        }

        public async Task<IEnumerable<XArticle>> Get(int? pLimit, int? pStart)
        {
            return await _Article.Get(pLimit, pStart);
        }
    }
}