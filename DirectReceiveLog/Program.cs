﻿using System;
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
    channel.ExchangeDeclare(exchange: "mp-direct-log", type: ExchangeType.Direct);

    var queueName = channel.QueueDeclare().QueueName;

    if (args.Length < 1)
    {
        Console.Error.WriteLine("Usage: {0} [info] [warning] [error]",
                                Environment.GetCommandLineArgs()[0]);
        Console.WriteLine(" Press [enter] to exit.");
        Console.ReadLine();
        Environment.ExitCode = 1;
        return;
    }

    foreach (var severity in args)
    {
        channel.QueueBind(queue: queueName,
                          exchange: "mp-direct-log",
                          routingKey: severity);
    }

    Console.WriteLine(" [*] Waiting for messages.");

    var consumer = new EventingBasicConsumer(channel);
    consumer.Received += (model, ea) =>
    {
        var body = ea.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        Console.WriteLine(" [x] Received '{0}':'{1}'", ea.RoutingKey, message);
    };

    channel.BasicConsume(queue: queueName,
                         autoAck: true,
                         consumer: consumer);

    Console.WriteLine(" Press [enter] to exit.");
    Console.ReadLine();
}

