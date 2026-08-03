using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using ServiceBus.Api;
using ServiceBus.Contracts;
using ServiceBus.Messaging;

var builder = WebApplication.CreateBuilder(args);

var postgresConnection = builder.Configuration.GetConnectionString("Postgres")
    ?? "Host=postgres;Port=5432;Database=rambase_bus;Username=rambase;Password=rambase";

var rabbitOptions = new RabbitMqOptions
{
    Host = builder.Configuration["RabbitMq:Host"] ?? "rabbitmq",
    Port = int.Parse(builder.Configuration["RabbitMq:Port"] ?? "5672"),
    User = builder.Configuration["RabbitMq:User"] ?? "rambase",
    Password = builder.Configuration["RabbitMq:Password"] ?? "rambase",
    VHost = builder.Configuration["RabbitMq:VHost"] ?? "rambase"
};

var authOptions = new ApiKeyAuthOptions
{
    Keys = new Dictionary<string, ApiKeyIdentity>
    {
        [builder.Configuration["Auth:SourceApiKey"] ?? "source-dev-key"] = new() { Role = ApiRoles.Source },
        [builder.Configuration["Auth:PartnerApiKey"] ?? "partner-dev-key"] = new() { Role = ApiRoles.Partner, PartnerId = "demo-partner" },
        [builder.Configuration["Auth:AdminApiKey"] ?? "admin-dev-key"] = new() { Role = ApiRoles.Admin }
    }
};

builder.Services.AddSingleton(new DatabaseService(postgresConnection));
builder.Services.AddSingleton(rabbitOptions);
builder.Services.AddSingleton(authOptions);

builder.Services.AddHealthChecks()
    .AddNpgSql(postgresConnection, name: "postgres");

var app = builder.Build();

var db = app.Services.GetRequiredService<DatabaseService>();
await db.InitializeAsync();

EventPublisher publisher = await EventPublisher.CreateAsync(rabbitOptions);
app.Lifetime.ApplicationStopping.Register(() => publisher.DisposeAsync().AsTask());

app.UseApiKeyAuthentication(authOptions);

app.MapGet("/", () => Results.Content(DemoUi.HubPage, "text/html"));

app.MapGet("/health", () => Results.Ok(new { status = "healthy", service = "service-bus-api" }));

app.MapHealthChecks("/ready", new HealthCheckOptions
{
    Predicate = _ => true,
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        var status = report.Status == HealthStatus.Healthy ? "ready" : "not_ready";
        await context.Response.WriteAsJsonAsync(new { status, entries = report.Entries.Keys });
    }
});

app.MapPost("/api/events", async (PublishEventRequest request, HttpContext ctx, DatabaseService database) =>
{
    var identity = ApiKeyAuthentication.GetIdentity(ctx);
    if (!ApiKeyAuthentication.HasRole(identity, ApiRoles.Source) && !ApiKeyAuthentication.HasRole(identity, ApiRoles.Admin))
        return Results.Forbid();

    var eventId = await database.NextEventIdAsync(request.SystemId);
    var envelope = new EventEnvelope
    {
        SystemId = request.SystemId,
        EventType = request.EventType,
        Database = request.Database,
        RamBaseEventId = eventId,
        RegisterTime = DateTime.UtcNow,
        ObjectDetailsUri = request.ObjectDetailsUri,
        Parameters = request.Parameters ?? new Dictionary<string, string>()
    };

    await publisher.PublishEventAsync(envelope);
    return Results.Accepted($"/api/events/{envelope.MessageId}", envelope);
});

app.MapPost("/api/webhooks", async (CreateWebhookRequest request, HttpContext ctx, DatabaseService database) =>
{
    var identity = ApiKeyAuthentication.GetIdentity(ctx);
    if (!ApiKeyAuthentication.HasRole(identity, ApiRoles.Partner) && !ApiKeyAuthentication.HasRole(identity, ApiRoles.Admin))
        return Results.Forbid();

    var partnerId = identity.PartnerId ?? "demo-partner";
    var (webhook, created) = await database.CreateWebhookAsync(partnerId, request);

    if (created)
    {
        await publisher.PublishManagementAsync(new ManagementMessage
        {
            Action = ManagementAction.WebHookCreated,
            WebhookId = webhook.Id,
            PartnerId = partnerId
        });
        return Results.Created($"/api/webhooks/{webhook.Id}", webhook);
    }

    return Results.Ok(webhook);
});

app.MapGet("/api/webhooks", async (HttpContext ctx, DatabaseService database) =>
{
    var identity = ApiKeyAuthentication.GetIdentity(ctx);
    if (ApiKeyAuthentication.HasRole(identity, ApiRoles.Admin))
        return Results.Ok(await database.GetWebhooksAsync());
    if (ApiKeyAuthentication.HasRole(identity, ApiRoles.Partner))
        return Results.Ok(await database.GetWebhooksAsync(identity.PartnerId));
    return Results.Forbid();
});

app.MapDelete("/api/webhooks/{id:guid}", async (Guid id, HttpContext ctx, DatabaseService database) =>
{
    var identity = ApiKeyAuthentication.GetIdentity(ctx);
    if (!ApiKeyAuthentication.HasRole(identity, ApiRoles.Partner) && !ApiKeyAuthentication.HasRole(identity, ApiRoles.Admin))
        return Results.Forbid();

    var partnerId = identity.PartnerId ?? "demo-partner";
    var ok = await database.DeleteWebhookAsync(id, partnerId);
    if (!ok)
        return Results.NotFound();

    await publisher.PublishManagementAsync(new ManagementMessage
    {
        Action = ManagementAction.WebHookDeleted,
        WebhookId = id,
        PartnerId = partnerId
    });

    return Results.NoContent();
});

app.MapGet("/api/dlq", async (HttpContext ctx, DatabaseService database) =>
{
    var identity = ApiKeyAuthentication.GetIdentity(ctx);
    if (!ApiKeyAuthentication.HasRole(identity, ApiRoles.Admin))
        return Results.Forbid();
    return Results.Ok(await database.GetDlqAsync());
});

app.Run();
