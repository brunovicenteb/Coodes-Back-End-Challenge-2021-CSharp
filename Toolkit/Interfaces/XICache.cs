using System;

namespace Coodesh.Back.End.Challenge2021.CSharp.Toolkit.Interfaces
{
    public interface XICache : IDisposable
    {
        bool IsLastHit { get; }

        bool TryGet<T>(string pKey, out T pOutput);

        void Set(string pKey, object pObject);

        void Remove(params string[] pKeys);

        void RemoveByPattern(string pPattern);
    }
}