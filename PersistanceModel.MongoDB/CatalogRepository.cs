using EntityModel;
using Infrastructure.PersistenceModel.MongoDB;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace PersistanceModel.MongoDB;

public class CatalogRepository : Repository<Catalog>, ICatalogRepository
{
    public CatalogRepository(IOptionsMonitor<MongoDbSettings> options) : base(options)
    {
    }
    
    public async Task<IEnumerable<Catalog>> GetCatalogByProductIdAsync(Guid productId)
    {
        return await this.GetCollection()
            .Find(e => e.Products.Any(p => p.Id == productId))
            .ToListAsync();
    }
}