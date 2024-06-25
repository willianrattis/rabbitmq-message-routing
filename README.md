# RabbitMQ Topic Exchange Example

This application demonstrates the capabilities of RabbitMQ for routing messages using the Topic Exchange type. It allows one application to send messages to a Broker, which then routes the messages to a queue where two applications can consume the same queue but not consume each other's messages. Thus, two applications can consume the same queue, each consuming only its own messages.

## Project Structure

```
src
├── Consumer
│   ├── Compras.Credito
│   │   ├── bin
│   │   ├── obj
│   │   ├── Compras.Credito.csproj
│   │   ├── GlobalUsings.cs
│   │   └── Program.cs
│   ├── Compras.Debito
│   │   ├── bin
│   │   ├── obj
│   │   ├── Compras.Debito.csproj
│   │   ├── GlobalUsings.cs
│   │   └── Program.cs
│   └── Financeiro
│       ├── bin
│       ├── obj
│       ├── Financeiro.csproj
│       ├── GlobalUsings.cs
│       └── Program.cs
├── Core
│   └── RabbitMqLib
│       ├── bin
│       ├── obj
│       ├── RabbitMqLib.csproj
│       ├── RabbitMqPublisher.cs
│       └── RabbitMqSubscriber.cs
├── Publisher
│   ├── Compras.Console
│   │   ├── bin
│   │   ├── obj
│   │   ├── Compras.Console.csproj
│   │   ├── GlobalUsings.cs
│   │   └── Program.cs
│   └── Financeiro.Console
│       ├── bin
│       ├── obj
│       ├── Financeiro.Console.csproj
│       ├── GlobalUsings.cs
│       └── Program.cs
```

## Prerequisites

- Docker

## How to Run

