using Moq;
using Pedidos.Core.Entities;
using Pedidos.Core.Entities.Enums;
using Pedidos.Core.Entities.ValueObjects;
using Pedidos.UseCases.DTOs;
using Pedidos.UseCases.Exceptions.Produto;
using Pedidos.UseCases.Gateway;

namespace Pedidos.UseCases.Tests
{
    public class ProdutoUseCasesTests
    {
        private Mock<IProdutoPersistenceGateway> _mockProdutoPersistenceGateway;
        private ProdutoUseCases _produtoUseCases;

        [SetUp]
        public void SetUp()
        {
            _mockProdutoPersistenceGateway = new Mock<IProdutoPersistenceGateway>();
            _produtoUseCases = new ProdutoUseCases(_mockProdutoPersistenceGateway.Object);
        }

        [Test]
        public void Can_Create()
        {
            Assert.That(_produtoUseCases, Is.Not.Null);
        }

        [Test]
        public async Task BuscaProdutosAsync_ValidCall_ReturnsProdutoDtos()
        {
            var produtos = new List<ProdutoAggregate>
            {
                new ProdutoAggregate { Id = 1, Nome = "Produto 1", Preco = new Preco(10.0), Categoria = Categoria.Lanche },
                new ProdutoAggregate { Id = 2, Nome = "Produto 2", Preco = new Preco(20.0), Categoria = Categoria.Lanche }
            };

            _mockProdutoPersistenceGateway.Setup(x => x.GetProdutosByCategoriaAsync(Categoria.Lanche)).ReturnsAsync(produtos);

            var result = await _produtoUseCases.BuscaProdutosAsync(Categoria.Lanche);

            var resultList = result.ToList();

            Assert.That(result, Is.Not.Null);
            Assert.That(resultList[0].Nome, Is.EqualTo("Produto 1"));
            Assert.That(resultList[0].Preco.Value, Is.EqualTo(10.0));
        }

        [Test]
        public async Task CadastraProdutoAsync_ProdutoJaCadastrado_ThrowsProdutoJaCadastradoException()
        {
            var cadastraProdutoDto = new CadastraProdutoDto { Nome = "Produto Já Cadastrado" };
            _mockProdutoPersistenceGateway.Setup(x => x.GetProdutoByNomeAsync("Produto Já Cadastrado")).ReturnsAsync(new ProdutoAggregate());

            var ex = Assert.ThrowsAsync<ProdutoJaCadastradoException>(async () =>
                await _produtoUseCases.CadastraProdutoAsync(cadastraProdutoDto));
            Assert.That(ex.Message, Is.EqualTo("Produto ja cadastrado."));
        }

        [Test]
        public async Task RemoveProdutoAsync_ProdutoNaoEncontrado_ThrowsProdutoNaoCadastradoException()
        {
            int produtoId = 1;
            _mockProdutoPersistenceGateway.Setup(x => x.GetProdutoByIdAsync(produtoId)).ReturnsAsync((ProdutoAggregate)null);

            var ex = Assert.ThrowsAsync<ProdutoNaoCadastradoException>(async () =>
                await _produtoUseCases.RemoveProdutoAsync(produtoId));
            Assert.That(ex.Message, Is.EqualTo("Produto nao encontrado."));
        }

        [Test]
        public async Task AtualizaProdutoAsync_ProdutoNaoEncontrado_ThrowsProdutoNaoCadastradoException()
        {
            int produtoId = 1;
            var atualizaProdutoDto = new AtualizaProdutoDto { Nome = "Produto Atualizado", Preco = 30.0, Categoria = Categoria.Lanche };
            _mockProdutoPersistenceGateway.Setup(x => x.GetProdutoByIdAsync(produtoId)).ReturnsAsync((ProdutoAggregate)null);

            var ex = Assert.ThrowsAsync<ProdutoNaoCadastradoException>(async () =>
                await _produtoUseCases.AtualizaProdutoAsync(produtoId, atualizaProdutoDto));
            Assert.That(ex.Message, Is.EqualTo("Produto nao encontrado."));
        }

        [Test]
        public async Task AtualizaProdutoAsync_ValidUpdate_ReturnsUpdatedProdutoDto()
        {
            int produtoId = 1;
            var produtoExistente = new ProdutoAggregate { Id = produtoId, Nome = "Produto Existente", Preco = new Preco(10.0), Categoria = Categoria.Lanche };
            var atualizaProdutoDto = new AtualizaProdutoDto { Nome = "Produto Atualizado", Preco = 30.0, Categoria = Categoria.Bebida };

            _mockProdutoPersistenceGateway.Setup(x => x.GetProdutoByIdAsync(produtoId)).ReturnsAsync(produtoExistente);
            _mockProdutoPersistenceGateway.Setup(x => x.UpdateProdutoAsync(It.IsAny<ProdutoAggregate>())).ReturnsAsync(true);

            var result = await _produtoUseCases.AtualizaProdutoAsync(produtoId, atualizaProdutoDto);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Nome, Is.EqualTo("Produto Atualizado"));
        }

        [Test]
        public async Task CadastraProdutoAsync_ProdutoValido_ProdutoCadastradoComSucesso()
        {
            var cadastraProdutoDto = new CadastraProdutoDto { Nome = "Produto Novo", Preco = 20.0, Categoria = Categoria.Lanche };
            _mockProdutoPersistenceGateway.Setup(x => x.GetProdutoByNomeAsync("Produto Novo")).ReturnsAsync((ProdutoAggregate)null);
            _mockProdutoPersistenceGateway.Setup(x => x.SaveProdutoAsync(It.IsAny<ProdutoAggregate>())).ReturnsAsync(true);

            await _produtoUseCases.CadastraProdutoAsync(cadastraProdutoDto);

            _mockProdutoPersistenceGateway.Verify(x => x.SaveProdutoAsync(It.IsAny<ProdutoAggregate>()), Times.Once);
        }
    }
}