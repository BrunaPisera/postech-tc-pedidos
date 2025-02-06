using Pedidos.Core.Entities.ValueObjects;
using Pedidos.Core.Entities;
using Pedidos.UseCases.DTOs;

namespace Pedidos.UseCases.Extensions
{
    static internal class CadastrarProdutoDtoExtensions
    {
        static internal ProdutoAggregate ToProdutoAggregate(this CadastraProdutoDto cadastraProdutoDto)
        {
            if (cadastraProdutoDto == null)
            {
                return new ProdutoAggregate();
            }

            return new ProdutoAggregate()
            {
                Nome = cadastraProdutoDto.Nome,
                Preco = new Preco(cadastraProdutoDto.Preco),
                Categoria = cadastraProdutoDto.Categoria,
            };
        }
    }
}
