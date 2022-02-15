using Coodesh.Back.End.Challenge2021.CSharp.Toolkit.Interfaces;

namespace Coodesh.Back.End.Challenge2021.CSharp.Toolkit.Cache
{
    public abstract class XBaseCache : XICache
    {
        private bool _IsLastHit;
        public bool IsLastHit => _IsLastHit;

        public abstract void Set(string pKey, object pObject);
        public abstract void Dispose();

        protected abstract bool TryGetFromCache<T>(string pKey, out T pOutput);

        protected abstract void TryRemoveByPattern(string pPattern);

        protected abstract void Remove(string pKey);

        public void RemoveByPattern(string pPattern)
        {
            if (string.IsNullOrEmpty(pPattern))
                return;
            _IsLastHit = false;
            TryRemoveByPattern(pPattern);
        }

        public bool TryGet<T>(string pKey, out T pOutput)
        {
            return _IsLastHit = TryGetFromCache<T>(pKey, out pOutput);
        }

        public void Remove(params string[] pKeys)
        {
            if (pKeys == null || pKeys.Length == 0)
                return;
            _IsLastHit = false;
            foreach (var key in pKeys)
                Remove(key);
        }
    }
}