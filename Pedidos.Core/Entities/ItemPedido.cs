namespace Pedidos.Core.Entities
{
    public class ItemPedido : Entity<Guid>
    {
        public ProdutoAggregate Produto { get; set; }
        public short Quantidade { get; set; }
        public string? Customizacao { get; set; }
    }
}