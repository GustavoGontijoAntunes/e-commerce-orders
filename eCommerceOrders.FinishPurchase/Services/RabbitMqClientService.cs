using eCommerceOrders.FinishPurchase.Services.Interfaces;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace eCommerceOrders.FinishPurchase.Services
{
    public class RabbitMqClientService : IBusService
    {
        private readonly IModel _channel;
        const string EXCHANGE = "ecommerce-orders";
        const string ROUTING_KEY = "orders.finished-purchase";
        JsonSerializerOptions options;

        public RabbitMqClientService()
        {
            var connectionFactory = new ConnectionFactory
            {
                HostName = "localhost"
            };

            /* Configuração de serialização para ignorar ciclos */
            options = new JsonSerializerOptions
            {
                ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles,
                WriteIndented = true
            };

            var connection = connectionFactory.CreateConnection("ecommerce-orders");

            _channel = connection.CreateModel();
        }

        public void Publish<T>(T message)
        {
            var json = JsonSerializer.Serialize(message, options);
            var byteArray = Encoding.UTF8.GetBytes(json);

            _channel.BasicPublish(EXCHANGE, ROUTING_KEY, null, byteArray);
        }
    }
}