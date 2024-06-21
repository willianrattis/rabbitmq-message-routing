var factory = new ConnectionFactory()
{
    HostName = "localhost"
};

using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.QueueDeclare(queue: "saudacao_1", // Nome da fila
                     durable: false, // Se igual a true a fila permanece ativa mesmo após o servidor ser reiniciado
                     exclusive: false, // Se igual a true a fila só pode ser acessada pela conexão que a declarou e será excluída quando a conexão for fechada
                     autoDelete: false, // Se igual a true a fila será deletada automaticamente após os consumidores usarem a fila
                     arguments: null);

Console.WriteLine("Digite a mensagem que deseja enviar:");
string message = Console.ReadLine() ?? string.Empty;

if (string.IsNullOrEmpty(message))
{
    Console.WriteLine("A mensagem não pode ser nula ou vazia.");
}
else
{
    var body = Encoding.UTF8.GetBytes(message);

    channel.BasicPublish(exchange: "",
                         routingKey: "saudacao_1",
                         basicProperties: null,
                         body: body);

    Console.WriteLine($" [x] Sended: {message}");
}
