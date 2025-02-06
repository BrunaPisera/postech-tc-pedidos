using Pedidos.UseCases.DTOs;

namespace Pedidos.UseCases.Interfaces
{
    public interface IPedidoUseCases
    {
        Task<PedidoRealizadoDto> RealizarPedidoAsync(CriaPedidoDto pedidoDto);
        Task<List<PedidoDto>> GetAllPedidosAsync();
        Task<PedidoDto> GetPedidoByIdAsync(Guid pedidoId);
        Task<bool> ConfirmaPagamentoAsync(Guid pedidoId);
    }
}
