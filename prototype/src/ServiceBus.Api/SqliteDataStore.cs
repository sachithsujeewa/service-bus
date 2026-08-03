using Microsoft.Data.Sqlite;
using ServiceBus.Contracts;

namespace ServiceBus.Api;

public sealed class SqliteDataStore : IDataStore
{
    private readonly string _connectionString;

    public SqliteDataStore(string path)
    {
        var fullPath = Path.GetFullPath(path);
        var dir = Path.GetDirectoryName(fullPath);
        if (!string.IsNullOrEmpty(dir))
            Directory.CreateDirectory(dir);
        _connectionString = $"Data Source={fullPath}";
    }

    public async Task InitializeAsync()
    {
        await using var conn = new SqliteConnection(_connectionString);
        await conn.OpenAsync();
        await using var cmd = conn.CreateCommand();
        cmd.CommandText = """
            CREATE TABLE IF NOT EXISTS webhooks (
                id TEXT PRIMARY KEY,
                partner_id TEXT NOT NULL,
                target_url TEXT NOT NULL,
                event_type TEXT NOT NULL,
                database TEXT NOT NULL DEFAULT 'ALL',
                hmac_secret TEXT NULL,
                active INTEGER NOT NULL DEFAULT 1,
                created_at TEXT NOT NULL
            );
            CREATE TABLE IF NOT EXISTS publish_cursor (
                system_id TEXT PRIMARY KEY,
                last_event_id INTEGER NOT NULL DEFAULT 0
            );
            CREATE TABLE IF NOT EXISTS dlq_entries (
                id TEXT PRIMARY KEY,
                message_id TEXT,
                payload TEXT NOT NULL,
                error TEXT NOT NULL,
                created_at TEXT NOT NULL
            );
            """;
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task<long> NextEventIdAsync(string systemId)
    {
        await using var conn = new SqliteConnection(_connectionString);
        await conn.OpenAsync();
        await using var cmd = conn.CreateCommand();
        cmd.CommandText = """
            INSERT INTO publish_cursor (system_id, last_event_id) VALUES ($id, 1)
            ON CONFLICT(system_id) DO UPDATE SET last_event_id = last_event_id + 1;
            SELECT last_event_id FROM publish_cursor WHERE system_id = $id;
            """;
        cmd.Parameters.AddWithValue("$id", systemId);
        return (long)(await cmd.ExecuteScalarAsync() ?? 1L);
    }

    public async Task<(WebhookRegistration Webhook, bool Created)> CreateWebhookAsync(string partnerId, CreateWebhookRequest request)
    {
        var existing = await FindActiveWebhookAsync(partnerId, request.TargetUrl, request.EventType, request.Database);
        if (existing is not null)
            return (existing, false);

        var webhook = new WebhookRegistration
        {
            Id = Guid.NewGuid(),
            PartnerId = partnerId,
            TargetUrl = request.TargetUrl,
            EventType = request.EventType,
            Database = request.Database,
            HmacSecret = request.HmacSecret,
            Active = true,
            CreatedAt = DateTime.UtcNow
        };

        await using var conn = new SqliteConnection(_connectionString);
        await conn.OpenAsync();
        await using var cmd = conn.CreateCommand();
        cmd.CommandText = """
            INSERT INTO webhooks (id, partner_id, target_url, event_type, database, hmac_secret, active, created_at)
            VALUES ($id, $pid, $url, $etype, $db, $secret, 1, $created);
            """;
        cmd.Parameters.AddWithValue("$id", webhook.Id.ToString());
        cmd.Parameters.AddWithValue("$pid", webhook.PartnerId);
        cmd.Parameters.AddWithValue("$url", webhook.TargetUrl);
        cmd.Parameters.AddWithValue("$etype", webhook.EventType);
        cmd.Parameters.AddWithValue("$db", webhook.Database);
        cmd.Parameters.AddWithValue("$secret", (object?)webhook.HmacSecret ?? DBNull.Value);
        cmd.Parameters.AddWithValue("$created", webhook.CreatedAt.ToString("O"));
        await cmd.ExecuteNonQueryAsync();
        return (webhook, true);
    }

    private async Task<WebhookRegistration?> FindActiveWebhookAsync(
        string partnerId, string targetUrl, string eventType, string database)
    {
        await using var conn = new SqliteConnection(_connectionString);
        await conn.OpenAsync();
        await using var cmd = conn.CreateCommand();
        cmd.CommandText = """
            SELECT id, partner_id, target_url, event_type, database, hmac_secret, active, created_at
            FROM webhooks
            WHERE active = 1
              AND partner_id = $pid
              AND target_url = $url
              AND lower(event_type) = lower($etype)
              AND lower(database) = lower($db)
            LIMIT 1;
            """;
        cmd.Parameters.AddWithValue("$pid", partnerId);
        cmd.Parameters.AddWithValue("$url", targetUrl);
        cmd.Parameters.AddWithValue("$etype", eventType);
        cmd.Parameters.AddWithValue("$db", database);

        await using var reader = await cmd.ExecuteReaderAsync();
        if (!await reader.ReadAsync())
            return null;

        return new WebhookRegistration
        {
            Id = Guid.Parse(reader.GetString(0)),
            PartnerId = reader.GetString(1),
            TargetUrl = reader.GetString(2),
            EventType = reader.GetString(3),
            Database = reader.GetString(4),
            HmacSecret = reader.IsDBNull(5) ? null : reader.GetString(5),
            Active = reader.GetInt32(6) == 1,
            CreatedAt = DateTime.Parse(reader.GetString(7))
        };
    }

    public async Task<List<WebhookRegistration>> GetWebhooksAsync(string? partnerId = null)
    {
        await using var conn = new SqliteConnection(_connectionString);
        await conn.OpenAsync();
        await using var cmd = conn.CreateCommand();
        cmd.CommandText = partnerId is null
            ? "SELECT id, partner_id, target_url, event_type, database, hmac_secret, active, created_at FROM webhooks WHERE active = 1;"
            : "SELECT id, partner_id, target_url, event_type, database, hmac_secret, active, created_at FROM webhooks WHERE active = 1 AND partner_id = $pid;";
        if (partnerId is not null)
            cmd.Parameters.AddWithValue("$pid", partnerId);

        var list = new List<WebhookRegistration>();
        await using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            list.Add(new WebhookRegistration
            {
                Id = Guid.Parse(reader.GetString(0)),
                PartnerId = reader.GetString(1),
                TargetUrl = reader.GetString(2),
                EventType = reader.GetString(3),
                Database = reader.GetString(4),
                HmacSecret = reader.IsDBNull(5) ? null : reader.GetString(5),
                Active = reader.GetInt32(6) == 1,
                CreatedAt = DateTime.Parse(reader.GetString(7))
            });
        }
        return list;
    }

