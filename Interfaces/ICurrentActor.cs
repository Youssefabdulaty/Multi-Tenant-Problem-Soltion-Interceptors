using System.Diagnostics;
using static System.Net.WebRequestMethods;

namespace MinimalAPI.Interfaces
{
    public interface ICurrentActor
    {
        string? UserId { get; }
        Guid? TenantId { get; }
        string CorrelationId { get; }
    }

    public sealed class CurrentActor : ICurrentActor
    {
        public CurrentActor(IHttpContextAccessor accessor)
        {

            var httpContext = accessor.HttpContext;
            var userId = httpContext?.User?.Identity.Name;
            UserId = userId;
            TenantId = Guid.TryParse(httpContext?.Request.Headers["X-Tenant-Id"], out var tid) ? tid : null;

            CorrelationId =
                httpContext?.TraceIdentifier
                ?? Activity.Current?.Id
                ?? Guid.NewGuid().ToString("N");
        }
        public string? UserId { get; }
        public Guid? TenantId { get; }
        public string CorrelationId { get; }
    }
}
