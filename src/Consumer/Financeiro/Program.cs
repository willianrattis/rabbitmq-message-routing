var hostname = "localhost";
var queueName = "fila_3";
var userName = "guest";
var password = "guest";

var subscriber = new RabbitMqSubscriber(hostname, queueName, userName, password);

subscriber.Subscribe("financeiro");
