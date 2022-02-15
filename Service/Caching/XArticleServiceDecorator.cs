using Microsoft.Extensions.Logging;
using Coodesh.Back.End.Challenge2021.CSharp.Domain.Interfaces;
using Coodesh.Back.End.Challenge2021.CSharp.Toolkit.Interfaces;
using Coodesh.Back.End.Challenge2021.CSharp.Domain.Entities;
using FluentValidation;
using System.Collections.Generic;
using Coodesh.Back.End.Challenge2021.CSharp.Service.Validators;
using System;

namespace Coodesh.Back.End.Challenge2021.CSharp.Service.Caching
{
    public class XArticleServiceDecorator : XIArticleService
    {
        private const string _CountCacheKey = "count";
        private const string _ArticleKey = "Article-{0}";

        public XArticleServiceDecorator(XIArticleService pInner, XICache pCache, ILogger<XArticleServiceDecorator> pLogger)
        {
            _Inner = pInner;
            _Cache = pCache;
            _Logger = pLogger;
        }

        private readonly XIArticleService _Inner;
        private readonly XICache _Cache;
        private readonly ILogger<XArticleServiceDecorator> _Logger;

        public long Count()
        {
            long output;
            if (_Cache.TryGet(_CountCacheKey, out output))
            {
                _Logger.LogInformation("Count hit from Cache.");
                return output;
            }
            _Logger.LogInformation("Count from Database.");
            output = _Inner.Count();
            _Cache.Set(_CountCacheKey, output);
            return output;
        }

        public bool Delete(int pObjectID)
        {
            if (!_Inner.Delete(pObjectID))
                return false;
            _Cache.RemoveByPattern("get-start:*");
            _Cache.Remove(_CountCacheKey, string.Format(_ArticleKey, pObjectID));
            return true;
        }

        public IEnumerable<XArticle> Get(int? pLimit, int? pStart)
        {
            int start = pStart ?? 0;
            int limit = Math.Min(50, pLimit ?? 10);
            IEnumerable<XArticle> output;
            string key = $"Get-start:{start}-limit:{limit}";
            if (_Cache.TryGet(key, out output))
            {
                _Logger.LogInformation($"{key} from Cache.");
                return output;
            }
            _Logger.LogInformation($"{key} from Database.");
            output = _Inner.Get(limit, start);
            _Cache.Set(key, output);
            return output;
        }

        public XArticle GetObjectByID(int pObjectID)
        {
            XArticle output;
            string key = string.Format(_ArticleKey, pObjectID);
            if (_Cache.TryGet(key, out output))
            {
                _Logger.LogInformation($"{key} from Cache.");
                return output;
            }
            _Logger.LogInformation($"{key} from Database.");
            output = _Inner.GetObjectByID(pObjectID);
            _Cache.Set(key, output);
            _Cache.RemoveByPattern("get-start:*");
            return output;
        }

        public XArticle Add<TValidator>(XArticle pInput)
            where TValidator : AbstractValidator<XArticle>
        {
            XArticle output = _Inner.Add<XArticleValidator>(pInput);
            string key = string.Format(_ArticleKey, output.ID);
            _Cache.Set(key, output);
            _Cache.Remove(_CountCacheKey);
            _Cache.RemoveByPattern("get-start:*");
            return output;
        }

        public XArticle Update<TValidator>(int pObjectID, XArticle pInput)
            where TValidator : AbstractValidator<XArticle>
        {
            XArticle output = _Inner.Update<XArticleValidator>(pObjectID, pInput);
            string key = string.Format(_ArticleKey, output.ID);
            _Cache.Set(key, output);
            _Cache.RemoveByPattern("get-start:*");
            return output;
        }
    }
}