using System;
using MongoDB.Driver;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Configuration;
using Coodesh.Back.End.Challenge2021.CSharp.Entities.Entities;
using Coodesh.Back.End.Challenge2021.CSharp.Domain.Interfaces.Generics;

namespace Coodesh.Back.End.Challenge2021.CSharp.Infrastructure.Repository.Generics
{
    public abstract class XBaseRepository<T> : XIBasicRepository<T>, IDisposable where T : XArtifact
    {
        private bool _IsDisposed = false;
        private readonly SafeHandle _Handle = new SafeFileHandle(IntPtr.Zero, true);
        protected readonly IMongoCollection<T> Objects;

        public XBaseRepository(IConfiguration pConfiguration)
        {
            var client = new MongoClient(pConfiguration.GetValue<string>
                ("DataBaseSettings:ConnectionString"));
            var database = client.GetDatabase(pConfiguration.GetValue<string>
                ("DataBaseSettings:DataBaseName"));
            Objects = database.GetCollection<T>(pConfiguration.GetValue<string>
                    ("DataBaseSettings:CollectionName"));
        }

        public abstract Task<T> Update(T pObject);

        public async Task<long> Count()
        {
            return await Objects.CountDocumentsAsync(FilterDefinition<T>.Empty);
        }

        public async Task<T> Add(T pObject)
        {
            int newID = pObject.ID;
            if (newID <= 0)
                newID = pObject.ID = Guid.NewGuid().GetHashCode();
            await Objects.InsertOneAsync(pObject);
            return await GetObjectByID(newID);
        }

        public async Task<bool> Delete(int pObjectID)
        {
            var deleteResult = await Objects.DeleteOneAsync(
                o => o.ID == pObjectID);
            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }

        public async Task<T> GetObjectByID(int pObjectID)
        {
            var builder = Builders<T>.Filter;
            var filter = builder.Eq("ID", pObjectID);
            return await Objects.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> Get(int? pLimit, int? pStart)
        {
            int skip = pStart ?? 0;
            int limit = Math.Min(50, pLimit ?? 10);
            return await Objects.Find(FilterDefinition<T>.Empty)
                .Skip(skip)
                .Limit(limit)
                .ToListAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool pDisposing)
        {
            if (_IsDisposed)
                return;
            if (pDisposing)
                _Handle.Dispose();
            _IsDisposed = true;
        }
    }
}