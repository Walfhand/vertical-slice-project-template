using Api.Domain.Orders.AggregateRoots;
using Engine.EFCore;

namespace Api.Infrastructure.Persistence.Configs.Etcs;

public class OrderEtc : BaseEtc<Order, OrderId>
{
    protected override OrderId KeyCreator(Guid id) => new(id);
}