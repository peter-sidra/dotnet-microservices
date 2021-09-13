using System;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using PlatformService.Dtos;
using RabbitMQ.Client;

namespace PlatformService.AsyncDataServices {
    public class MessageBusClient : IMessageBusClient {
        private readonly IConfiguration _config;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public MessageBusClient(IConfiguration config) {
            _config = config;

            // Setup the connection to the message bus
            var factory = new ConnectionFactory() {
                HostName = _config["RabbitMQHost"],
                Port = int.Parse(_config["RabbitMQPort"])
            };
            try {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();

                _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
                _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;

                Console.WriteLine("--> Connected to the message bus");
            }
            catch (System.Exception ex) {
                Console.WriteLine("--> Couldn't connect to the" +
                $" message bus: {ex.Message}");
            }
        }
        public void PublishNewPlatform(PlatformPublishedDto platformPublishedDto) {
            var message = JsonSerializer.Serialize(platformPublishedDto);

            if (_connection.IsOpen) {
                Console.WriteLine("--> RabbitMQ Connection Open, sending message...");
                sendMessage(message);
            }
            else {
                Console.WriteLine("--> RabbitMQ connection is closed, not sending");
            }
        }

        private void sendMessage(string message) {
            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(
                exchange: "trigger",
                routingKey: "",
                basicProperties: null,
                body: body);
            Console.WriteLine($"--> We have sent {message}");
        }

        private void Dispose() {
            Console.WriteLine("MessageBus Disposed");
            if (_channel.IsOpen) {
                _channel.Close();
                _connection.Close();
            }
        }

        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs shutdownEventArgs) {
            Console.WriteLine("--> RabbitMQ Connection Shutdown");
        }
    }
}