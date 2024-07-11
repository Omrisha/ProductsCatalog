using EntityModel;
using Infrastructure.PersistenceModel.MongoDB;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace PersistanceModel.MongoDB;

public class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(IOptionsMonitor<MongoDbSettings> options) : base(options)
    {
    }
    
    public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(CategoryEnum cateogry)
    {
        return await this.GetCollection()
            .Find(e => e.Category == cateogry)
            .ToListAsync();
    }
    
    public async Task<IEnumerable<Product>> GetProductsByPriceLimitAsync(decimal priceLimit)
    {
        return await this.GetCollection()
            .Find(e => e.Price <= priceLimit)
            .ToListAsync();
    }
}