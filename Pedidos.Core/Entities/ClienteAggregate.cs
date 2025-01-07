using Pedidos.Core.Entities.ValueObjects;

namespace Pedidos.Core.Entities
{
    public class ClienteAggregate : Entity<Guid>, IAggregateRoot
    {
        public CPF? CPF { get; set; }
        public Email? Email { get; set; }
        public string? Nome { get; set; }
    }
}
