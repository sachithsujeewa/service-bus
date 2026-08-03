using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using PartnerApp;
using ServiceBus.Contracts;

var builder = WebApplication.CreateBuilder(args);

var busUrl = builder.Configuration["ServiceBus:BaseUrl"] ?? "http://service-bus-api:8080";
var partnerKey = builder.Configuration["ServiceBus:PartnerApiKey"] ?? "partner-dev-key";
var webhookPublicUrl = builder.Configuration["Partner:PublicWebhookUrl"] ?? "http://partner-app:5102/webhook";
var hmacSecret = builder.Configuration["Partner:HmacSecret"] ?? "demo-hmac-secret";
var demoEventType = "OrderCreated";

var received = new List<object>();
var jsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);

// Register webhook on startup only if not already registered (avoid duplicates on restart)
using (var client = new HttpClient { Timeout = TimeSpan.FromSeconds(10) })
{
    client.DefaultRequestHeaders.Add("X-Api-Key", partnerKey);

    for (var attempt = 1; attempt <= 30; attempt++)
    {
        try
        {
            var listResponse = await client.GetAsync($"{busUrl}/api/webhooks");
            if (listResponse.IsSuccessStatusCode)
            {
                var existing = await listResponse.Content.ReadFromJsonAsync<List<WebhookRegistration>>(jsonOptions);
                var alreadyRegistered = existing?.Any(w =>
                    w.Active
                    && string.Equals(w.TargetUrl, webhookPublicUrl, StringComparison.OrdinalIgnoreCase)
                    && string.Equals(w.EventType, demoEventType, StringComparison.OrdinalIgnoreCase)) == true;

                if (alreadyRegistered)
                {
                    Console.WriteLine("Webhook already registered — skipping duplicate registration");
                    break;
                }
            }

            var reg = new CreateWebhookRequest
            {
                TargetUrl = webhookPublicUrl,
                EventType = demoEventType,
                Database = "ALL",
                HmacSecret = hmacSecret
            };
            var regResponse = await client.PostAsJsonAsync($"{busUrl}/api/webhooks", reg);
            var regBody = await regResponse.Content.ReadAsStringAsync();
            Console.WriteLine($"Webhook registration: {regResponse.StatusCode} {regBody}");
            if (regResponse.IsSuccessStatusCode)
                break;
        }
        catch (Exception ex) when (attempt < 30)
        {
            Console.WriteLine($"Webhook registration attempt {attempt} failed: {ex.Message}");
            await Task.Delay(TimeSpan.FromSeconds(2));
        }
    }
}

var app = builder.Build();

app.MapGet("/", () => Results.Content(DemoUi.Page, "text/html"));

app.MapGet("/health", () => Results.Ok(new { status = "healthy", service = "partner-app", receivedCount = received.Count }));

app.MapGet("/received", () => Results.Ok(received));

app.MapPost("/webhook", async (HttpRequest request) =>
{
    using var reader = new StreamReader(request.Body);
    var body = await reader.ReadToEndAsync();

    if (request.Headers.TryGetValue("X-Rambase-Signature", out var sigHeader))
    {
        var expected = ComputeHmac(body, hmacSecret);
        if (!string.Equals(sigHeader.ToString(), expected, StringComparison.OrdinalIgnoreCase))
            return Results.Unauthorized();
    }

    var eventId = request.Headers["X-Rambase-EventID"].ToString();
    var entry = new { eventId, body, at = DateTime.UtcNow };
    received.Add(entry);
    Console.WriteLine($"Webhook received: {eventId}");
    return Results.Ok(new { ok = true });
});

app.Run();

static string ComputeHmac(string body, string secret)
{
    using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret));
    return Convert.ToHexString(hmac.ComputeHash(Encoding.UTF8.GetBytes(body))).ToLowerInvariant();
}
