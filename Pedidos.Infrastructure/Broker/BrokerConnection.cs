using RabbitMQ.Client;

namespace Pedidos.Infrastructure.Broker
{
    public interface IBrokerConnection
    {
        IModel CreateChannel();
        void Dispose();
    }

    public class BrokerConnection : IBrokerConnection
    {
        private readonly IConnection _connection;

        public BrokerConnection()
        {
            var hostName = Environment.GetEnvironmentVariable("BROKER_HOSTNAME");
            var portString = Environment.GetEnvironmentVariable("BROKER_PORT");         

            if (string.IsNullOrEmpty(portString) || !int.TryParse(portString, out var port))
            {
                throw new ArgumentException("A variável de ambiente BROKER_PORT é inválida ou não está definida.");
            }

            var userName = Environment.GetEnvironmentVariable("BROKER_USERNAME");
            var password = Environment.GetEnvironmentVariable("BROKER_PASSWORD");

            var factory = new ConnectionFactory
            {
                HostName = hostName,
                Port = port,
                UserName = userName,
                Password = password
            };

            _connection = factory.CreateConnection();
        }

        public IModel CreateChannel()
        {
            return _connection.CreateModel();
        }

        public void Dispose()
        {
            _connection?.Dispose();
        }
    }
}
