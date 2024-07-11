using AutoMapper;
using EntityModel;

namespace DomainModel.Impl.MappingProfile;

public class DomainModelMappingProfile : Profile
{
    public DomainModelMappingProfile()
    {
        this.CreateMap<CreateProductInput, Product>();
        this.CreateMap<UpdateProductInput, Product>();
        this.CreateMap<Product, ProductDto>();
        this.CreateMap<UniqueProperty, UniquePropertyDto>();
        this.CreateMap<UniquePropertyDto, UniqueProperty>();
        
        this.CreateMap<Catalog, CatalogDto>()
            .ForMember(d => d.Products, m => m.Ignore());
        
        this.CreateMap<CreateCatalogInput, Catalog>();
        this.CreateMap<UpdateCatalogInput, Catalog>();
    }
}