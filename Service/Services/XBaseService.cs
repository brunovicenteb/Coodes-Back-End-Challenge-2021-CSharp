using System;
using FluentValidation;
using System.Collections.Generic;
using Coodesh.Back.End.Challenge2021.CSharp.Domain.Entities;
using Coodesh.Back.End.Challenge2021.CSharp.Domain.Interfaces;
using Coodesh.Back.End.Challenge2021.CSharp.Toolkit.Exceptions;

namespace Coodesh.Back.End.Challenge2021.CSharp.Service.Services
{
    public abstract class XBaseService<TEntity> : XIBaseService<TEntity> where TEntity : XBaseEntity
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

        public IEnumerable<TEntity> Get(int? pLimit, int? pStart)
        {
            int start = pStart ?? 0;
            int limit = Math.Min(50, pLimit ?? 10);
            return _BaseRepository.Get(limit, start);
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
            var validateResult = pValidator.Validate(pOject);
            if (!validateResult.IsValid)
                throw new XBadRequestException(validateResult.ToString());
        }
    }
}