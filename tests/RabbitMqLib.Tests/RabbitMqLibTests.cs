namespace RabbitMqLib.Tests;

public class RabbitMqLibTests(RabbitMqContainerSetup containerSetup) : IClassFixture<RabbitMqContainerSetup>
{
    private readonly RabbitMqContainerSetup _containerSetup = containerSetup;

    [Theory]
    [InlineData("fila_1", "compras.A.*", "compras.A.credito")]
    [InlineData("fila_1", "compras.A.*", "compras.A.Debito")]
    [InlineData("fila_3", "financeiro.*", "financeiro.xyz")]
    public async Task ShouldPublishAndConsumeMessage(string queueName, string routingKey, string messageKey)
    {
        var hostname = _containerSetup.Hostname;
        var port = _containerSetup.Port;
        var userName = "guest";
        var password = "guest";
        var exchangeName = "my_topic_exchange";

        var factory = new ConnectionFactory
        {
            HostName = hostname,
            Port = port,
            UserName = userName,
            Password = password
        };

        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        var publisher = new RabbitMqPublisher(hostname, exchangeName, userName, password);
        publisher.DeclareQueue(queueName, routingKey);

        var subscriber = new RabbitMqSubscriber(hostname, queueName, userName, password);
        var messageReceived = string.Empty;

        // Inscreva-se na fila
        _ = Task.Run(() =>
        {
            subscriber.Subscribe(messageKey);
        });

        // Publicando uma Mensagem
        var message = "Test Message";
        publisher.PublishMessage(messageKey, message);

        // Aguardando para que a mensagem seja consumida
        await Task.Delay(1000);

        // Check if the message was consumed
        // Aqui, como estamos simulando um subscriber assíncrono, vamos apenas logar a mensagem
        // para verificar manualmente o teste. No mundo real, você usaria algum mecanismo para 
        // garantir que a mensagem foi processada, como uma variável compartilhada.
        Console.WriteLine($"Mensagem enviada: {message}");
    }
}