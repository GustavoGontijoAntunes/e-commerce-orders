using eCommerceOrders.SendEmail.Models;
using eCommerceOrders.SendEmail.Services.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Text.Json;

namespace eCommerceOrders.SendEmail.Services
{
    public class OrderCreatedConsumerService : IHostedService
    {
        private readonly IModel _channel;
        const string ORDER_PURCHASE_FINISHED_QUEUE = "order-purchase-finished";
        public IEmailService _emailService;
        public readonly string _emailTo;
        public readonly string _subject;
        public string body;
        CultureInfo cultureInfo;

        public OrderCreatedConsumerService(IEmailService emailService)
        {
            var connectionFactory = new ConnectionFactory
            {
                HostName = "localhost"
            };

            var connection = connectionFactory.CreateConnection("ecommerce-orders-consumer");

            _channel = connection.CreateModel();
            _emailService = emailService;
            _emailTo = "gustavo_antunes07@hotmail.com";
            _subject = "Compra Efetuada com Sucesso!";
            body = "";
            cultureInfo = new CultureInfo("pt-BR");
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (sender, eventArgs) =>
            {
                var contentArray = eventArgs.Body.ToArray();
                var contentString = Encoding.UTF8.GetString(contentArray);

                var @event = JsonSerializer.Deserialize<Order>(contentString);

                body = $"<p>O pedido de número {@event.Id} foi efetuado com sucesso!</p>" +
                       $"<p>Itens do pedido:</p><ul>";

                foreach(var item in @event.OrderProducts)
                {
                    body += $"<li>{item.Quantity}x {item.Product.Name} = R${(item.Quantity * item.Product.Price).ToString("N2", cultureInfo)}</li>";
                }

                body += $"</ul><p>Valor total: R${ @event.TotalValue.ToString("N2", cultureInfo)}</p>";
                body += "<p>Muito obrigado por comprar conosco! =)</p>";

                Console.WriteLine($"Message received: {contentString}");
                _emailService.SendEmailAsync(_emailTo, _subject, body, true);

                _channel.BasicAck(eventArgs.DeliveryTag, false);
            };

            _channel.BasicConsume(ORDER_PURCHASE_FINISHED_QUEUE, false, consumer);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}