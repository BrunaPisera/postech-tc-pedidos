using Pedidos.Core.Entities;
using Pedidos.Infrastructure.Data;
using Pedidos.UseCases.Gateway;
using Microsoft.EntityFrameworkCore;

namespace Pedidos.Infrastructure.Gateway
{
    internal class PedidoPersistenceGateway : IPedidoPersistenceGateway
    {
        private ApplicationContext Context;

        public PedidoPersistenceGateway(ApplicationContext context)
        {
            Context = context;
        }

        public async Task<List<PedidoAggregate>> GetAllPedidos()
        {          
            var pedidos = await Context.Pedido
                .Include(x => x.Cliente)               
                .Include(p => p.Itens)
                .ThenInclude(p => p.Produto)
                .ToListAsync();

            return pedidos;
        }

        public async Task<PedidoAggregate?> GetPedidoById(Guid idPedido)
        {
            var pedido =
                await Context.Pedido
                        .Include(x => x.Cliente)                      
                        .Include(p => p.Itens)
                        .ThenInclude(p => p.Produto)
                        .FirstOrDefaultAsync(x => x.Id == idPedido);

            return pedido;
        } 

        public async Task<bool> SavePedidoAsync(PedidoAggregate pedido)
        {
            await Context.Pedido.AddAsync(pedido);

            var result = await Context.SaveChangesAsync();

            return result > 0;
        }
    }
}