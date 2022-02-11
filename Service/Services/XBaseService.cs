using System;
using AutoMapper;
using System.Linq;
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
        private readonly IMapper _Mapper;

        protected abstract string EntityName { get; }

        public XBaseService(XIBaseRepository<TEntity> pBaseRepository, IMapper pMapper)
        {
            _BaseRepository = pBaseRepository;
            _Mapper = pMapper;
        }

        public long Count()
        {
            return _BaseRepository.Count();
        }

        public bool Delete(int pObjectID)
        {
            return _BaseRepository.Delete(pObjectID);
        }

        public IEnumerable<TOutputModel> Get<TOutputModel>(int? pLimit, int? pStart) where TOutputModel : class
        {
            var entities = _BaseRepository.Get(pLimit, pStart);
            var outputModels = entities.Select(s => _Mapper.Map<TOutputModel>(s));
            return outputModels;
        }

        public TOutputModel GetObjectByID<TOutputModel>(int pObjectID) where TOutputModel : class
        {
            var entity = _BaseRepository.GetObjectByID(pObjectID);
            if (entity == null)
                throw new XNotFoundException($"{EntityName} {pObjectID} not found.");
            var outputModel = _Mapper.Map<TOutputModel>(entity);
            return outputModel;
        }

        public TOutputModel Add<TInputModel, TOutputModel, TValidator>(TInputModel pInputModel)
            where TValidator : AbstractValidator<TEntity>
            where TInputModel : XBaseEntity
            where TOutputModel : XBaseEntity
        {
            if (pInputModel == null)
                throw new XBadRequestException($"Invalid {EntityName}.");
            if (pInputModel.ID == 0)
                pInputModel.ID = Math.Abs(Guid.NewGuid().GetHashCode()) * -1;
            TEntity entity = _Mapper.Map<TEntity>(pInputModel);
            Validate(entity, Activator.CreateInstance<TValidator>());
            entity = _BaseRepository.Add(entity);
            if (entity == null)
                throw new XBadRequestException($"Error to process {EntityName}.");
            TOutputModel outputModel = _Mapper.Map<TOutputModel>(entity);
            return outputModel;
        }

        public TOutputModel Update<TInputModel, TOutputModel, TValidator>(int pObjectID, TInputModel pInputModel)
            where TValidator : AbstractValidator<TEntity>
            where TInputModel : XBaseEntity
            where TOutputModel : XBaseEntity
        {
            if (pInputModel == null)
                throw new XBadRequestException($"Invalid {EntityName}.");
            pInputModel.ID = pObjectID;
            TEntity entity = _Mapper.Map<TEntity>(pInputModel);
            Validate(entity, Activator.CreateInstance<TValidator>());
            entity = _BaseRepository.Update(entity);
            if (entity == null)
                throw new XNotFoundException($"{EntityName} {pObjectID} not found.");
            TOutputModel outputModel = _Mapper.Map<TOutputModel>(entity);
            return outputModel;
        }

        private void Validate(TEntity pOject, AbstractValidator<TEntity> pValidator)
        {
            pValidator.ValidateAndThrow(pOject);
        }
    }
}