var factory = new ConnectionFactory()
{
    HostName = "localhost"
};

using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.QueueDeclare(queue: "fila_1",
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

var consumer = new EventingBasicConsumer(channel);

consumer.Received += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    var routingKey = ea.RoutingKey;

    if (routingKey == "compras.A.debito")
    {
        var timestamp = DateTime.Now.ToString("HH:mm:ss");
        Console.WriteLine($" [X] Received in debit: {message} at {timestamp}");
    }
};

channel.BasicConsume(queue: "fila_1",
                     autoAck: true,
                     consumer: consumer);

Console.WriteLine("Consumidor de débito aguardando mensagens. Pressione [enter] para sair.");
Console.ReadLine();
