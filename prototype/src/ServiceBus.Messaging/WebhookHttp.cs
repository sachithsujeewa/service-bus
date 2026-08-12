using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace ServiceBus.Messaging;

/// <summary>
/// Dev helpers for delivering webhooks to endpoints hosted on the Docker host
/// (e.g. IIS Express / Visual Studio on Windows).
/// </summary>
public static class WebhookHttp
{
    /// <summary>
    /// HttpClient for outbound webhooks:
    /// - trusts HTTPS certificates for localhost / host.docker.internal (VS / IIS Express);
    /// - does not follow redirects (IIS often 302 → https://localhost:… which is unreachable from Docker).
    /// </summary>
    public static HttpClient CreateClient(TimeSpan? timeout = null)
    {
        var handler = new HttpClientHandler
        {
            AllowAutoRedirect = false,
            ServerCertificateCustomValidationCallback = TrustLocalDevCertificate
        };
        return new HttpClient(handler) { Timeout = timeout ?? TimeSpan.FromSeconds(30) };
    }

    /// <summary>
    /// When the target uses <c>host.docker.internal</c> (any port), rewrite the HTTP
    /// <c>Host</c> header to <c>localhost[:port]</c> so IIS Express accepts the request.
    /// Routing still uses the original URL (Docker → host).
    /// </summary>
    public static void ApplyDevHostHeader(HttpRequestMessage request, string targetUrl)
    {
        if (!Uri.TryCreate(targetUrl, UriKind.Absolute, out var uri))
            return;

        if (!string.Equals(uri.Host, "host.docker.internal", StringComparison.OrdinalIgnoreCase))
            return;

        request.Headers.Host = uri.IsDefaultPort ? "localhost" : $"localhost:{uri.Port}";
    }

    private static bool TrustLocalDevCertificate(
        HttpRequestMessage request,
        X509Certificate2? _,
        X509Chain? __,
        SslPolicyErrors errors)
    {
        var host = request.RequestUri?.Host;
        if (IsLocalDevHost(host))
            return true;

        return errors == SslPolicyErrors.None;
    }

    private static bool IsLocalDevHost(string? host) =>
        string.Equals(host, "host.docker.internal", StringComparison.OrdinalIgnoreCase)
        || string.Equals(host, "localhost", StringComparison.OrdinalIgnoreCase)
        || host is "127.0.0.1" or "::1";
}
