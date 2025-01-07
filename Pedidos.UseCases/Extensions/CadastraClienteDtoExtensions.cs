using Pedidos.Core.Entities.ValueObjects;
using Pedidos.Core.Entities;
using Pedidos.UseCases.DTOs;

namespace Pedidos.UseCases.Extensions
{
    static internal class CadastraClienteDtoExtensions
    {
        static internal ClienteAggregate ToClienteAggregate(this CadastraClienteDto clienteDto)
        {
            if (clienteDto == null)
            {
                return new ClienteAggregate();
            }

            return new ClienteAggregate()
            {
                CPF = new CPF(clienteDto.CPF),
                Email = new Email(clienteDto.EnderecoEmail ?? ""),
                Nome = clienteDto.Nome
            };
        }
    }
}
