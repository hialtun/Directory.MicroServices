using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MicroServices.Core.Entity;
using MicroServices.Infrastructure.Utils;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace MicroServices.Infrastructure.Repository
{
    public class GenericRepository<TDocument> : IRepository<TDocument> where TDocument : DocumentEntity, new()
    {
        protected readonly IMongoCollection<TDocument> Collection;
        private readonly IOptions<DatabaseSettings> _settings;

        public GenericRepository(IOptions<DatabaseSettings> settings)
        {
            _settings = settings;
            var client = new MongoClient(_settings.Value.ConnectionString);
            var db = client.GetDatabase(_settings.Value.DatabaseName);
            Collection = db.GetCollection<TDocument>(typeof(TDocument).Name.ToLowerInvariant());
        }

        public virtual IQueryable<TDocument> Get(Expression<Func<TDocument, bool>> predicate = null)
        {
            return predicate == null
                ? Collection.AsQueryable()
                : Collection.AsQueryable().Where(predicate);
        }

        public virtual Task<TDocument> GetByIdAsync(string id)
        {
            return Collection.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public virtual async Task<TDocument> CreateAsync(TDocument document)
        {
            var options = new InsertOneOptions { BypassDocumentValidation = false };
            await Collection.InsertOneAsync(document, options);
            return document;
        }

        public virtual async Task<TDocument> UpdateAsync(TDocument document)
        {
            return await Collection.FindOneAndReplaceAsync(x => x.Id == document.Id, document);
        }

        public virtual async Task<TDocument> DeleteAsync(string id)
        {
            return await Collection.FindOneAndDeleteAsync(x => x.Id == id);
        }
    }
}