    public async Task<bool> DeleteWebhookAsync(Guid id, string partnerId)
    {
        await using var conn = new SqliteConnection(_connectionString);
        await conn.OpenAsync();
        await using var cmd = conn.CreateCommand();
        cmd.CommandText = "UPDATE webhooks SET active = 0 WHERE id = $id AND partner_id = $pid;";
        cmd.Parameters.AddWithValue("$id", id.ToString());
        cmd.Parameters.AddWithValue("$pid", partnerId);
        return await cmd.ExecuteNonQueryAsync() > 0;
    }

    public async Task<List<object>> GetDlqAsync()
    {
        await using var conn = new SqliteConnection(_connectionString);
        await conn.OpenAsync();
        await using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT id, message_id, payload, error, created_at FROM dlq_entries ORDER BY created_at DESC LIMIT 50;";
        var list = new List<object>();
        await using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            list.Add(new
            {
                id = reader.GetString(0),
                messageId = reader.IsDBNull(1) ? null : reader.GetString(1),
                payload = reader.GetString(2),
                error = reader.GetString(3),
                createdAt = reader.GetString(4)
            });
        }
        return list;
    }

    public async Task InsertDlqAsync(string? messageId, string payload, string error)
    {
        await using var conn = new SqliteConnection(_connectionString);
        await conn.OpenAsync();
        await using var cmd = conn.CreateCommand();
        cmd.CommandText = "INSERT INTO dlq_entries (id, message_id, payload, error, created_at) VALUES ($id, $mid, $payload, $err, $created);";
        cmd.Parameters.AddWithValue("$id", Guid.NewGuid().ToString());
        cmd.Parameters.AddWithValue("$mid", (object?)messageId ?? DBNull.Value);
        cmd.Parameters.AddWithValue("$payload", payload);
        cmd.Parameters.AddWithValue("$err", error);
        cmd.Parameters.AddWithValue("$created", DateTime.UtcNow.ToString("O"));
        await cmd.ExecuteNonQueryAsync();
    }
}
