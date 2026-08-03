using ServiceBus.Dispatcher;
using ServiceBus.Messaging;

var builder = Host.CreateApplicationBuilder(args);

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

builder.Services.AddSingleton(rabbitOptions);
builder.Services.AddSingleton(sp =>
    new DispatcherWorker(
        sp.GetRequiredService<ILogger<DispatcherWorker>>(),
        rabbitOptions,
        postgresConnection));

builder.Services.AddHostedService(sp => sp.GetRequiredService<DispatcherWorker>());

var host = builder.Build();
host.Run();
