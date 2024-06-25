var hostname = "localhost";
var exchangeName = "my_topic_exchange";
var userName = "guest";
var password = "guest";

var publisher = new RabbitMqPublisher(hostname, exchangeName, userName, password);
publisher.DeclareQueue("fila_3", "financeiro.*");

Console.WriteLine("Digite a chave de roteamento (e.g., financeiro.xyz):");
string routingKey = Console.ReadLine() ?? "financeiro.default";

Console.WriteLine("Digite a mensagem que deseja enviar:");
string message = Console.ReadLine() ?? string.Empty;

publisher.PublishMessage(routingKey, message);
