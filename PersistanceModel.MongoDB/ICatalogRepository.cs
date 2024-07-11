using EntityModel;
using MongoDB.Driver;

namespace PersistanceModel.MongoDB;

public interface ICatalogRepository
{
    Task<Catalog> GetCatalogByProductIdAsync(Guid productId);
    IMongoCollection<Catalog> GetCollection();
    Task<IEnumerable<Catalog>> GetAllAsync();
    Task<Catalog> GetByIdAsync(Guid id);
    Task<Catalog> CreateAsync(Catalog entity);
    Task UpdateAsync(Catalog entity);
    Task DeleteAsync(Guid id);
}