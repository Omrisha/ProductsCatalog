namespace DomainModel.Interfaces;

public interface IProductService
{
    Task<IEnumerable<ProductDto>> GetProductsAsync();
    Task<ProductDto> GetProductByIdAsync(Guid id);
    Task<IEnumerable<ProductDto>> GetProductsByCategoryAsync(CategoryEnum category);
    Task<IEnumerable<ProductDto>> GetProductsByPriceLimitAsync(decimal priceLimit);
    Task<CreateNewProductOutput> CreateProductAsync(CreateNewProductInput product);
    Task UpdateProductAsync(UpdateNewProductInput product);
    Task DeleteProductAsync(Guid id);
}