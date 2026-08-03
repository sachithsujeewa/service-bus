using System.Security.Cryptography;
using System.Text;
using Npgsql;
using ServiceBus.Contracts;
using ServiceBus.Messaging;

namespace ServiceBus.Api;

public sealed class DatabaseService : IDataStore
{
    private readonly string _connectionString;

    public DatabaseService(string connectionString) => _connectionString = connectionString;

    public async Task InitializeAsync()
    {
        await using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();
        await using var cmd = conn.CreateCommand();
        cmd.CommandText = """
            CREATE TABLE IF NOT EXISTS webhooks (
                id UUID PRIMARY KEY,
                partner_id TEXT NOT NULL,
                target_url TEXT NOT NULL,
                event_type TEXT NOT NULL,
                database TEXT NOT NULL DEFAULT 'ALL',
                hmac_secret TEXT NULL,
                active BOOLEAN NOT NULL DEFAULT TRUE,
                created_at TIMESTAMPTZ NOT NULL DEFAULT NOW()
            );
            CREATE TABLE IF NOT EXISTS publish_cursor (
                system_id TEXT PRIMARY KEY,
                last_event_id BIGINT NOT NULL DEFAULT 0
            );
            CREATE TABLE IF NOT EXISTS dlq_entries (
                id UUID PRIMARY KEY,
                message_id TEXT,
                payload JSONB NOT NULL,
                error TEXT NOT NULL,
                created_at TIMESTAMPTZ NOT NULL DEFAULT NOW()
            );
            """;
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task<long> NextEventIdAsync(string systemId)
    {
        await using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();
        await using var cmd = conn.CreateCommand();
        cmd.CommandText = """
            INSERT INTO publish_cursor (system_id, last_event_id) VALUES ($1, 1)
            ON CONFLICT (system_id) DO UPDATE SET last_event_id = publish_cursor.last_event_id + 1
            RETURNING last_event_id;
            """;
        cmd.Parameters.AddWithValue(systemId);
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

        await using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();
        await using var cmd = conn.CreateCommand();
        cmd.CommandText = """
            INSERT INTO webhooks (id, partner_id, target_url, event_type, database, hmac_secret, active, created_at)
            VALUES ($1, $2, $3, $4, $5, $6, $7, $8);
            """;
        cmd.Parameters.AddWithValue(webhook.Id);
        cmd.Parameters.AddWithValue(webhook.PartnerId);
        cmd.Parameters.AddWithValue(webhook.TargetUrl);
        cmd.Parameters.AddWithValue(webhook.EventType);
        cmd.Parameters.AddWithValue(webhook.Database);
        cmd.Parameters.AddWithValue((object?)webhook.HmacSecret ?? DBNull.Value);
        cmd.Parameters.AddWithValue(webhook.Active);
        cmd.Parameters.AddWithValue(webhook.CreatedAt);
        await cmd.ExecuteNonQueryAsync();
        return (webhook, true);
    }

    private async Task<WebhookRegistration?> FindActiveWebhookAsync(
        string partnerId, string targetUrl, string eventType, string database)
    {
        await using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();
        await using var cmd = conn.CreateCommand();
        cmd.CommandText = """
            SELECT id, partner_id, target_url, event_type, database, hmac_secret, active, created_at
            FROM webhooks
            WHERE active = TRUE
              AND partner_id = $1
              AND target_url = $2
              AND lower(event_type) = lower($3)
              AND lower(database) = lower($4)
            LIMIT 1;
            """;
        cmd.Parameters.AddWithValue(partnerId);
        cmd.Parameters.AddWithValue(targetUrl);
        cmd.Parameters.AddWithValue(eventType);
        cmd.Parameters.AddWithValue(database);

        await using var reader = await cmd.ExecuteReaderAsync();
        if (!await reader.ReadAsync())
            return null;

        return new WebhookRegistration
        {
            Id = reader.GetGuid(0),
            PartnerId = reader.GetString(1),
            TargetUrl = reader.GetString(2),
            EventType = reader.GetString(3),
            Database = reader.GetString(4),
            HmacSecret = reader.IsDBNull(5) ? null : reader.GetString(5),
            Active = reader.GetBoolean(6),
            CreatedAt = reader.GetDateTime(7)
        };
    }

    public async Task<List<WebhookRegistration>> GetWebhooksAsync(string? partnerId = null)
    {
        await using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();
        await using var cmd = conn.CreateCommand();
        cmd.CommandText = partnerId is null
            ? "SELECT id, partner_id, target_url, event_type, database, hmac_secret, active, created_at FROM webhooks WHERE active = TRUE;"
            : "SELECT id, partner_id, target_url, event_type, database, hmac_secret, active, created_at FROM webhooks WHERE active = TRUE AND partner_id = $1;";
        if (partnerId is not null)
            cmd.Parameters.AddWithValue(partnerId);

        var list = new List<WebhookRegistration>();
        await using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            list.Add(new WebhookRegistration
            {
                Id = reader.GetGuid(0),
                PartnerId = reader.GetString(1),
                TargetUrl = reader.GetString(2),
                EventType = reader.GetString(3),
                Database = reader.GetString(4),
                HmacSecret = reader.IsDBNull(5) ? null : reader.GetString(5),
                Active = reader.GetBoolean(6),
                CreatedAt = reader.GetDateTime(7)
            });
        }
        return list;
    }

    public async Task<bool> DeleteWebhookAsync(Guid id, string partnerId)
    {
        await using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();
        await using var cmd = conn.CreateCommand();
        cmd.CommandText = "UPDATE webhooks SET active = FALSE WHERE id = $1 AND partner_id = $2;";
        cmd.Parameters.AddWithValue(id);
        cmd.Parameters.AddWithValue(partnerId);
        return await cmd.ExecuteNonQueryAsync() > 0;
    }

    public async Task<List<object>> GetDlqAsync()
    {
        await using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();
        await using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT id, message_id, payload, error, created_at FROM dlq_entries ORDER BY created_at DESC LIMIT 50;";
        var list = new List<object>();
        await using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            list.Add(new
            {
                id = reader.GetGuid(0),
                messageId = reader.IsDBNull(1) ? null : reader.GetString(1),
                payload = reader.GetFieldValue<string>(2),
                error = reader.GetString(3),
                createdAt = reader.GetDateTime(4)
            });
        }
        return list;
    }

    public async Task InsertDlqAsync(string? messageId, string payload, string error)
    {
        await using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();
        await using var cmd = conn.CreateCommand();
        cmd.CommandText = "INSERT INTO dlq_entries (id, message_id, payload, error) VALUES ($1, $2, $3::jsonb, $4);";
        cmd.Parameters.AddWithValue(Guid.NewGuid());
        cmd.Parameters.AddWithValue((object?)messageId ?? DBNull.Value);
        cmd.Parameters.AddWithValue(payload);
        cmd.Parameters.AddWithValue(error);
        await cmd.ExecuteNonQueryAsync();
    }
}

public static class HmacHelper
{
    public static string Sign(string body, string secret, string algorithm = "SHA256")
    {
        var key = Encoding.UTF8.GetBytes(secret);
        using HMAC hmac = algorithm.ToUpperInvariant() switch
        {
            "SHA256" => new HMACSHA256(key),
            "SHA1" => new HMACSHA1(key),
            _ => new HMACMD5(key)
        };
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(body));
        return Convert.ToHexString(hash).ToLowerInvariant();
    }
}
