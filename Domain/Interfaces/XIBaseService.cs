using FluentValidation;
using System.Collections.Generic;
using Coodesh.Back.End.Challenge2021.CSharp.Domain.Entities;
using Coodesh.Back.End.Challenge2021.CSharp.Toolkit.Interfaces;

namespace Coodesh.Back.End.Challenge2021.CSharp.Domain.Interfaces
{
    public interface XIBaseService<TEntity, TQuery>
        where TEntity : XBaseEntity
        where TQuery : XICustomQueryable

    {
        long Count();

        bool Delete(int pObjectID);

        IEnumerable<TEntity> Get(TQuery pQuery);

        TEntity GetObjectByID(int pObjectID);

        TEntity Add<TValidator>(TEntity pInput)
            where TValidator : AbstractValidator<TEntity>;

        TEntity Update<TValidator>(int pObjectID, TEntity pInput)
            where TValidator : AbstractValidator<TEntity>;
    }
}