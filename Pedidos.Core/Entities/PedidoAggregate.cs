namespace Pedidos.Core.Entities
{
    public class PedidoAggregate : Entity<Guid>, IAggregateRoot
    {
        public ClienteAggregate? Cliente { get; set; }
        public List<ItemPedido> Itens { get; set; } = new List<ItemPedido>();
        public double CalcularValorPedido() => Itens.Sum(x => x.Produto.Preco.Value * x.Quantidade);
        public bool PagamentoConfirmado { get; set; }
        public DateTime HorarioRecebimento { get; set; } = DateTime.UtcNow;
    }
}
