using Pedidos.Core.Entities;
using Pedidos.UseCases.DTOs;

namespace Pedidos.UseCases.Interfaces
{
    public interface IClienteUseCases
    {
        Task CadastraClienteAsync(CadastraClienteDto clienteDto);
        Task<ClienteAggregate?> GetClienteByCPFAsync(string cpf);
    }
}
