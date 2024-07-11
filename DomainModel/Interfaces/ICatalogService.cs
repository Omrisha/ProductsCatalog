namespace DomainModel
{
    public interface ICatalogService
    {
        Task<CreateCatalogOutput> CreateCatalogAsync(CreateCatalogInput Catalog);
        Task<DeleteCatalogOutput> DeleteCatalogAsync(Guid id);
        Task<CatalogDto> GetCatalogByIdAsync(Guid id);
        Task<IEnumerable<CatalogDto>> GetCatalogsAsync();
        Task<IEnumerable<CatalogDto>> GetCatalogsByProductIdAsync(Guid productId);
        Task<UpdateCatalogOutput> UpdateCatalogAsync(UpdateCatalogInput Catalog);
    }
}