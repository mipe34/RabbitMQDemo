using System;
using RabbitMQ.Client;
using System.Text;
using SharedCode;
using RabbitMQ.Client.Events;

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
    channel.QueueDeclare(queue: "mpHello",
                         durable: false,
                         exclusive: false,
                         autoDelete: false,
                         arguments: null);

    var consumer = new EventingBasicConsumer(channel);
    consumer.Received += (model, ea) =>
    {
        var body = ea.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        Console.WriteLine(" [x] Received {0}", message);
    };

    channel.BasicConsume(queue: "mpHello",
                         autoAck: true,
                         consumer: consumer);

    Console.WriteLine(" Press [enter] to exit.");
    Console.ReadLine();
}

