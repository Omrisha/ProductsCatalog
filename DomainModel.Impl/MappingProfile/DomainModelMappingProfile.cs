using AutoMapper;
using EntityModel;

namespace DomainModel.Impl.MappingProfile;

public class DomainModelMappingProfile : Profile
{
    public DomainModelMappingProfile()
    {
        this.CreateMap<CreateNewProductInput, Product>();
        this.CreateMap<UpdateNewProductInput, Product>();
        this.CreateMap<Product, ProductDto>();

        this.CreateMap<Product, ElectricProductDto>()
            .ForMember(d => ((ElectricProductDto)d).ExpiryDate,
                m => m.MapFrom(s => s.UniqueProperties.FirstOrDefault(u => u.Name == "ExpiryDate").Value));
        
        this.CreateMap<Product, FreshProductDto>()
            .ForMember(d => ((FreshProductDto)d).SocketType,
                m => m.MapFrom(s => s.UniqueProperties.FirstOrDefault(u => u.Name == "SocketType").Value))
            .ForMember(d => ((FreshProductDto)d).Voltage,
                m => m.MapFrom(s => s.UniqueProperties.FirstOrDefault(u => u.Name == "Voltage").Value));
        this.CreateMap<UniquePropertyDto, UniqueProperty>();
    }
}