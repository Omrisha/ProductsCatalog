using Infrastructure.EntityModel;

namespace EntityModel;

public class Product : BaseEntity
{
    public string Title { get; set; }

    public string Description { get; set; }

    public decimal Price { get; set; }

    public CategoryEnum Category { get; set; }

    public bool IsActive { get; set; }

    public List<UniqueProperty> UniqueProperties { get; set; }
}