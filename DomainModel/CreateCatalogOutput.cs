namespace DomainModel;

public class CreateCatalogOutput
{
    public List<string> Errors { get; set; }

    public Guid? Id { get; set; }
}