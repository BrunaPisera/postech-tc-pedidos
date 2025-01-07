using Pedidos.Core.Entities;
using Pedidos.Core.Entities.Enums;

namespace Pedidos.UseCases.Gateway
{
    public interface IProdutoPersistenceGateway
    {
        public Task<bool> SaveProdutoAsync(ProdutoAggregate produtoAggregate);
        Task<ProdutoAggregate?> GetProdutoByNomeAsync(string nome);
        Task<ProdutoAggregate?> GetProdutoByIdAsync(int id);
        Task<IEnumerable<ProdutoAggregate>> GetProdutosAsync();
        Task<IEnumerable<ProdutoAggregate>> GetProdutosByCategoriaAsync(Categoria categoria);
        bool RemoveProduto(ProdutoAggregate produtoAggregate);
        Task<bool> UpdateProdutoAsync(ProdutoAggregate produtoCadastrado);
        Task<List<ProdutoAggregate>> GetProdutosByIdsAsync(IEnumerable<int> ids);
    }
}
