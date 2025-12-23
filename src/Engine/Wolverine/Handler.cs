using System.Security.Claims;
using Engine.EFCore;
using Engine.Wolverine.Factory;
using Microsoft.AspNetCore.Http;

namespace Engine.Wolverine;

public abstract class Handler
{
    protected readonly IDbContext DbContext;

    protected Handler(IAppDbContextFactory dbContextFactory, IHttpContextAccessor contextAccessor)
    {
        DbContext = dbContextFactory.CreateDbContext();
        UserId = contextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Name);
    }

    public string? UserId { get; set; }
}