using EntityModel;
using MongoDB.Driver;

namespace PersistanceModel.MongoDB;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetProductsByCategoryAsync(CategoryEnum cateogry);
    Task<IEnumerable<Product>> GetProductsByPriceLimitAsync(decimal priceLimit);
    IMongoCollection<Product> GetCollection();
    Task<IEnumerable<Product>> GetAllAsync();
    Task<Product> GetByIdAsync(Guid id);
    Task<Product> CreateAsync(Product entity);
    Task UpdateAsync(Product entity);
    Task DeleteAsync(Guid id);
}