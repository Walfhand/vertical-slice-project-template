using DomainEssentials.Core.Implementations;
using DomainEssentials.Core.Keys;

namespace Api.Domain.Orders.AggregateRoots;

public record OrderId : IdBase
{
    public OrderId(Guid id) : base(id)
    {
    }

    public OrderId()
    {
    }
}

public class Order : AggregateRoot<OrderId>
{
    
}