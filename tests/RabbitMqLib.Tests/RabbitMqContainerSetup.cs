namespace RabbitMqLib.Tests;

public class RabbitMqContainerSetup : IAsyncLifetime
{
    public RabbitMqContainer RabbitMqContainer { get; private set; }
    public string Hostname => RabbitMqContainer.Hostname;
    public int Port => RabbitMqContainer.GetMappedPublicPort(5672);

    public async Task InitializeAsync()
    {
        RabbitMqContainer = new RabbitMqBuilder()
            .WithImage("rabbitmq:3-management")
            .WithPortBinding(5672, 5672)
            .WithPortBinding(15672, 15672)
            .WithUsername("guest")
            .WithPassword("guest")
            .Build();

        await RabbitMqContainer.StartAsync();
    }

    public async Task DisposeAsync()
    {
        if (RabbitMqContainer != null)
        {
            await RabbitMqContainer.StopAsync();
            await RabbitMqContainer.DisposeAsync();
        }
    }
}
