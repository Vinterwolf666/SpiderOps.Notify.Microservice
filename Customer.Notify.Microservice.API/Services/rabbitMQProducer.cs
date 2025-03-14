namespace Customer.Notify.Microservice.API.Services
{
    using Microsoft.AspNetCore.Connections;
    using RabbitMQ.Client;
    using System;
    using System.Text;
    using System.Threading.Tasks;

        public class RabbitMQProducer
        {
        private const string QueueName = "recovery_password_stage";
        private const string QueueNameAccountCreation = "account_creation_notification";
        private const string QueueNameAccountRemoved = "account_removed_notification";
        private const string QueueNameAccountUpdated = "account_updated_notification";
        private const string QueueNameAppDeployed = "app_deployed_notification";
        private const string QueueDeployedRemoved = "deployment_removed_notification";
        private const string QueueInfrastructure = "infrastructure_notification";

        private readonly ConnectionFactory _factory;

            public RabbitMQProducer()
            {
                _factory = new ConnectionFactory()
                {
                    HostName = "35.202.158.223",
                    UserName = "vinterwolf",
                    Password = "vinterland"
                };

            }

            public async Task NotifyAccountCreationStageCompleted()
            {
                await using var connection = await _factory.CreateConnectionAsync();
                await using var channel = await connection.CreateChannelAsync();

                await channel.QueueDeclareAsync(
                    queue: QueueName,
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null
                );

                var message = "Recovery password email - Stage completed";
                var body = Encoding.UTF8.GetBytes(message);
                var properties = new BasicProperties();

                await channel.BasicPublishAsync(
                    exchange: "",
                    routingKey: QueueName,
                    mandatory: false,
                    basicProperties: properties,
                    body: body
                );

                Console.WriteLine($"[x] Sent: {message}");
            }


        public async Task NotifyAccountCreationEmailSent(string email, int id)
        {
            await using var connection = await _factory.CreateConnectionAsync();
            await using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(
                queue: QueueNameAccountCreation,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            var message = $"Account creation email sent for user {id} ({email})";
            var body = Encoding.UTF8.GetBytes(message);
            var properties = new BasicProperties();

            await channel.BasicPublishAsync(
                exchange: "",
                routingKey: QueueNameAccountCreation,
                mandatory: false,
                basicProperties: properties,
                body: body
            );

            Console.WriteLine($"[x] Sent: {message}");
        }


        public async Task NotifyAccountRemovedEmailSent(string email, int id)
        {
            await using var connection = await _factory.CreateConnectionAsync();
            await using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(
                queue: QueueNameAccountRemoved,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            var message = $"Account removed email sent for user {id} ({email})";
            var body = Encoding.UTF8.GetBytes(message);
            var properties = new BasicProperties();

            await channel.BasicPublishAsync(
                exchange: "",
                routingKey: QueueNameAccountRemoved,
                mandatory: false,
                basicProperties: properties,
                body: body
            );

            Console.WriteLine($"[x] Sent: {message}");
        }


        public async Task NotifyAccountUpdatedEmailSent(string email, int id)
        {
            await using var connection = await _factory.CreateConnectionAsync();
            await using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(
                queue: QueueNameAccountUpdated,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            var message = $"Account updated email sent for user {id} ({email})";
            var body = Encoding.UTF8.GetBytes(message);
            var properties = new BasicProperties();

            await channel.BasicPublishAsync(
                exchange: "",
                routingKey: QueueNameAccountUpdated,
                mandatory: false,
                basicProperties: properties,
                body: body
            );

            Console.WriteLine($"[x] Sent: {message}");
        }

        public async Task NotifyAppDeployedEmailSent(string email, int id)
        {
            await using var connection = await _factory.CreateConnectionAsync();
            await using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(
                queue: QueueNameAppDeployed,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            var message = $"App deployment email sent for user {id} ({email})";
            var body = Encoding.UTF8.GetBytes(message);
            var properties = new BasicProperties();

            await channel.BasicPublishAsync(
                exchange: "",
                routingKey: QueueNameAppDeployed,
                mandatory: false,
                basicProperties: properties,
                body: body
            );

            Console.WriteLine($"[x] Sent: {message}");
        }

        public async Task NotifyRemovedDeploymentEmailSent(string email, int id)
        {
            await using var connection = await _factory.CreateConnectionAsync();
            await using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(
                queue: QueueDeployedRemoved,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            var message = $"Removed deployment email sent for user {id} ({email})";
            var body = Encoding.UTF8.GetBytes(message);
            var properties = new BasicProperties();

            await channel.BasicPublishAsync(
                exchange: "",
                routingKey: QueueDeployedRemoved,
                mandatory: false,
                basicProperties: properties,
                body: body
            );

            Console.WriteLine($"[x] Sent: {message}");
        }


        public async Task NotifyInfrastructureEmailSent(string email, int id)
        {
            await using var connection = await _factory.CreateConnectionAsync();
            await using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(
                queue: QueueInfrastructure,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            var message = $"Infrastructure deployment email sent for user {id} ({email})";
            var body = Encoding.UTF8.GetBytes(message);
            var properties = new BasicProperties();

            await channel.BasicPublishAsync(
                exchange: "",
                routingKey: QueueInfrastructure,
                mandatory: false,
                basicProperties: properties,
                body: body
            );

            Console.WriteLine($"[x] Sent: {message}");
        }


    }
    }



