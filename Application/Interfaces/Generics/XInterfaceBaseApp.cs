using System.Threading.Tasks;
using System.Collections.Generic;

namespace Coodesh.Back.End.Challenge2021.CSharp.Application.Interfaces.Generics
{
    public interface XInterfaceBaseApp<T> where T : class
    {
        Task<long> Count();
        Task<T> Add(T pValue);
        Task<T> Update(T pValue);
        Task<bool> Delete(int pObjectID);
        Task<T> GetObjectByID(int pObjectID);
        Task<IEnumerable<T>> Get(int? pLimit, int? pStart);
    }
}