using System;
using FluentValidation;
using System.Collections.Generic;
using Coodesh.Back.End.Challenge2021.CSharp.Domain.Entities;
using Coodesh.Back.End.Challenge2021.CSharp.Domain.Interfaces;
using Coodesh.Back.End.Challenge2021.CSharp.Toolkit.Exceptions;
using Coodesh.Back.End.Challenge2021.CSharp.Toolkit.Interfaces;

namespace Coodesh.Back.End.Challenge2021.CSharp.Service.Services
{
    public abstract class XBaseService<TEntity, TQuery> : XIBaseService<TEntity, TQuery>
        where TEntity : XBaseEntity
        where TQuery : XICustomQueryable
    {
        private readonly XIBaseRepository<TEntity> _BaseRepository;

        protected abstract string EntityName { get; }

        public XBaseService(XIBaseRepository<TEntity> pBaseRepository)
        {
            _BaseRepository = pBaseRepository;
        }

        public long Count()
        {
            return _BaseRepository.Count();
        }

        public bool Delete(int pObjectID)
        {
            return _BaseRepository.Delete(pObjectID);
        }

        public IEnumerable<TEntity> Get(TQuery pQuery)
        {
            int offset = pQuery.Offset ?? 0;
            int limit = Math.Min(50, pQuery.Limit ?? 10);
            return _BaseRepository.Get(limit, offset);
        }

        public TEntity GetObjectByID(int pObjectID)
        {
            TEntity output = _BaseRepository.GetObjectByID(pObjectID);
            if (output == null)
                throw new XNotFoundException($"{EntityName} {pObjectID} not found.");
            return output;
        }

        public TEntity Add<TValidator>(TEntity pInput)
            where TValidator : AbstractValidator<TEntity>
        {
            if (pInput == null)
                throw new XBadRequestException($"Invalid {EntityName}.");
            if (pInput.ID == 0)
                pInput.ID = Math.Abs(Guid.NewGuid().GetHashCode()) * -1;
            Validate(pInput, Activator.CreateInstance<TValidator>());
            pInput = _BaseRepository.Add(pInput);
            if (pInput == null)
                throw new XBadRequestException($"Error to process {EntityName}.");
            return pInput;
        }

        public TEntity Update<TValidator>(int pObjectID, TEntity pInput)
            where TValidator : AbstractValidator<TEntity>
        {
            if (pInput == null)
                throw new XBadRequestException($"Invalid {EntityName}.");
            pInput.ID = pObjectID;
            Validate(pInput, Activator.CreateInstance<TValidator>());
            pInput = _BaseRepository.Update(pInput);
            if (pInput == null)
                throw new XNotFoundException($"{EntityName} {pObjectID} not found.");
            return pInput;
        }

        private void Validate(TEntity pOject, AbstractValidator<TEntity> pValidator)
        {
            pValidator.ValidateAndThrow(pOject);
        }
    }
}