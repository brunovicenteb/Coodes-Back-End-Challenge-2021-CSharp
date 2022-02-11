using System.Collections.Generic;
using Coodesh.Back.End.Challenge2021.CSharp.Domain.Entities;

namespace Coodesh.Back.End.Challenge2021.CSharp.Domain.Interfaces
{
    public interface XIBaseRepository<TEntity> where TEntity : XBaseEntity
    {
        long Count();
        TEntity Add(TEntity pValue);
        TEntity Update(TEntity pValue);
        bool Delete(int pObjectID);
        TEntity GetObjectByID(int pObjectID);
        IEnumerable<TEntity> Get(int? pLimit, int? pStart);
    }
}