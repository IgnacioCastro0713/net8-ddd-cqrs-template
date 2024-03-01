using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Providers;

public sealed class UserContextProvider(IHttpContextAccessor httpContextAccessor) : IUserContextProvider
{
    public string? NtUser => httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Name);

    public int? Id => int.TryParse(httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier),
        out var value)
        ? value
        : null;
}