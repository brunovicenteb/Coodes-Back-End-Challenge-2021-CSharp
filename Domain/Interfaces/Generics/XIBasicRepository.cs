using System.Threading.Tasks;
using System.Collections.Generic;
using Coodesh.Back.End.Challenge2021.CSharp.Entities.Entities;

namespace Coodesh.Back.End.Challenge2021.CSharp.Domain.Interfaces.Generics
{
    public interface XIBasicRepository<T> where T : XArtifact
    {
        Task<long> Count();
        Task<T> Add(T pValue);
        Task<T> Update(T pValue);
        Task<bool> Delete(int pObjectID);
        Task<T> GetObjectByID(int pObjectID);
        Task<IEnumerable<T>> Get(int? pLimit, int? pStart);
    }
}