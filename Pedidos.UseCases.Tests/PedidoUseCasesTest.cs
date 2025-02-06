using Moq;
using Pedidos.Core.Entities;
using Pedidos.Core.Entities.Enums;
using Pedidos.Core.Entities.ValueObjects;
using Pedidos.UseCases.DTOs;
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
    }
}
