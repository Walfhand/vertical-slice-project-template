using Engine.Wolverine;
using Engine.Wolverine.Factory;
using Microsoft.AspNetCore.Mvc;
using QuickApi.Engine.Web.Endpoints.Impl;

namespace Api.Features.Orders.CreateOrderRepr;

public sealed record Request
{
    [FromBody] public RequestBody Body { get; set; } = null!;

    public sealed record RequestBody(string CustomerId, decimal Total);
}

public sealed record Response(Guid OrderId, string Status, DateTimeOffset CreatedAt);

public class CreateOrder
{
    public sealed class CreateOrderEndpoint()
        : PostMinimalEndpoint<Request, Response>("orders");
}

public sealed class CreateRequestHandler(
    IAppDbContextFactory dbContextFactory,
    IHttpContextAccessor contextAccessor)
    : Handler(dbContextFactory, contextAccessor)
{
    public Response Handle(Request request)
    {
        return new Response(
            Guid.NewGuid(),
            "Created",
            DateTimeOffset.UtcNow);
    }
}