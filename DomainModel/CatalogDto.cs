namespace DomainModel;

public class CatalogDto
{
    public Guid Id { get; set; }

    public string Title { get; set; }

    public List<ProductDto> Products { get; set; }
}