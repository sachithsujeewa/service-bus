namespace ServiceBus.Contracts;

public static class ApiRoles
{
    public const string Source = "source";
    public const string Partner = "partner";
    public const string Admin = "admin";
}

public sealed class ApiKeyIdentity
{
    public string Role { get; set; } = "";
    public string? PartnerId { get; set; }
}
