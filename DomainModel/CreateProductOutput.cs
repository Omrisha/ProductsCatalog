namespace DomainModel;

public class CreateProductOutput
{
    public List<string> Errors { get; set; }
    
    public Guid? ProductId { get; set; }
}