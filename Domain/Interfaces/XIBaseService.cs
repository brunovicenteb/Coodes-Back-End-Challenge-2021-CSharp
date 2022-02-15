using FluentValidation;
using System.Collections.Generic;
using Coodesh.Back.End.Challenge2021.CSharp.Domain.Entities;

namespace Coodesh.Back.End.Challenge2021.CSharp.Domain.Interfaces
{
    public interface XIBaseService<TEntity> where TEntity : XBaseEntity
    {
        long Count();

        bool Delete(int pObjectID);

        IEnumerable<TEntity> Get(int? pLimit, int? pStart);

        TEntity GetObjectByID(int pObjectID);

        TEntity Add<TValidator>(TEntity pInput)
            where TValidator : AbstractValidator<TEntity>;

        TEntity Update<TValidator>(int pObjectID, TEntity pInput)
            where TValidator : AbstractValidator<TEntity>;
    }
}