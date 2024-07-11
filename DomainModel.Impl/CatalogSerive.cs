using AutoMapper;
using EntityModel;
using Microsoft.Extensions.Logging;
using PersistanceModel.MongoDB;

namespace DomainModel.Impl;

public class CatalogService : ICatalogService
{
    private readonly ICatalogRepository catalogRepository;
    private readonly IProductRepository productRepository;
    private readonly IMapper mapper;
    private readonly ILogger<CatalogService> logger;

    public CatalogService(
        ICatalogRepository catalogRepository,
        IProductRepository productRepository,
        IMapper mapper,
        ILogger<CatalogService> logger)
    {
        this.catalogRepository = catalogRepository;
        this.productRepository = productRepository;
        this.mapper = mapper;
        this.logger = logger;
    }

    public async Task<IEnumerable<CatalogDto>> GetCatalogsAsync()
    {
        IEnumerable<Catalog> catalogs = await this.catalogRepository
            .GetAllAsync();

        Dictionary<Guid, List<Guid>> catalogToProduct = catalogs.ToDictionary(c => c.Id, c => c.Products);

        IEnumerable<CatalogDto> catalogDtos = this.mapper.Map<IEnumerable<Catalog>, IEnumerable<CatalogDto>>(catalogs);

        foreach (CatalogDto catalogDto in catalogDtos)
        {
            HashSet<Guid> productIds = catalogToProduct[catalogDto.Id]
                .Distinct()
                .ToHashSet();

            IEnumerable<Product> products = (await this.productRepository
                .GetAllAsync())
                .Where(p => productIds.Contains(p.Id));

            catalogDto.Products = this.mapper.Map<IEnumerable<Product>, IEnumerable<ProductDto>>(products)
                .ToList();
        }

        return catalogDtos;
    }

    public async Task<CatalogDto> GetCatalogByIdAsync(Guid id)
    {
        Catalog catalog = await this.catalogRepository
            .GetByIdAsync(id) ?? throw new KeyNotFoundException("Catalog with selected key not found");

        CatalogDto catalogDto = this.mapper.Map<Catalog, CatalogDto>(catalog);

        HashSet<Guid> productIds = catalog.Products
                .Distinct()
                .ToHashSet();

        IEnumerable<Product> products = (await this.productRepository
            .GetAllAsync())
            .Where(p => productIds.Contains(p.Id));

        catalogDto.Products = this.mapper.Map<IEnumerable<Product>, IEnumerable<ProductDto>>(products)
            .ToList();

        return catalogDto;
    }

    public async Task<CatalogDto> GetCatalogByProductIdAsync(Guid productId)
    {
        Catalog catalog = await this.catalogRepository
            .GetCatalogByProductIdAsync(productId) ?? throw new KeyNotFoundException("Catalog with selected key not found");

        CatalogDto catalogDto = this.mapper.Map<Catalog, CatalogDto>(catalog);

        HashSet<Guid> productIds = catalog.Products
                .Distinct()
                .ToHashSet();

        IEnumerable<Product> products = (await this.productRepository
            .GetAllAsync())
            .Where(p => productIds.Contains(p.Id));

        catalogDto.Products = this.mapper.Map<IEnumerable<Product>, IEnumerable<ProductDto>>(products)
            .ToList();

        return catalogDto;
    }

    public async Task<CreateCatalogOutput> CreateCatalogAsync(CreateCatalogInput catalog)
    {
        CreateCatalogOutput output = new()
        {
            Errors = new(),
        };
        
        // If one of the product is a fresh one, check it expiry date is at 
        // least 7 days from now
        output.Errors = await FreshProductValidations(catalog.Products);

        if (output.Errors.Count == 0)
        {
            Catalog newCatalog = this.mapper.Map<CreateCatalogInput, Catalog>(catalog);
            
            Catalog created = await this.catalogRepository
                .CreateAsync(newCatalog);

            output.Id = created.Id;
        }

        return output;
    }


    public async Task<UpdateCatalogOutput> UpdateCatalogAsync(UpdateCatalogInput catalog)
    {
        UpdateCatalogOutput output = new()
        {
            Errors = new(),
        };

        // If one of the product is a fresh one, check it expiry date is at 
        // least 7 days from now
        output.Errors = await FreshProductValidations(catalog.Products);

        Catalog catalogToUpdate = await this.catalogRepository.GetByIdAsync(catalog.Id);
        
        if (catalogToUpdate == null)
        {
            output.Errors.Add("Catalog with selected key not found");
        }
        
        if (catalogToUpdate.Products.Concat(catalog.Products).Distinct().Count() != catalog.Products.Count)
        {
            output.Errors.Add("Catalog can't contain duplicate products");
        }
        
        if (output.Errors.Count == 0)
        {
            this.mapper.Map<UpdateCatalogInput, Catalog>(catalog, catalogToUpdate);

            await this.catalogRepository
                .UpdateAsync(catalogToUpdate);
        }

        return output;
    }

    public async Task<DeleteCatalogOutput> DeleteCatalogAsync(Guid id)
    {
        DeleteCatalogOutput output = new()
        {
            Errors = new(),
        };

        Catalog catalogExists = await this.catalogRepository
            .GetByIdAsync(id);

        if (catalogExists == null)
        {
            output.Errors.Add("Catalog with selected key not found");
        }

        await this.catalogRepository
            .DeleteAsync(id);

        return output;
    }
    
    private async Task<List<string>> FreshProductValidations(List<Guid> productIds)
    {
        List<string> errors = new();
        IEnumerable<Product> products = await this.productRepository
            .GetByFilterAsync(p => productIds.Contains(p.Id));

        foreach (Product product in products)
        {
            if (product.Category == EntityModel.CategoryEnum.FreshProduct)
            {
                UniqueProperty? expiryDate = product.UniqueProperties
                    .FirstOrDefault(p => p.Name == "ExpiryDate");
                if (expiryDate != null)
                {
                    if (DateTime.TryParse(expiryDate?.Value.ToString(), out DateTime expiryDateValue) && 
                        expiryDateValue < DateTime.UtcNow.AddDays(7))
                    {
                        errors.Add("Fresh products must have an expiry date at least 7 days from now");
                    }
                }
            }
        }
        
        return errors;
    }
}