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

    public async Task<CreateProductOutput> CreateProductAsync(CreateProductInput product)
    {
        CreateProductOutput output = new()
        {
            Errors = new(),
        };

        // Checking if product is a Fresh product that it has an Expiry date
        // or if product is a Electric product that it has both voltage and socket type
        output.Errors = product.Category switch
        {
            CategoryEnum.FreshProduct => FreshProductValidations(product.UniqueProperties),
            CategoryEnum.ElectricProduct => ElectricalProductValidations(product.UniqueProperties),
            _ => new List<string> { "Invalid product category" },
        };

        if (output.Errors.Count == 0)
        {
            Product newProduct = this.mapper.Map<CreateProductInput, Product>(product);

            Product created = await this.productRepository
                .CreateAsync(newProduct);

            output.ProductId = created.Id;
        }

        return output;
    }
    

    public async Task<UpdateProductOutput> UpdateProductAsync(UpdateProductInput product)
    {
        UpdateProductOutput output = new()
        {
            Errors = new(),
        };

        // Checking if product is a Fresh product that it has an Expiry date
        // or if product is a Electric product that it has both voltage and socket type
        output.Errors = product.Category switch
        {
            CategoryEnum.FreshProduct => FreshProductValidations(product.UniqueProperties),
            CategoryEnum.ElectricProduct => ElectricalProductValidations(product.UniqueProperties),
            _ => new List<string> { "Invalid product category" },
        };

        Product productToUpdate = await this.productRepository.GetByIdAsync(product.Id);

        if (productToUpdate == null)
        {
            output.Errors.Add("Product with selected key not found");
        }

        if (output.Errors.Count == 0)
        {
            this.mapper.Map<UpdateProductInput, Product>(product, productToUpdate);

            await this.productRepository
                .UpdateAsync(productToUpdate);
        }

        return output;
    }

    public async Task<DeleteProductOutput> DeleteProductAsync(Guid id)
    {
        DeleteProductOutput output = new()
        {
            Errors = new(),
        };

        Product productExists = await this.productRepository
            .GetByIdAsync(id);

        if (productExists == null)
        {
            output.Errors.Add("Product with selected key not found");
        }

        if (output.Errors.Count == 0)
        {
            await this.productRepository
                .DeleteAsync(id);
        }

        return output;
    }

    private static List<string> ElectricalProductValidations(List<UniquePropertyDto> uniqueProperties)
    {
        List<string> errors = new();

        UniquePropertyDto? voltage = uniqueProperties.FirstOrDefault(up => up.Name.ToLower() == "voltage");
        if (voltage == null)
        {
            errors.Add("Electric product must have voltage");
        }

        UniquePropertyDto? socketType = uniqueProperties.FirstOrDefault(up => up.Name.ToLower() == "sockettype");
        if (socketType == null)
        {
            errors.Add("Electric product must have socket type");
        }

        // Checking that socket type option has valid value
        // UK/EU socket has 220v
        // US socket has 110v
        Dictionary<string, string> validSocketTypeToVoltage = new()
            {
                { "UK",  "220v" },
                { "EU",  "220v" },
                { "US",  "110v" },
            };

        if (!validSocketTypeToVoltage.ContainsKey(socketType.Value))
        {
            errors.Add("Invalid socket type option");
        }
        else
        {
            if (validSocketTypeToVoltage[socketType.Value] != voltage.Value)
            {
                errors.Add($"Socket of type {socketType.Value} must have voltage of {validSocketTypeToVoltage[socketType.Value]}");
            }
        }

        return errors;
    }

    private static List<string> FreshProductValidations(List<UniquePropertyDto> uniqueProperties)
    {
        List<string> errors = new();

        if (!uniqueProperties.Any(up => up.Name == "ExpiryDate"))
        {
            errors.Add("Fresh product must have expiry date");
        }

        return errors;
    }
}