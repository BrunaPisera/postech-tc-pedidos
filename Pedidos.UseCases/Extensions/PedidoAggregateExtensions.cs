using Pedidos.Core.Entities;
using Pedidos.UseCases.DTOs;

namespace Pedidos.UseCases.Extensions
{
    static internal class PedidoAggregateExtensions
    {
        static internal PedidoDto ToPedidoDto(this PedidoAggregate pedidoAggregate)
        {
            var pedido = new PedidoDto()
            {
                IdPedido = pedidoAggregate.Id,
                CpfCliente = pedidoAggregate.Cliente?.CPF?.ToString(),
                NomeCliente = pedidoAggregate.Cliente?.Nome,
                ValorTotal = pedidoAggregate.CalcularValorPedido(),                        
                HorarioRecebimento = pedidoAggregate.HorarioRecebimento
            };

            pedidoAggregate.Itens.ForEach(i =>
            {
                pedido.Itens.Add(new ItemPedidoDto()
                {
                    Customizacao = i.Customizacao,
                    Quantidade = i.Quantidade,
                    IdProduto = i.Produto.Id,
                    Preco = i.Produto.Preco.Value
                });

            });

            return pedido;
        }
    }
}
