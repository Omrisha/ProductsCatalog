namespace DomainModel;

public class CreateCatalogInput
{
    public string Title { get; set; }

    public List<Guid> Products { get; set; }
}
