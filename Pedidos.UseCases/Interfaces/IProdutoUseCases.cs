using Pedidos.Core.Entities.Enums;
using Pedidos.UseCases.DTOs;

namespace Pedidos.UseCases.Interfaces
{
    public interface IProdutoUseCases
    {
        Task<ProdutoDto> AtualizaProdutoAsync(int id, AtualizaProdutoDto produto);
        Task<IEnumerable<ProdutoDto>> BuscaProdutosAsync();
        Task<IEnumerable<ProdutoDto>> BuscaProdutosAsync(Categoria nomeCategoria);
        Task CadastraProdutoAsync(CadastraProdutoDto cadastraProdutoDto);
        Task RemoveProdutoAsync(int id);
    }
}
