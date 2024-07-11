using Infrastructure.EntityModel.Interfaces;

namespace Infrastructure.EntityModel;

public abstract class BaseEntity : IEntity
{
    public Guid Id { get; set; }
}