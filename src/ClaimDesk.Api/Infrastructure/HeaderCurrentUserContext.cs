using ClaimDesk.Application.Abstractions;

namespace ClaimDesk.Api.Infrastructure;

public class HeaderCurrentUserContext : ICurrentUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HeaderCurrentUserContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int UserId
    {
        get
        {
            if (TryGetUserId(out var userId))
            {
                return userId;
            }

            return 0;
        }
    }

    public string Role => _httpContextAccessor.HttpContext?.Request.Headers["X-User-Role"].FirstOrDefault() ?? "Anonymous";

    public bool TryGetUserId(out int userId)
    {
        var rawValue = _httpContextAccessor.HttpContext?.Request.Headers["X-User-Id"].FirstOrDefault();
        return int.TryParse(rawValue, out userId) && userId > 0;
    }

    public bool IsInRole(string role)
    {
        return string.Equals(Role, role, StringComparison.OrdinalIgnoreCase);
    }
}
