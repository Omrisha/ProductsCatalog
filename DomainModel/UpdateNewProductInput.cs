namespace DomainModel;

public class UpdateNewProductInput
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public decimal Price { get; set; }
    public CategoryEnum Category { get; set; }
    public string Description { get; set; }
    public List<UniquePropertyDto> UniqueProperties { get; set; }
}