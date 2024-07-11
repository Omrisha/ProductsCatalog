namespace DomainModel;

public class CreateNewProductOutput
{
    public List<string> Errors { get; set; }
    
    public Guid? ProductId { get; set; }
}