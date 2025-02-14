using Moq;
using Pedidos.Core.Entities;
using Pedidos.Core.Entities.Enums;
using Pedidos.Core.Entities.ValueObjects;
using Pedidos.UseCases.DTOs;
using Pedidos.UseCases.Exceptions.Pedido;
using Pedidos.UseCases.Gateway;

namespace Pedidos.UseCases.Tests
{
    public class PedidoUseCasesTest
    {
        private PedidoUseCase _pedidoUseCases {  get; set; }

        private Mock<IPedidoPersistenceGateway> _pedidoPersistantGatewayMock { get; set; }
        private Mock<IClientePersistenceGateway> _clientePersistenceGatewayMock { get; set; }
        private Mock<IProdutoPersistenceGateway> _produtoPersistenceGatewayMock { get; set; }

        [SetUp]
        public void Setup()
        {
            _pedidoPersistantGatewayMock = new Mock<IPedidoPersistenceGateway>();
            _clientePersistenceGatewayMock = new Mock<IClientePersistenceGateway>();
            _produtoPersistenceGatewayMock = new Mock<IProdutoPersistenceGateway>();


            _pedidoUseCases = new PedidoUseCase(_pedidoPersistantGatewayMock.Object,
                                                _clientePersistenceGatewayMock.Object,
                                                _produtoPersistenceGatewayMock.Object);
        }

        [Test]
        public void Can_Create()
        {
            Assert.That(_pedidoUseCases, Is.Not.Null);
        }

        [Test]
        public void RealizarPedidoAsync_ReturnProdutoNaoCadastradoException_WhenExceptionIsThrown()
        {
            var pedidoDto = new CriaPedidoDto
            {
                CpfCliente = "",
                Itens = new List<CriaItemPedidoDto>()
            };      

            _produtoPersistenceGatewayMock
                .Setup(p => p.GetProdutosByIdsAsync(It.IsAny<IEnumerable<int>>()))
                .ReturnsAsync(new List<ProdutoAggregate>());

            var result = _pedidoUseCases.RealizarPedidoAsync(pedidoDto);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Exception.InnerException.Message, Is.EqualTo("Um ou mais produtos do pedido nao foram encontrados. Verifique o pedido."));
        }

