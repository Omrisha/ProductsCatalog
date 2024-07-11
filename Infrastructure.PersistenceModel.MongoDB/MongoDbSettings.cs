using Infrastructure.PersistenceModel.MongoDB.Interfaces;

namespace Infrastructure.PersistenceModel.MongoDB;

public class MongoDbSettings : IMongoDbSettings
{
    public string DatabaseName { get; set; }
    public string ConnectionString { get; set; }
}