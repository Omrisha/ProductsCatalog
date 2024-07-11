using Infrastructure.EntityModel;

namespace EntityModel;

public class Catalog : BaseEntity
{
    public string Title { get; set; }

    public List<Guid> Products { get; set; }
}