using Pedidos.Infrastructure.Broker;
using Pedidos.Infrastructure.Data;
using Pedidos.Infrastructure.Gateway;
using Pedidos.UseCases;
using Pedidos.UseCases.Gateway;
using Pedidos.UseCases.Interfaces;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IClienteUseCases, ClienteUseCases>();
            services.AddScoped<IClientePersistenceGateway, ClientePersistenceGateway>();

            services.AddScoped<IProdutoUseCases, ProdutoUseCases>();
            services.AddScoped<IProdutoPersistenceGateway, ProdutoPersistenceGateway>();

            services.AddScoped<IPedidoUseCases, PedidoUseCase>();
            services.AddScoped<IPedidoPersistenceGateway, PedidoPersistenceGateway>();

            services.AddScoped<IBrokerConnection, BrokerConnection>();
            services.AddScoped<IBrokerPublisher, BrokerPublisher>();

            services.AddDbContext<ApplicationContext>();

            return services;
        }
    }
}
