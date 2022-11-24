using System;
using RabbitMQ.Client;
using System.Text;
using SharedCode;

var factory = new ConnectionFactory() { HostName = Consts.HostName, 
                                        Port = Consts.Port, 
                                        UserName = Consts.UserName, 
                                        Password = Consts.Password };
using(var connection = factory.CreateConnection())
using(var channel = connection.CreateModel())
{
    channel.QueueDeclare(queue: "mpHello", 
                         durable: false, 
                         exclusive: false, 
                         autoDelete: false, 
                         arguments: null);

    var message = "Hello MQ";
    var body = Encoding.UTF8.GetBytes(message);

    channel.BasicPublish(exchange: "", 
                         routingKey: "mpHello", 
                         basicProperties: null, 
                         body: body);


    Console.WriteLine("[x] Sent {0}", message);
}

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();
