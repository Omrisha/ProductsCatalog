using System.Linq.Expressions;
using Infrastructure.EntityModel.Interfaces;
using MongoDB.Driver;

namespace Infrastructure.PersistenceModel.MongoDB;

public interface IRepository<TEntity> where TEntity : IEntity
{
    public IMongoCollection<TEntity> GetCollection();
    public Task<IEnumerable<TEntity>> GetAllAsync();
    Task<IEnumerable<TEntity>> GetByFilterAsync(Expression<Func<TEntity, bool>> filter);
    public Task<TEntity> GetByIdAsync(Guid id);
    public Task<TEntity> CreateAsync(TEntity entity);
    public Task UpdateAsync(TEntity entity);
    public Task DeleteAsync(Guid id);
}