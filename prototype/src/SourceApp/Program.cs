using Microsoft.AspNetCore.Mvc;
using SourceApp;

var builder = WebApplication.CreateBuilder(args);

var busUrl = builder.Configuration["ServiceBus:BaseUrl"] ?? "http://service-bus-api:8080";
var apiKey = builder.Configuration["ServiceBus:SourceApiKey"] ?? "source-dev-key";

var app = builder.Build();

app.MapGet("/", () => Results.Content(DemoUi.Page, "text/html"));

app.MapGet("/health", () => Results.Ok(new { status = "healthy", service = "source-app" }));

static Dictionary<string, string> BuildParameters(EmitRequest? body)
{
    if (body?.Parameters is { Count: > 0 })
        return body.Parameters;

    return new Dictionary<string, string> { ["orderId"] = Guid.NewGuid().ToString("N")[..8] };
}

async Task<IResult> EmitAsync(string eventType, EmitRequest? body, string busUrl, string apiKey)
{
    using var client = new HttpClient { Timeout = TimeSpan.FromSeconds(30) };
    client.DefaultRequestHeaders.Add("X-Api-Key", apiKey);
    var payload = new
    {
        systemId = body?.SystemId ?? "DEMO",
        eventType,
        database = body?.Database ?? "ALL",
        parameters = BuildParameters(body),
        objectDetailsUri = body?.ObjectDetailsUri
    };

    try
    {
        var response = await client.PostAsJsonAsync($"{busUrl}/api/events", payload);
        var content = await response.Content.ReadAsStringAsync();
        return Results.Json(
            new { status = response.StatusCode.ToString(), body = content },
            statusCode: (int)response.StatusCode);
    }
    catch (Exception ex)
    {
        return Results.Json(
            new { status = "Error", body = ex.Message },
            statusCode: StatusCodes.Status502BadGateway);
    }
}

// GET fallback — one-click demo without JSON body (browser link / bookmark)
app.MapGet("/emit/{eventType}", (string eventType) => EmitAsync(eventType, null, busUrl, apiKey));

app.MapPost("/emit/{eventType}", async (string eventType, [FromBody] EmitRequest? body) =>
    await EmitAsync(eventType, body, busUrl, apiKey));

app.Run();

public sealed class EmitRequest
{
    public string? SystemId { get; set; }
    public string? Database { get; set; }
    public Dictionary<string, string>? Parameters { get; set; }
    public string? ObjectDetailsUri { get; set; }
}
