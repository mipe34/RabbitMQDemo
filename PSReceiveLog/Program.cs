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
    channel.ExchangeDeclare(exchange: "mplog", type: ExchangeType.Fanout);

    var queueName = channel.QueueDeclare().QueueName;
    channel.QueueBind(queue: queueName,
                      exchange: "mplog",
                      routingKey: "");

    Console.WriteLine(" [*] Waiting for logs.");

    var consumer = new EventingBasicConsumer(channel);
    consumer.Received += (model, ea) =>
    {
        var body = ea.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        Console.WriteLine(" [x] {0}", message);
    };

    channel.BasicConsume(queue: queueName,
                         autoAck: true,
                         consumer: consumer);

    Console.WriteLine(" Press [enter] to exit.");
    Console.ReadLine();
}

