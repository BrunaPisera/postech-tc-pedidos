using Pedidos.UseCases.DTOs;
using Pedidos.UseCases.Gateway;
using Pedidos.UseCases.Interfaces;
using Pedidos.UseCases.Extensions;
using Pedidos.UseCases.Exceptions.Produto;
using Pedidos.UseCases.Exceptions.Pedido;

namespace Pedidos.UseCases
{
    public class PedidoUseCase : IPedidoUseCases
    {
        private readonly IPedidoPersistenceGateway PedidoPersistencePort;
        private readonly IClientePersistenceGateway ClientePersistencePort;
        private readonly IProdutoPersistenceGateway ProdutoPersistencePort;

        public PedidoUseCase(IPedidoPersistenceGateway pedidoPersistencePort,
                            IClientePersistenceGateway clientePersistencePort,
                            IProdutoPersistenceGateway produtoPersistencePort)
        {
            PedidoPersistencePort = pedidoPersistencePort;
            ClientePersistencePort = clientePersistencePort;
            ProdutoPersistencePort = produtoPersistencePort;
        }   

        public async Task<PedidoRealizadoDto> RealizarPedidoAsync(CriaPedidoDto pedidoDto)
        {
            var cliente = await ClientePersistencePort.GetClienteByCPF(pedidoDto.CpfCliente ?? "");
      
            var produtos = await ProdutoPersistencePort.GetProdutosByIdsAsync(pedidoDto.Itens.Select(i => i.IdProduto));

            if (produtos.Count == 0 || produtos.Count != pedidoDto.Itens.Count)
            {
                throw new ProdutoNaoCadastradoException("Um ou mais produtos do pedido nao foram encontrados. Verifique o pedido.");
            }
            var pedido = pedidoDto.ToPedidoAggregate(cliente, produtos);

            var pedidoCadastrado = await PedidoPersistencePort.SavePedidoAsync(pedido);

            if (!pedidoCadastrado) throw new RealizarPedidoException("Ocorreu um erro ao realizar o pedido.");

            return new PedidoRealizadoDto()
            {
                IdPedido = pedido.Id,
                ClienteName = cliente.Nome
            };
        }

        public async Task<PedidoDto> GetPedidoByIdAsync(Guid idPedido)
        {
            var pedido = await PedidoPersistencePort.GetPedidoById(idPedido);

            if (pedido == null) throw new PedidoNaoEncontradoException("Pedido nao encontrado");

            return pedido.ToPedidoDto();
        }

        public async Task<List<PedidoDto>> GetAllPedidosAsync()
        {
            var pedidos = await PedidoPersistencePort.GetAllPedidos();
            var pedidosDto = pedidos.Select(x => x.ToPedidoDto());

            return OrdernarPedidosPorHora(pedidosDto).ToList();
        }

        public async Task<bool> ConfirmaPagamentoAsync(Guid idPedido)
        {
            var pedido = await PedidoPersistencePort.GetPedidoById(idPedido);

            if (pedido == null) throw new PedidoNaoEncontradoException("Pedido nao encontrado");

            pedido.PagamentoConfirmado = true;

            var pedidoAtualizado = await PedidoPersistencePort.SavePedidoAsync(pedido);

            if (!pedidoAtualizado) throw new RealizarPedidoException("Ocorreu um erro ao realizar o pedido.");

            return pedidoAtualizado;
        }       
        private IEnumerable<PedidoDto> OrdernarPedidosPorHora(IEnumerable<PedidoDto> pedidos)
        {
            return pedidos.OrderBy(p => p.HorarioRecebimento);
        }
    }
}
