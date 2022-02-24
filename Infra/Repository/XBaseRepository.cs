using System;
using System.Linq;
using MongoDB.Driver;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Coodesh.Back.End.Challenge2021.CSharp.Domain.Entities;
using Coodesh.Back.End.Challenge2021.CSharp.Domain.Interfaces;

namespace Coodesh.Back.End.Challenge2021.CSharp.Infra.Repository
{
    public abstract class XBaseRepository<TEntity> : XIBaseRepository<TEntity> where TEntity : XBaseEntity
    {
        protected readonly IMongoCollection<TEntity> Objects;

        public XBaseRepository(IConfiguration pConfiguration)
        {
            string strCon = pConfiguration.GetValue<string>("DataBaseSettings:ConnectionString");
            string dataBaseName = pConfiguration.GetValue<string>("DataBaseSettings:DataBaseName");
            string collectionName = pConfiguration.GetValue<string>("DataBaseSettings:CollectionName");
            var client = new MongoClient(strCon);
            var database = client.GetDatabase(dataBaseName);
            Objects = database.GetCollection<TEntity>(collectionName);
        }

        protected abstract void UpdateData(TEntity pUpdated, TEntity pOriginal);

        public long Count()
        {
            return Objects.CountDocuments(FilterDefinition<TEntity>.Empty);
        }

        public TEntity Add(TEntity pObject)
        {
            Objects.InsertOne(pObject);
            return GetObjectByID(pObject.ID);
        }

        public TEntity Update(TEntity pObject)
        {
            TEntity t = GetObjectByID(pObject.ID);
            if (t == null)
                return null;
            UpdateData(pObject, t);
            var updateResult = Objects.ReplaceOne(
                o => o.ID == t.ID, replacement: pObject);
            if (updateResult.IsAcknowledged && updateResult.ModifiedCount > 0)
                return GetObjectByID(pObject.ID);
            return null;
        }

        public bool Delete(int pObjectID)
        {
            var deleteResult = Objects.DeleteOne(o => o.ID == pObjectID);
            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }

        public TEntity GetObjectByID(int pObjectID)
        {
            var builder = Builders<TEntity>.Filter;
            var filter = builder.Eq("ID", pObjectID);
            return Objects.Find(filter).FirstOrDefault();
        }

        public IEnumerable<TEntity> Get(int pLimit, int pOffset)
        {
            return Objects.Find(FilterDefinition<TEntity>.Empty)
                .Skip(pOffset)
                .Limit(pLimit).ToList();
        }
    }
}