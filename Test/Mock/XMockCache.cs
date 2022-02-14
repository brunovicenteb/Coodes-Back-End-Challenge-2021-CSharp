using System;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;
using Coodesh.Back.End.Challenge2021.CSharp.Toolkit.Cache;

namespace Coodesh.Back.End.Challenge2021.CSharp.Test.Mock
{
    public sealed class XMockCache : XBaseCache
    {
        private readonly MemoryCache _Cache = new MemoryCache(new MemoryCacheOptions { ExpirationScanFrequency = TimeSpan.FromSeconds(10) });

        public override void Dispose()
        {
            _Cache.Dispose();
        }

        protected override void Remove(string pKey)
        {
            _Cache.Remove(pKey);
        }

        public override void Set(string pKey, object pObject)
        {
            _Cache.Set(pKey, pObject);
        }

        protected override bool TryGetFromCache<T>(string pKey, out T pOutput)
        {
            return _Cache.TryGetValue<T>(pKey, out pOutput);
        }
    }
}