namespace DomainModel;

public class UpdateCatalogInput
{
    public Guid Id { get; set; }

    public string Title { get; set; }

    public List<Guid> Products { get; set; }
}