        [Test]
        public void RealizarPedidoAsync_ReturnProdutoNaoCadastradoException()
        {
            var pedidoDto = new CriaPedidoDto
            {
                CpfCliente = "",
                Itens = new List<CriaItemPedidoDto>()
                {
                    new CriaItemPedidoDto
                    {
                        IdProduto = 2,
                        Quantidade = 1,
                        Customizacao = "Sem cebola"
                    }
                }
            };

            var produtoAggregateMock = new List<ProdutoAggregate>
            {
                new ProdutoAggregate
                {
                    Id = 2,
                    Nome = "X-tudo",
                    Preco = new Preco(10),
                    Categoria = Categoria.Lanche,
                },
                new ProdutoAggregate
                {
                    Id = 3,
                    Nome = "X-salada",
                    Preco = new Preco(20),
                    Categoria = Categoria.Lanche,
                }

            };

            _produtoPersistenceGatewayMock
                .Setup(p => p.GetProdutosByIdsAsync(It.IsAny<IEnumerable<int>>()))
                .ReturnsAsync(produtoAggregateMock);

            _pedidoPersistantGatewayMock
                .Setup(x => x.SavePedidoAsync(It.IsAny<PedidoAggregate>()))
                .ReturnsAsync(false);

            var result = _pedidoUseCases.RealizarPedidoAsync(pedidoDto);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Exception.InnerException.Message, Is.EqualTo("Um ou mais produtos do pedido nao foram encontrados. Verifique o pedido."));
        }

        [Test]
        public void RealizarPedidoAsync_ReturnRealizarPedidoException_WhenExceptionIsThrown()
        {
            var pedidoDto = new CriaPedidoDto
            {
                CpfCliente = "",
                Itens = new List<CriaItemPedidoDto>() 
                {
                    new CriaItemPedidoDto
                    {
                        IdProduto = 2,
                        Quantidade = 1,
                        Customizacao = "Sem cebola"
                    }
                }
            };

            var produtoAggregateMock = new List<ProdutoAggregate>
            {
                new ProdutoAggregate
                {
                    Id = 2,
                    Nome = "X-tudo",
                    Preco = new Preco(10),
                    Categoria = Categoria.Lanche,
                }
            };

            _produtoPersistenceGatewayMock
                .Setup(p => p.GetProdutosByIdsAsync(It.IsAny<IEnumerable<int>>()))
                .ReturnsAsync(produtoAggregateMock);

            _pedidoPersistantGatewayMock
                .Setup(x => x.SavePedidoAsync(It.IsAny<PedidoAggregate>()))
                .ReturnsAsync(false);

            var result = _pedidoUseCases.RealizarPedidoAsync(pedidoDto);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Exception.InnerException.Message, Is.EqualTo("Ocorreu um erro ao realizar o pedido."));
        }

        [Test]
        public void RealizarPedidoAsync_ReturnPedidoRealizadoDto_WhenPedidoIsCreated()
        {
            var pedidoDto = new CriaPedidoDto
            {
                CpfCliente = "234.456.876-98",
                Itens = new List<CriaItemPedidoDto>
                {
                    new CriaItemPedidoDto 
                    { 
                        IdProduto = 2,
                        Quantidade = 1,
                        Customizacao = "Sem cebola"
                    },
                   new CriaItemPedidoDto
                    {
                        IdProduto = 3,
                        Quantidade = 1,
                        Customizacao = ""
                    },
                }
            };

            var clienteAggregateMock = new ClienteAggregate
            {
                CPF = new CPF("234.456.876-98"),
                Email = new Email("bruna@gmail.com"),
                Nome = "Joao Antonio"
            };

            var produtoAggregateMock = new List<ProdutoAggregate>
            {
                new ProdutoAggregate
                {
                    Id = 2,
                    Nome = "X-tudo",
                    Preco = new Preco(10),
                    Categoria = Categoria.Lanche,
                },
                new ProdutoAggregate
                {
                    Id = 3,
                    Nome = "X-salada",
                    Preco = new Preco(20),
                    Categoria = Categoria.Lanche,
                }

            };

            _clientePersistenceGatewayMock
                .Setup(x => x.GetClienteByCPF(It.IsAny<string>()))
                .ReturnsAsync(clienteAggregateMock);

            _produtoPersistenceGatewayMock
                .Setup(p => p.GetProdutosByIdsAsync(It.IsAny<IEnumerable<int>>()))
                .ReturnsAsync(produtoAggregateMock);

            _pedidoPersistantGatewayMock
                .Setup(x => x.SavePedidoAsync(It.IsAny<PedidoAggregate>()))
                .ReturnsAsync(true);

            var result = _pedidoUseCases.RealizarPedidoAsync(pedidoDto);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result.ClienteName, Is.EqualTo(clienteAggregateMock.Nome));
            _pedidoPersistantGatewayMock.Verify(p => p.SavePedidoAsync(It.IsAny<PedidoAggregate>()), Times.Once);
        }

        [Test]
        public async Task GetPedidoByIdAsync_ValidId_ReturnsPedidoDto()
        {
            var idPedido = Guid.NewGuid();
            var pedidoAggregate = new PedidoAggregate { Id = idPedido };

            _pedidoPersistantGatewayMock.Setup(x => x.GetPedidoById(idPedido)).ReturnsAsync(pedidoAggregate);

            var result = await _pedidoUseCases.GetPedidoByIdAsync(idPedido);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.IdPedido, Is.EqualTo(idPedido));
        }

        [Test]
        public void GetPedidoByIdAsync_InvalidId_ThrowsPedidoNaoEncontradoException()
        {
            var idPedido = Guid.NewGuid();

            _pedidoPersistantGatewayMock.Setup(x => x.GetPedidoById(idPedido)).ReturnsAsync((PedidoAggregate)null);

            Assert.ThrowsAsync<PedidoNaoEncontradoException>(async () =>
                await _pedidoUseCases.GetPedidoByIdAsync(idPedido));
        }

        [Test]
        public async Task GetAllPedidosAsync_ReturnsOrderedPedidos()
        {
            var pedidos = new List<PedidoAggregate>
            {
                new PedidoAggregate { Id = Guid.NewGuid(), HorarioRecebimento = DateTime.Now.AddHours(1) },
                new PedidoAggregate { Id = Guid.NewGuid(), HorarioRecebimento = DateTime.Now }
            };

            _pedidoPersistantGatewayMock.Setup(x => x.GetAllPedidos()).ReturnsAsync(pedidos);

            var result = await _pedidoUseCases.GetAllPedidosAsync();

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0].HorarioRecebimento, Is.LessThan(result[1].HorarioRecebimento));
        }

        [Test]
        public async Task ConfirmaPagamentoAsync_ValidId_ReturnsTrue()
        {
            var idPedido = Guid.NewGuid();
            var pedidoAggregate = new PedidoAggregate { Id = idPedido, PagamentoConfirmado = false };

            _pedidoPersistantGatewayMock.Setup(x => x.GetPedidoById(idPedido)).ReturnsAsync(pedidoAggregate);
            _pedidoPersistantGatewayMock.Setup(x => x.SavePedidoAsync(pedidoAggregate)).ReturnsAsync(true);

            var result = await _pedidoUseCases.ConfirmaPagamentoAsync(idPedido);

            Assert.That(result, Is.True);
            Assert.That(pedidoAggregate.PagamentoConfirmado, Is.True);
        }

        [Test]
        public void ConfirmaPagamentoAsync_InvalidId_ThrowsPedidoNaoEncontradoException()
        {
            var idPedido = Guid.NewGuid();

            _pedidoPersistantGatewayMock.Setup(x => x.GetPedidoById(idPedido)).ReturnsAsync((PedidoAggregate)null);

            Assert.ThrowsAsync<PedidoNaoEncontradoException>(async () =>
                await _pedidoUseCases.ConfirmaPagamentoAsync(idPedido));
        }

        [Test]
        public void GetPedidoByIdAsync_GuidEmpty_ThrowsPedidoNaoEncontradoException()
        {
            var idPedido = Guid.Empty;

            _pedidoPersistantGatewayMock.Setup(x => x.GetPedidoById(idPedido))
                                         .ReturnsAsync((PedidoAggregate)null);

            Assert.ThrowsAsync<PedidoNaoEncontradoException>(async () =>
                await _pedidoUseCases.GetPedidoByIdAsync(idPedido));
        }

        // Teste corrigido para ConfirmaPagamentoAsync - Guid.Empty
        [Test]
        public void ConfirmaPagamentoAsync_GuidEmpty_ThrowsPedidoNaoEncontradoException()
        {
            var idPedido = Guid.Empty;

            _pedidoPersistantGatewayMock.Setup(x => x.GetPedidoById(idPedido))
                                         .ReturnsAsync((PedidoAggregate)null);

            Assert.ThrowsAsync<PedidoNaoEncontradoException>(async () =>
                await _pedidoUseCases.ConfirmaPagamentoAsync(idPedido));
        }
    }
}
