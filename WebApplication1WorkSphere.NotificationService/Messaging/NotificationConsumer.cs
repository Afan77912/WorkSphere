using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace WorkSphere.NotificationService.Messaging
{
    public class NotificationConsumer
    {

        private readonly IConfiguration _configuration;


        public NotificationConsumer(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task StartAsync()
        {
            var factory = new ConnectionFactory
            {
                HostName = _configuration["RabbitMQ:Host"] ?? "rabbitmq",
                Port = int.Parse(_configuration["RabbitMQ:Port"] ?? "5672"),
                UserName = _configuration["RabbitMQ:Username"] ?? "guest",
                Password = _configuration["RabbitMQ:Password"] ?? "guest"
            };

            var connection = await factory.CreateConnectionAsync();
            var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(
                queue: "employee-events",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                if (message.StartsWith("Employee created:"))
                {
                    Console.WriteLine("CREATE EVENT RECEIVED");
                }
                else if (message.StartsWith("Employee updated:"))
                {
                    Console.WriteLine("UPDATE EVENT RECEIVED");
                }
                else if (message.StartsWith("Employee deleted:"))
                {
                    Console.WriteLine("DELETE EVENT RECEIVED");
                }
                Console.WriteLine(
                    $"Notification Service received: {message}"
                );
            };

            await channel.BasicConsumeAsync(
                queue: "employee-events",
                autoAck: true,
                consumer: consumer);

            Console.WriteLine(
                "Notification Service is listening for employee events..."
            );
        }
    }
}