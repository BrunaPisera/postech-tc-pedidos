using RabbitMQ.Client;
using System.Text;

namespace Pedidos.Infrastructure.Broker
{
    public interface IBrokerPublisher
    {
        void PublishMessage(string exchange, string message, string routingKey);
    }

    public class BrokerPublisher : IBrokerPublisher
    {
        IBrokerConnection _brokerConnection;
        public BrokerPublisher(IBrokerConnection brokerConnection)
        {
            _brokerConnection = brokerConnection;
        }

        public void PublishMessage(string exchange, string message, string routingKey)
        {
            using (var channel = _brokerConnection.CreateChannel())
            {
                channel.ExchangeDeclare(exchange: exchange,
                                         type: "topic",
                                         durable: true,
                                         autoDelete: false);
                
                var body = Encoding.UTF8.GetBytes(message);
                
                channel.BasicPublish(exchange: exchange,
                                     routingKey: routingKey,
                                     basicProperties: null,
                                     body: body);               
            }
        }
    }
}
