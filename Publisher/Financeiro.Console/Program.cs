﻿
var factory = new ConnectionFactory()
{
    HostName = "localhost"
};

using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

// Declaração do Exchange
string exchangeName = "my_topic_exchange";
channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Topic);

// Declaração das filas
channel.QueueDeclare(queue: "fila_3", durable: false, exclusive: false, autoDelete: false, arguments: null);

// Vinculação das filas com o Exchange usando padrões de chave de roteamento
channel.QueueBind(queue: "fila_3", exchange: exchangeName, routingKey: "financeiro.*");

Console.WriteLine("Digite a chave de roteamento (e.g., financeiro.xyz):");
string routingKey = Console.ReadLine() ?? "financeiro.default";

Console.WriteLine("Digite a mensagem que deseja enviar:");
string message = Console.ReadLine() ?? string.Empty;

if (string.IsNullOrEmpty(message))
{
    Console.WriteLine("A mensagem não pode ser nula ou vazia.");
}
else
{
    var body = Encoding.UTF8.GetBytes(message);

    channel.BasicPublish(exchange: exchangeName,
                         routingKey: routingKey,
                         basicProperties: null,
                         body: body);

    Console.WriteLine($" [x] Enviou: {message} com a chave de roteamento: {routingKey}");
}