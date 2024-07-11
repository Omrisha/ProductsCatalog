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

    public async Task<CreateCatalogOutput> CreateCatalogAsync(CreateCatalogInput Catalog)
    {
        CreateCatalogOutput output = new()
        {
            Errors = new(),
        };

        // Checking if Catalog is a Fresh Catalog that it has an Expiry date
        // or if Catalog is a Electric Catalog that it has both voltage and socket type
        output.Errors = Catalog.Category switch
        {
            CategoryEnum.FreshCatalog => FreshCatalogValidations(Catalog.UniqueProperties),
            CategoryEnum.ElectricCatalog => ElectricalCatalogValidations(Catalog.UniqueProperties),
            _ => new List<string> { "Invalid Catalog category" },
        };

        if (output.Errors.Count == 0)
        {
            Catalog newCatalog = this.mapper.Map<CreateCatalogInput, Catalog>(Catalog);

            Catalog created = await this.catalogRepository
                .CreateAsync(newCatalog);

            output.CatalogId = created.Id;
        }

        return output;
    }


    public async Task<UpdateCatalogOutput> UpdateCatalogAsync(UpdateCatalogInput Catalog)
    {
        UpdateCatalogOutput output = new()
        {
            Errors = new(),
        };

        // Checking if Catalog is a Fresh Catalog that it has an Expiry date
        // or if Catalog is a Electric Catalog that it has both voltage and socket type
        output.Errors = Catalog.Category switch
        {
            CategoryEnum.FreshCatalog => FreshCatalogValidations(Catalog.UniqueProperties),
            CategoryEnum.ElectricCatalog => ElectricalCatalogValidations(Catalog.UniqueProperties),
            _ => new List<string> { "Invalid Catalog category" },
        };

        Catalog CatalogToUpdate = await this.catalogRepository.GetByIdAsync(Catalog.Id);

        if (CatalogToUpdate == null)
        {
            output.Errors.Add("Catalog with selected key not found");
        }

        this.mapper.Map<UpdateCatalogInput, Catalog>(Catalog, CatalogToUpdate);

        await this.catalogRepository
            .UpdateAsync(CatalogToUpdate);

        return output;
    }

    public async Task<DeleteCatalogOutput> DeleteCatalogAsync(Guid id)
    {
        DeleteCatalogOutput output = new()
        {
            Errors = new(),
        };

        Catalog CatalogExists = await this.catalogRepository
            .GetByIdAsync(id);

        if (CatalogExists == null)
        {
            output.Errors.Add("Catalog with selected key not found");
        }

        await this.catalogRepository
            .DeleteAsync(id);

        return output;
    }
}