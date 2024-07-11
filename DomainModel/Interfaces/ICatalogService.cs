namespace DomainModel
{
    public interface ICatalogService
    {
        Task<CreateCatalogOutput> CreateCatalogAsync(CreateCatalogInput catalog);
        Task<DeleteCatalogOutput> DeleteCatalogAsync(Guid id);
        Task<CatalogDto> GetCatalogByIdAsync(Guid id);
        Task<IEnumerable<CatalogDto>> GetCatalogsAsync();
        Task<CatalogDto> GetCatalogByProductIdAsync(Guid productId);
        Task<UpdateCatalogOutput> UpdateCatalogAsync(UpdateCatalogInput catalog);
    }
}