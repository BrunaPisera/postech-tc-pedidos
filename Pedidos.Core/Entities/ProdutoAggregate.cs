using Pedidos.Core.Entities.Enums;
using Pedidos.Core.Entities.ValueObjects;

namespace Pedidos.Core.Entities
{
    public class ProdutoAggregate : Entity<int>, IAggregateRoot
    {
        public string Nome { get; set; }
        public Preco Preco { get; set; }
        public Categoria Categoria { get; set; }
    }
}
