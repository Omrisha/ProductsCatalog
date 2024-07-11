using Infrastructure.EntityModel.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Infrastructure.PersistenceModel.MongoDB;

public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : IEntity
{
    private readonly IMongoCollection<TEntity> collection;

    public Repository(IOptionsMonitor<MongoDbSettings> options)
    {
        MongoDbSettings mongoDbSettings = options.CurrentValue;

        if (mongoDbSettings is null)
        {
            throw new ArgumentNullException(nameof(mongoDbSettings));
        }
        
        var client = new MongoClient(mongoDbSettings.ConnectionString);
        var database = client.GetDatabase(mongoDbSettings.DatabaseName);
        this.collection = database.GetCollection<TEntity>(typeof(TEntity).Name);
    }
    
    public IMongoCollection<TEntity> GetCollection()
    {
        return this.collection;
    }
    
    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await this.collection.Find(_ => true).ToListAsync();
    }
    
    public async Task<TEntity> GetByIdAsync(Guid id)
    {
        return await this.collection.Find(e => e.Id == id).FirstOrDefaultAsync();
    }
    
    public async Task<TEntity> CreateAsync(TEntity entity)
    {
        await this.collection.InsertOneAsync(entity);

        return entity;
    }
    
    public async Task UpdateAsync(TEntity entity)
    {
        await this.collection.ReplaceOneAsync(e => e.Id == entity.Id, entity);
    }
    
    public async Task DeleteAsync(Guid id)
    {
        await this.collection.DeleteOneAsync(e => e.Id == id);
    }
}