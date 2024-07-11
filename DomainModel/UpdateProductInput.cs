namespace DomainModel;

public class UpdateProductInput
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public decimal Price { get; set; }
    public CategoryEnum Category { get; set; }
    public string Description { get; set; }
    public List<UniquePropertyDto> UniqueProperties { get; set; }
}