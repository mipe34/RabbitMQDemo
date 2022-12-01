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
    channel.ExchangeDeclare(exchange: "mp-direct-log", type: ExchangeType.Direct);

    var severity = (args.Length > 0) ? args[0] : "info";
    var message = GetMessage(args);
    var body = Encoding.UTF8.GetBytes(message);

    channel.BasicPublish(exchange: "mp-direct-log",
                         routingKey: severity,
                         basicProperties: null,
                         body: body);


    Console.WriteLine("[x] Sent '{0}':'{1}'", message, severity);
}

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();

static string GetMessage(string[] args)
{
    return ((args.Length > 0) ? string.Join(" ", args.Skip(1)) : "Hello World!");
}