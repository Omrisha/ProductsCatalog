using EntityModel;
using Infrastructure.PersistenceModel.MongoDB;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Moq;
using PersistanceModel.MongoDB;

namespace DataAccess.PersistenceModel.UnitTests;

public class ProductRepositoryTests
{
    [Fact]
    public void CanGetAllProducts()
    {
        // GIVEN a product repository with two products
        Product mockProduct1 = new Product()
        {
            Id = Guid.NewGuid(),
            Title = "Product 1",
            Price = 10,
            Category = CategoryEnum.ElectricProduct,
            Description = "Description 1",
            UniqueProperties = new()
            {
                new UniqueProperty() { Name = "Voltage", Value = "220V" },
                new UniqueProperty() { Name = "SocketType", Value = "UK" },
            },
        };
        Product mockProduct2 = new Product()
        {
            Id = Guid.NewGuid(),
            Title = "Product 2",
            Price = 20,
            Category = CategoryEnum.ElectricProduct,
            Description = "Description 2",
            UniqueProperties = new()
            {
                new UniqueProperty() { Name = "Voltage", Value = "220V" },
                new UniqueProperty() { Name = "SocketType", Value = "EU" },
            },
        };
        Mock<IOptions<MongoDbSettings>> options = new();
        Mock<IMongoCollection<Product>> collection = new();
        
        Mock<IProductRepository> repository = new();
        repository.Setup(r => r.GetCollection()).Returns(collection.Object);
        repository.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Product>()
        {
            mockProduct1,
            mockProduct2
        });
        
        // WHEN getting all products
        IEnumerable<Product> products = repository.Object.GetAllAsync().Result;
        
        // THEN the result should contain the two products
        Assert.Equal(products.Count(), new List<Product>() { mockProduct1, mockProduct2 }.Count);
    }
}