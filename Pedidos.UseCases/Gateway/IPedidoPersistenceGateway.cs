using Pedidos.Core.Entities;

namespace Pedidos.UseCases.Gateway
{
    public interface IPedidoPersistenceGateway
    {      
        Task<bool> SavePedidoAsync(PedidoAggregate pedido);
        Task<PedidoAggregate> GetPedidoById(Guid idPedido);
        Task<List<PedidoAggregate>> GetAllPedidos();
    }
}
