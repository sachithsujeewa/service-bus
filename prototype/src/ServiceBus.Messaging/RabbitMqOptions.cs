namespace ServiceBus.Messaging;

public sealed class RabbitMqOptions
{
    public string Host { get; set; } = "localhost";
    public int Port { get; set; } = 5672;
    public string User { get; set; } = "rambase";
    public string Password { get; set; } = "rambase";
    public string VHost { get; set; } = "rambase";
}
