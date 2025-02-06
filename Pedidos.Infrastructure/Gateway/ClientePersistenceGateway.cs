using Microsoft.EntityFrameworkCore;
using Pedidos.Core.Entities;
using Pedidos.Infrastructure.Data;
using Pedidos.UseCases.Gateway;

namespace Pedidos.Infrastructure.Gateway
{
    internal class ClientePersistenceGateway : IClientePersistenceGateway
    {
        private ApplicationContext Context;

        public ClientePersistenceGateway(ApplicationContext context)
        {
            Context = context;
        }

        public async Task<ClienteAggregate?> GetClienteByCPF(string cpf)
        {
            return await Context.Cliente.Include(x => x.CPF).FirstOrDefaultAsync(c => c.CPF != null && c.CPF.Value == cpf);
        }

        public async Task<bool> SalvarClienteAsync(ClienteAggregate clienteAggregate)
        {
            await Context.Cliente.AddAsync(clienteAggregate);

            var result = await Context.SaveChangesAsync();

            return result > 0;
        }
    }
}
