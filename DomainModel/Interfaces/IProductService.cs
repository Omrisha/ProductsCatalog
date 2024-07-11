namespace DomainModel.Interfaces;

public interface IProductService
{
    Task<IEnumerable<ProductDto>> GetProductsAsync();
    Task<ProductDto> GetProductByIdAsync(Guid id);
    Task<IEnumerable<ProductDto>> GetProductsByCategoryAsync(CategoryEnum category);
    Task<IEnumerable<ProductDto>> GetProductsByPriceLimitAsync(decimal priceLimit);
    Task<CreateProductOutput> CreateProductAsync(CreateProductInput product);
    Task<UpdateProductOutput> UpdateProductAsync(UpdateProductInput product);
    Task<DeleteProductOutput> DeleteProductAsync(Guid id);
}