using System;
using RabbitMQ.Client;
using System.Text;
using SharedCode;

var factory = new ConnectionFactory()
{
    HostName = Consts.HostName,
    Port = Consts.Port,
    UserName = Consts.UserName,
    Password = Consts.Password
};
using (var connection = factory.CreateConnection())
using (var channel = connection.CreateModel())
{
    channel.ExchangeDeclare(exchange: "mp-topic-log", type: ExchangeType.Topic);

    var routingKey = (args.Length > 0) ? args[0] : "anonymous.info";
    var message = GetMessage(args);
    var body = Encoding.UTF8.GetBytes(message);

    var properties = channel.CreateBasicProperties();
    properties.Persistent = true;

    channel.BasicPublish(exchange: "mp-topic-log",
                         routingKey: routingKey,
                         basicProperties: null,
                         body: body);


    Console.WriteLine("[x] Sent '{0}':'{1}'", message, routingKey);
}

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();

static string GetMessage(string[] args)
{
    return ((args.Length > 0) ? string.Join(" ", args.Skip(1)) : "Hello World!");
}