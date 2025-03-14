using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Customer.Notify.Microservice.API.Services
{
    public class RabbitMQConsumer
    {
        private const string QueueName = "recovery_password_stage";
        private readonly ConnectionFactory _factory;

        public RabbitMQConsumer()
        {
            _factory = new ConnectionFactory()
            {
                HostName = "35.202.158.223",
                UserName = "vinterwolf",
                Password = "vinterland"
            };
        }

        public async Task StartListening()
        {
            using var connection = await _factory.CreateConnectionAsync();  
            using var channel = await connection.CreateChannelAsync();
            

            await channel.QueueDeclareAsync(queue: QueueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                try
                {
                    var recoveryRequest = JsonSerializer.Deserialize<RecoveryPasswordMessage>(message);
                    if (recoveryRequest != null)
                    {
                        Console.WriteLine($"[x] Processing recovery password for {recoveryRequest.Email}");

                        
                        await SendEmailAsync(recoveryRequest.Email);
                    }

                    await channel.BasicAckAsync(ea.DeliveryTag, false); 
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Error] Failed to process message: {ex.Message}");
                }
            };

            await channel.BasicConsumeAsync(queue: QueueName, autoAck: false, consumer: consumer);

            Console.WriteLine("[*] Waiting for messages.");
            await Task.Delay(Timeout.Infinite); 
        }

        private async Task SendEmailAsync(string email)
        {
            Console.WriteLine($"[Email] Sending password recovery email to {email}");
            await Task.Delay(1000); 
        }
    }

    public class RecoveryPasswordMessage
    {
        public string Email { get; set; }
        public int AccountCreationStage { get; set; }
    }
}
