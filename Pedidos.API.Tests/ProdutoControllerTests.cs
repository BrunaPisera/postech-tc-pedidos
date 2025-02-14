using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Pedidos.API.Controllers;
using Pedidos.Core.Entities.Enums;
using Pedidos.UseCases.DTOs;
using Pedidos.UseCases.Exceptions.Produto;
using Pedidos.UseCases.Interfaces;

namespace Produto.API.Tests.Controllers
{
    [TestFixture]
    public class ProdutoControllerTests
    {
        private Mock<IProdutoUseCases> _mockProdutoApplication;
        private ProdutoController _controller;

        [SetUp]
        public void Setup()
        {
            _mockProdutoApplication = new Mock<IProdutoUseCases>();
            _controller = new ProdutoController(_mockProdutoApplication.Object);
        }

        [TearDown]
        public void Teardown()
        {
            _controller?.Dispose();
            _controller = null;
        }

        public void Dispose()
        {
            _controller?.Dispose();
        }

        [Test]
        public async Task CadastraProduto_DeveRetornarOk()
        {
            var produtoDto = new CadastraProdutoDto() { 
                Nome = "Bruna"
            };

            _mockProdutoApplication.Setup(x => x.CadastraProdutoAsync(produtoDto)).Returns(Task.CompletedTask);

            var result = await _controller.CadastraProduto(produtoDto);

            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
            Assert.That(okResult.Value, Is.EqualTo(produtoDto));
        }

        [Test]
        public async Task CadastraProduto_DeveRetornarBadRequest_QuandoCadastrarProdutoException()
        {
            var produtoDto = new CadastraProdutoDto()
            {
                Nome = "Bruna"
            };
            
            _mockProdutoApplication.Setup(x => x.CadastraProdutoAsync(produtoDto))
                .ThrowsAsync(new CadastrarProdutoException("Erro ao cadastrar produto"));

            var result = await _controller.CadastraProduto(produtoDto);

            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult, Is.Not.Null);
            Assert.That(badRequestResult.StatusCode, Is.EqualTo(StatusCodes.Status400BadRequest));
            Assert.That(badRequestResult.Value, Is.EqualTo("Erro ao cadastrar produto"));
        }

        [Test]
        public async Task CadastraProduto_DeveRetornarConflict_QuandoProdutoJaCadastradoException()
        {
            var produtoDto = new CadastraProdutoDto()
            {
                Nome = "Bruna"
            };
            
            _mockProdutoApplication.Setup(x => x.CadastraProdutoAsync(produtoDto))
                .ThrowsAsync(new ProdutoJaCadastradoException("Produto já cadastrado"));

            var result = await _controller.CadastraProduto(produtoDto);

            var conflictResult = result as ConflictObjectResult;
            Assert.That(conflictResult, Is.Not.Null);
            Assert.That(conflictResult.StatusCode, Is.EqualTo(StatusCodes.Status409Conflict));
            Assert.That(conflictResult.Value, Is.EqualTo("Produto já cadastrado"));
        }

        [Test]
        public async Task CadastraProduto_DeveRetornarInternalServerError_EmCasoDeErro()
        {
            var produtoDto = new CadastraProdutoDto() { Nome = "Bruna" };
            _mockProdutoApplication.Setup(x => x.CadastraProdutoAsync(produtoDto))
                .ThrowsAsync(new System.Exception());

            var result = await _controller.CadastraProduto(produtoDto);

            var internalServerErrorResult = result as ObjectResult;
            Assert.That(internalServerErrorResult, Is.Not.Null);
            Assert.That(internalServerErrorResult.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));
        }

        [Test]
        public async Task BuscaProdutos_DeveRetornarOk_QuandoExistiremProdutos()
        {
            var produtos = new List<ProdutoDto> { new ProdutoDto() { Nome = "Bruna" } };

            _mockProdutoApplication.Setup(x => x.BuscaProdutosAsync()).ReturnsAsync(produtos);

            var result = await _controller.BuscaProdutos();

            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
            Assert.That(okResult.Value, Is.EqualTo(produtos));
        }

        [Test]
        public async Task BuscaProdutos_DeveRetornarNotFound_QuandoNaoExistiremProdutos()
        {
            _mockProdutoApplication.Setup(x => x.BuscaProdutosAsync()).ReturnsAsync(new List<ProdutoDto>());

            var result = await _controller.BuscaProdutos();

            var notFoundResult = result as NotFoundResult;
            Assert.That(notFoundResult, Is.Not.Null);
            Assert.That(notFoundResult.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
        }

        [Test]
        public async Task BuscaProdutosPorCategoria_DeveRetornarOk_QuandoExistiremProdutos()
        {
            var categoria = Categoria.Acompanhamento;
            var produtos = new List<ProdutoDto> { new ProdutoDto() };
            _mockProdutoApplication.Setup(x => x.BuscaProdutosAsync(categoria)).ReturnsAsync(produtos);

            var result = await _controller.BuscaProdutosPorCategoria(categoria);

            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
            Assert.That(okResult.Value, Is.EqualTo(produtos));
        }

        [Test]
        public async Task RemoveProduto_DeveRetornarNoContent()
        {
            var id = 1;
            _mockProdutoApplication.Setup(x => x.RemoveProdutoAsync(id)).Returns(Task.CompletedTask);

            var result = await _controller.RemoveProduto(id);

            var noContentResult = result as NoContentResult;
            Assert.That(noContentResult, Is.Not.Null);
            Assert.That(noContentResult.StatusCode, Is.EqualTo(StatusCodes.Status204NoContent));
        }

        [Test]
        public async Task AtualizarProduto_DeveRetornarOk_QuandoProdutoAtualizado()
        {
            var id = 1;
            var produtoDto = new AtualizaProdutoDto() { Nome = "Bruna" };
            var produtoAtualizado = new ProdutoDto() { Nome = "Alexis" };

            _mockProdutoApplication.Setup(x => x.AtualizaProdutoAsync(id, produtoDto)).ReturnsAsync(produtoAtualizado);

            var result = await _controller.AtualizarProduto(id, produtoDto);

            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
            Assert.That(okResult.Value, Is.EqualTo(produtoAtualizado));
        }
    }
}
