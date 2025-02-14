using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ControleDePedidos.API.Controllers;
using Pedidos.Infrastructure.Broker;
using Pedidos.UseCases.DTOs;
using Pedidos.UseCases.Exceptions.Produto;
using Pedidos.UseCases.Interfaces;

namespace Pedidos.API.Tests
{
    [TestFixture]
    public class PedidoControllerTests
    {
        private Mock<IPedidoUseCases> _mockPedidoUseCase;
        private Mock<IBrokerPublisher> _mockBrokerPublisher;
        private PedidoController _controller;

        [SetUp]
        public void Setup()
        {
            _mockPedidoUseCase = new Mock<IPedidoUseCases>();
            _mockBrokerPublisher = new Mock<IBrokerPublisher>();
            _controller = new PedidoController(_mockPedidoUseCase.Object, _mockBrokerPublisher.Object);
        }

        [Test]
        public async Task RealizaPedido_DeveRetornarOk()
        {
            var pedidoDto = new CriaPedidoDto
            {
                Itens = new List<CriaItemPedidoDto>
                {
                    new CriaItemPedidoDto 
                    { 
                        IdProduto = 1,
                        Quantidade = 1,
                        Customizacao = null,
                    }
                }
            };
            var pedidoRealizadoDto = new PedidoRealizadoDto();
            _mockPedidoUseCase.Setup(x => x.RealizarPedidoAsync(pedidoDto))
                              .ReturnsAsync(pedidoRealizadoDto);

            var result = await _controller.RealizaPedido(pedidoDto);

            Assert.IsInstanceOf<OkResult>(result);
            _mockBrokerPublisher.Verify(x => x.PublishMessage(
                It.IsAny<string>(),
                It.IsAny<string>(),
                "create.pedido"), Times.Once);
        }

        [Test]
        public async Task RealizaPedido_DeveRetornarBadRequest_QuandoPedidoDtoForNulo()
        {
            var result = await _controller.RealizaPedido(null);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task RealizaPedido_DeveRetornarBadRequest_QuandoProdutoNaoCadastrado()
        {
            var pedidoDto = new CriaPedidoDto { Itens = new List<CriaItemPedidoDto> { } };
            _mockPedidoUseCase.Setup(x => x.RealizarPedidoAsync(pedidoDto))
                              .ThrowsAsync(new ProdutoNaoCadastradoException("Produto nao cadastrado."));

            var result = await _controller.RealizaPedido(pedidoDto);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task RealizaPedido_DeveRetornarInternalServerError_QuandoOcorrerErroGenerico()
        {
            var pedidoDto = new CriaPedidoDto { Itens = new List<CriaItemPedidoDto> { } };
            _mockPedidoUseCase.Setup(x => x.RealizarPedidoAsync(pedidoDto))
                              .ThrowsAsync(new Exception());

            var result = await _controller.RealizaPedido(pedidoDto) as ObjectResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));
        }

        [Test]
        public async Task GetAllPedidos_DeveRetornarOk_ComListaDePedidos()
        {
            var pedidos = new List<PedidoDto> { new PedidoDto() };
            _mockPedidoUseCase.Setup(x => x.GetAllPedidosAsync())
                              .ReturnsAsync(pedidos);

            var result = await _controller.GetAllPedidos() as OkObjectResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
            Assert.That(result.Value, Is.EqualTo(pedidos));
        }

        [Test]
        public async Task GetAllPedidos_DeveRetornarInternalServerError_QuandoOcorrerErro()
        {
            _mockPedidoUseCase.Setup(x => x.GetAllPedidosAsync())
                              .ThrowsAsync(new Exception());

            var result = await _controller.GetAllPedidos() as ObjectResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));
        }

        [Test]
        public async Task GetPedidoById_DeveRetornarOk_QuandoEncontrarPedido()
        {
            var pedido = new PedidoDto();
            _mockPedidoUseCase.Setup(x => x.GetPedidoByIdAsync(It.IsAny<Guid>()))
                              .ReturnsAsync(pedido);

            var result = await _controller.GetPedidoById(Guid.NewGuid()) as OkObjectResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
            Assert.That(result.Value, Is.EqualTo(pedido));
        }

        [Test]
        public async Task GetPedidoById_DeveRetornarInternalServerError_QuandoOcorrerErro()
        {
            _mockPedidoUseCase.Setup(x => x.GetPedidoByIdAsync(It.IsAny<Guid>()))
                              .ThrowsAsync(new Exception());

            var result = await _controller.GetPedidoById(Guid.NewGuid()) as ObjectResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));
        }
    }
}
