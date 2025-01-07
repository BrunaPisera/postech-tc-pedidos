using Pedidos.Core.Entities;

namespace Pedidos.UseCases.Gateway
{
    public interface IClientePersistenceGateway
    {
        Task<bool> SalvarClienteAsync(ClienteAggregate clienteAggregate);
        Task<ClienteAggregate?> GetClienteByCPF(string cpf);
    }
}
