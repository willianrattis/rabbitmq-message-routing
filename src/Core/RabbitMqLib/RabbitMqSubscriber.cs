namespace RabbitMqLib;

/// <summary>
/// Classe responsável por subscrever e processar mensagens de uma fila no RabbitMQ.
/// </summary>
public class RabbitMqSubscriber
{
    private readonly IModel _channel;
    private readonly string _queueName;

    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="RabbitMqSubscriber"/>.
    /// </summary>
    /// <param name="hostname">O nome do host onde o RabbitMQ está rodando.</param>
    /// <param name="queueName">O nome da fila da qual as mensagens serão consumidas.</param>
    /// <param name="userName">O nome de usuário para autenticação no RabbitMQ.</param>
    /// <param name="password">A senha para autenticação no RabbitMQ.</param>
    public RabbitMqSubscriber(string hostname, string queueName, string userName, string password)
    {
        var factory = new ConnectionFactory()
        {
            HostName = hostname,
            UserName = userName,
            Password = password
        };
        var connection = factory.CreateConnection();
        _channel = connection.CreateModel();
        _queueName = queueName;
        _channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
    }

    /// <summary>
    /// Subscreve e processa mensagens da fila especificada.
    /// </summary>
    /// <param name="routingKey">A chave de roteamento para filtrar mensagens.</param>
    public void Subscribe(string routingKey)
    {
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var receivedRoutingKey = ea.RoutingKey;

            if (receivedRoutingKey.Contains(routingKey))
            {
                var timestamp = DateTime.Now.ToString("HH:mm:ss");
                Console.WriteLine($" [X] Recebida: {message} with routing key: {receivedRoutingKey} at {timestamp}");
            }
        };

        _channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);
        Console.WriteLine("Aguardando mensagens. Pressione [enter] para sair.");
        Console.ReadLine();
    }
}
