namespace Infrastructure.PersistenceModel.MongoDB.Interfaces;

public interface IMongoDbSettings
{
     string DatabaseName { get; set; }
     string ConnectionString { get; set; }
}