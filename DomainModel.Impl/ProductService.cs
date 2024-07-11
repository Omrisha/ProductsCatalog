using AutoMapper;
using Core;
using DomainModel.Interfaces;
using EntityModel;
using Microsoft.Extensions.Logging;
using PersistanceModel.MongoDB;

namespace DomainModel.Impl;

public class ProductService : IProductService
{
    private readonly IProductRepository productRepository;
    private readonly IMapper mapper;
    private readonly ILogger<ProductService> logger;
    
    public ProductService(
        IProductRepository productRepository,
        IMapper mapper,
        ILogger<ProductService> logger)
    {
        this.productRepository = productRepository;
        this.mapper = mapper;
        this.logger = logger;
    }
    
    public async Task<IEnumerable<ProductDto>> GetProductsAsync()
    {
        IEnumerable<Product> products = await this.productRepository
            .GetAllAsync();
        
        return this.mapper.Map<IEnumerable<Product>, IEnumerable<ProductDto>>(products);
    }

    public async Task<ProductDto> GetProductByIdAsync(Guid id)
    {
        Product product = await this.productRepository
            .GetByIdAsync(id) ?? throw new KeyNotFoundException("Product with selected key not found");
        
        return this.mapper.Map<Product, ProductDto>(product);
    }

    public async Task<IEnumerable<ProductDto>> GetProductsByCategoryAsync(CategoryEnum category)
    {
        IEnumerable<Product> products = await this.productRepository
            .GetProductsByCategoryAsync((EntityModel.CategoryEnum)category);
        
        return this.mapper.Map<IEnumerable<Product>, IEnumerable<ProductDto>>(products);
    }

    public async Task<IEnumerable<ProductDto>> GetProductsByPriceLimitAsync(decimal priceLimit)
    {
        IEnumerable<Product> products = await this.productRepository
            .GetProductsByPriceLimitAsync(priceLimit);
        
        return this.mapper.Map<IEnumerable<Product>, IEnumerable<ProductDto>>(products);
    }

    public async Task<CreateNewProductOutput> CreateProductAsync(CreateNewProductInput product)
    {
        List<string> errors = new();
            
        if (product.Category == CategoryEnum.FreshProduct)
        {
            if (!product.UniqueProperties.Any(up => up.Name == "ExpiryDate"))
            {
                errors.Add("Fresh product must have expiry date");
            }
        }

        if (product.Category == CategoryEnum.ElectricProduct)
        {
            if (!product.UniqueProperties.Any(up => up.Name == "Voltage"))
            {
                errors.Add("Electric product must have voltage");
            }
            
            if (!product.UniqueProperties.Any(up => up.Name == "SocketType"))
            {
                errors.Add("Electric product must have socket type");
            }
        }

        CreateNewProductOutput output = new();
        if (errors.Count == 0)
        {
            Product newProduct = this.mapper.Map<CreateNewProductInput, Product>(product);

            Product created = await this.productRepository
                .CreateAsync(newProduct);

            output.ProductId = created.Id;
        }
        else
        {
            output.Errors = errors;
        }
        
        return output;
    }

    public async Task UpdateProductAsync(UpdateNewProductInput product)
    {
        try
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }
            
            if (product.Category == CategoryEnum.FreshProduct)
            {
                if (!product.UniqueProperties.Any(up => up.Name == "ExpiryDate"))
                {
                    throw new ArgumentException("Fresh product must have expiry date");
                }
            }

            if (product.Category == CategoryEnum.ElectricProduct)
            {
                if (!product.UniqueProperties.Any(up => up.Name == "Voltage"))
                {
                    throw new ArgumentException("Electric product must have voltage");
                }
            
                if (!product.UniqueProperties.Any(up => up.Name == "SocketType"))
                {
                    throw new ArgumentException("Electric product must have socket type");
                }
            }

            Product productToUpdate = await this.productRepository.GetByIdAsync(product.Id);
            
            if (productToUpdate == null)
            {
                throw new KeyNotFoundException("Product with selected key not found");
            }
            
            this.mapper.Map<UpdateNewProductInput, Product>(product, productToUpdate);
            
            await this.productRepository
                .UpdateAsync(productToUpdate);
        }
        catch (Exception ex)
        {
            throw new ProductValidationException(ex.Message);
        }
    }

    public async Task DeleteProductAsync(Guid id)
    {
        Product productExists = await this.productRepository
            .GetByIdAsync(id);

        if (productExists == null)
        {
            throw new KeyNotFoundException("Product with selected key not found");
        }
        
        await this.productRepository
            .DeleteAsync(id);
    }
}