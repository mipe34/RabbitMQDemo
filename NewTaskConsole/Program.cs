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
    channel.QueueDeclare(queue: "mptask-queue",
                         durable: true,
                         exclusive: false,
                         autoDelete: false,
                         arguments: null);

    var message = GetMessage(args);
    var body = Encoding.UTF8.GetBytes(message);
    
    var properties = channel.CreateBasicProperties();
    properties.Persistent = true;

    channel.BasicPublish(exchange: "",
                         routingKey: "mptask-queue",
                         basicProperties: properties,
                         body: body);


    Console.WriteLine("[x] Sent {0}", message);
}

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();

static string GetMessage(string[] args)
{
    return ((args.Length > 0) ? string.Join(" ", args) : "Hello World!");
}