1. **Start RabbitMQ Docker Container**

    Start a RabbitMQ container with management plugins using the following command:

    ```sh
    docker run -d --name some-rabbit -p 5672:5672 -p 15672:15672 rabbitmq:3-management
    ```

    You can access the RabbitMQ management interface at [http://localhost:15672](http://localhost:15672) with the default credentials:
    - **Username:** guest
    - **Password:** guest

2. **Navigate to the project directory:**

    ```sh
    cd path/to/your/project
    ```

3. **Run the consumer applications:**

    ```sh
    cd src/Consumer/Compras.Credito
    dotnet run
    ```

    ```sh
    cd src/Consumer/Compras.Debito
    dotnet run
    ```

    ```sh
    cd src/Consumer/Financeiro
    dotnet run
    ```

4. **Run the publisher applications:**

    ```sh
    cd src/Publisher/Compras.Console
    dotnet run
    ```

    ```sh
    cd src/Publisher/Financeiro.Console
    dotnet run
    ```

## Code Examples

### Subscriber

```csharp
var hostname = "localhost";
var queueName = "fila_1";
var userName = "guest";
var password = "guest";

var subscriber = new RabbitMqSubscriber(hostname, queueName, userName, password);
subscriber.Subscribe("compras.A.credito");

var subscriber2 = new RabbitMqSubscriber(hostname, queueName, userName, password);
subscriber2.Subscribe("compras.A.debito");

var subscriber3 = new RabbitMqSubscriber(hostname, "fila_3", userName, password);
subscriber3.Subscribe("financeiro");
```

### Publisher

```csharp
var hostname = "localhost";
var exchangeName = "my_topic_exchange";
var userName = "guest";
var password = "guest";

var publisher = new RabbitMqPublisher(hostname, exchangeName, userName, password);
publisher.DeclareQueue("fila_1", "compras.A.*");

Console.WriteLine("Digite a chave de roteamento (e.g., compras.A.credito ou compras.A.debito):");
string routingKey = Console.ReadLine() ?? "compras.A.default";

Console.WriteLine("Digite a mensagem que deseja enviar:");
string message = Console.ReadLine() ?? string.Empty;

publisher.PublishMessage(routingKey, message);
```

---

# Exemplo de Exchange do RabbitMQ com Topic

Esta aplicação demonstra as capacidades do RabbitMQ para roteamento de mensagens usando o tipo de Exchange Topic. Ela permite que uma aplicação envie mensagens para um Broker, que então roteia as mensagens para uma fila onde duas aplicações podem consumir a mesma fila mas não consumir as mensagens uma da outra. Assim, duas aplicações podem consumir a mesma fila, cada uma consumindo apenas suas próprias mensagens.

## Estrutura do Projeto

```
src
├── Consumer
│   ├── Compras.Credito
│   │   ├── bin
│   │   ├── obj
│   │   ├── Compras.Credito.csproj
│   │   ├── GlobalUsings.cs
│   │   └── Program.cs
│   ├── Compras.Debito
│   │   ├── bin
│   │   ├── obj
│   │   ├── Compras.Debito.csproj
│   │   ├── GlobalUsings.cs
│   │   └── Program.cs
│   └── Financeiro
│       ├── bin
│       ├── obj
│       ├── Financeiro.csproj
│       ├── GlobalUsings.cs
│       └── Program.cs
├── Core
│   └── RabbitMqLib
│       ├── bin
│       ├── obj
│       ├── RabbitMqLib.csproj
│       ├── RabbitMqPublisher.cs
│       └── RabbitMqSubscriber.cs
├── Publisher
│   ├── Compras.Console
│   │   ├── bin
│   │   ├── obj
│   │   ├── Compras.Console.csproj
│   │   ├── GlobalUsings.cs
│   │   └── Program.cs
│   └── Financeiro.Console
│       ├── bin
│       ├── obj
│       ├── Financeiro.Console.csproj
│       ├── GlobalUsings.cs
│       └── Program.cs
```

## Pré-requisitos

- Docker

## Como Executar

1. **Inicie o Container Docker do RabbitMQ**

    Inicie um container RabbitMQ com plugins de gerenciamento usando o comando:

    ```sh
    docker run -d --name some-rabbit -p 5672:5672 -p 15672:15672 rabbitmq:3-management
    ```

    Você pode acessar a interface de gerenciamento do RabbitMQ em [http://localhost:15672](http://localhost:15672) com as credenciais padrão:
    - **Usuário:** guest
    - **Senha:** guest

2. **Navegue até o diretório do projeto:**

    ```sh
    cd path/to/your/project
    ```

3. **Execute as aplicações consumidoras:**

    ```sh
    cd src/Consumer/Compras.Credito
    dotnet run
    ```

    ```sh
    cd src/Consumer/Compras.Debito
    dotnet run
    ```

    ```sh
    cd src/Consumer/Financeiro
    dotnet run
    ```

4. **Execute as aplicações publicadoras:**

    ```sh
    cd src/Publisher/Compras.Console
    dotnet run
    ```

    ```sh
    cd src/Publisher/Financeiro.Console
    dotnet run
    ```

## Exemplos de Código

### Consumidor

```csharp
var hostname = "localhost";
var queueName = "fila_1";
var userName = "guest";
var password = "guest";

var subscriber = new RabbitMqSubscriber(hostname, queueName, userName, password);
subscriber.Subscribe("compras.A.credito");

var subscriber2 = new RabbitMqSubscriber(hostname, queueName, userName, password);
subscriber2.Subscribe("compras.A.debito");

var subscriber3 = new RabbitMqSubscriber(hostname, "fila_3", userName, password);
subscriber3.Subscribe("financeiro");
```

### Publicador

```csharp
var hostname = "localhost";
var exchangeName = "my_topic_exchange";
var userName = "guest";
var password = "guest";

var publisher = new RabbitMqPublisher(hostname, exchangeName, userName, password);
publisher.DeclareQueue("fila_1", "compras.A.*");

Console.WriteLine("Digite a chave de roteamento (e.g., compras.A.credito ou compras.A.debito):");
string routingKey = Console.ReadLine() ?? "compras.A.default";

Console.WriteLine("Digite a mensagem que deseja enviar:");
string message = Console.ReadLine() ?? string.empty;

publisher.PublishMessage(routingKey, message);
```

---
