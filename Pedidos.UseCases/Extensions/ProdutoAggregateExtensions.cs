using Pedidos.Core.Entities;
using Pedidos.UseCases.DTOs;

namespace Pedidos.UseCases.Extensions
{
    static internal class ProdutoAggregateExtensions
    {
        static internal ProdutoDto ToProdutoDto(this ProdutoAggregate produtoAggregate)
        {
            if (produtoAggregate == null)
            {
                return new ProdutoDto();
            }

            return new ProdutoDto()
            {
                Id = produtoAggregate.Id,
                Nome = produtoAggregate.Nome,
                Preco = produtoAggregate.Preco,
                Categoria = produtoAggregate.Categoria.ToString(),
            };
        }
    }
}
