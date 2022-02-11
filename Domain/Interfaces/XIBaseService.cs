using FluentValidation;
using System.Collections.Generic;
using Coodesh.Back.End.Challenge2021.CSharp.Domain.Entities;

namespace Coodesh.Back.End.Challenge2021.CSharp.Domain.Interfaces
{
    public interface XIBaseService<TEntity> where TEntity : XBaseEntity
    {
        TOutputModel Add<TInputModel, TOutputModel, TValidator>(TInputModel pInputModel)
            where TValidator : AbstractValidator<TEntity>
            where TInputModel : XBaseEntity
            where TOutputModel : XBaseEntity;

        long Count();

        bool Delete(int pObjectID);

        IEnumerable<TOutputModel> Get<TOutputModel>(int? pLimit, int? pStart) where TOutputModel : class;

        TOutputModel GetObjectByID<TOutputModel>(int pObjectID) where TOutputModel : class;

        TOutputModel Update<TInputModel, TOutputModel, TValidator>(int pObjectID, TInputModel pInputModel)
            where TValidator : AbstractValidator<TEntity>
            where TInputModel : XBaseEntity
            where TOutputModel : XBaseEntity;
    }
}