namespace RabbitMqLib;

/// <summary>
/// Classe responsável por publicar mensagens no RabbitMQ.
/// </summary>
public class RabbitMqPublisher
{
    private readonly IModel _channel;
    private readonly string _exchangeName;

    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="RabbitMqPublisher"/>.
    /// </summary>
    /// <param name="hostname">O nome do host onde o RabbitMQ está rodando.</param>
    /// <param name="exchangeName">O nome do exchange no RabbitMQ.</param>
    /// <param name="userName">O nome de usuário para autenticação no RabbitMQ.</param>
    /// <param name="password">A senha para autenticação no RabbitMQ.</param>
    public RabbitMqPublisher(string hostname, string exchangeName, string userName, string password)
    {
        var factory = new ConnectionFactory()
        {
            HostName = hostname,
            UserName = userName,
            Password = password
        };
        var connection = factory.CreateConnection();
        _channel = connection.CreateModel();
        _exchangeName = exchangeName;
        _channel.ExchangeDeclare(exchange: _exchangeName, type: ExchangeType.Topic);
    }

    /// <summary>
    /// Declara uma fila e a vincula ao exchange com a chave de roteamento especificada.
    /// </summary>
    /// <param name="queueName">O nome da fila a ser declarada.</param>
    /// <param name="routingKey">A chave de roteamento usada para vincular a fila ao exchange.</param>
    public void DeclareQueue(string queueName, string routingKey)
    {
        _channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
        _channel.QueueBind(queue: queueName, exchange: _exchangeName, routingKey: routingKey);
    }

    /// <summary>
    /// Publica uma mensagem no exchange com a chave de roteamento especificada.
    /// </summary>
    /// <param name="routingKey">A chave de roteamento usada para enviar a mensagem.</param>
    /// <param name="message">A mensagem a ser enviada.</param>
    /// <exception cref="ArgumentException">Lançado quando a mensagem é nula ou vazia.</exception>
    public void PublishMessage(string routingKey, string message)
    {
        if (string.IsNullOrEmpty(message))
            throw new ArgumentException("A mensagem não pode ser nula ou vazia.", nameof(message));

        var body = Encoding.UTF8.GetBytes(message);
        _channel.BasicPublish(exchange: _exchangeName, routingKey: routingKey, basicProperties: null, body: body);
        Console.WriteLine($" [x] Enviou: {message} com a chave de roteamento: {routingKey}");
    }
}
