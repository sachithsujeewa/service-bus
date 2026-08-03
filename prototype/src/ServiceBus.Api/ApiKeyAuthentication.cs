using Microsoft.AspNetCore.Http;
using ServiceBus.Contracts;

namespace ServiceBus.Api;

public sealed class ApiKeyAuthOptions
{
    public Dictionary<string, ApiKeyIdentity> Keys { get; set; } = new();
}

public static class ApiKeyAuthentication
{
    public const string HeaderName = "X-Api-Key";

    public static IApplicationBuilder UseApiKeyAuthentication(this IApplicationBuilder app, ApiKeyAuthOptions options)
    {
        return app.Use(async (context, next) =>
        {
            if (context.Request.Path.StartsWithSegments("/health")
                || context.Request.Path.StartsWithSegments("/ready")
                || context.Request.Path == "/")
            {
                await next();
                return;
            }

            if (!context.Request.Headers.TryGetValue(HeaderName, out var key) || string.IsNullOrWhiteSpace(key))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Missing API key");
                return;
            }

            if (!options.Keys.TryGetValue(key!, out var identity))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Invalid API key");
                return;
            }

            context.Items["ApiIdentity"] = identity;
            await next();
        });
    }

    public static ApiKeyIdentity GetIdentity(HttpContext context)
        => (ApiKeyIdentity)context.Items["ApiIdentity"]!;

    public static bool HasRole(ApiKeyIdentity identity, string role)
        => string.Equals(identity.Role, role, StringComparison.OrdinalIgnoreCase);
}
