using Pedidos.UseCases.Gateway;
using Pedidos.UseCases;
using Moq;
using Pedidos.UseCases.DTOs;
using Pedidos.Core.Entities;
using Pedidos.Core.Entities.Enums;
using Pedidos.Core.Entities.ValueObjects;

namespace Pedidos.BDD.Tests.StepDefinitions
{
    [Binding]
    public class PedidoStepDefinitions
    {
        private PedidoUseCase _pedidoUseCases { get; set; }

        private CriaPedidoDto _pedidoDto;       
        private ClienteAggregate _clienteAggregate;
        private new List<ProdutoAggregate> _produtoAggregateList;
        private Task<PedidoRealizadoDto> _result;

        private Mock<IPedidoPersistenceGateway> _pedidoPersistantGatewayMock { get; set; }
        private Mock<IClientePersistenceGateway> _clientePersistenceGatewayMock { get; set; }
        private Mock<IProdutoPersistenceGateway> _produtoPersistenceGatewayMock { get; set; }
        [BeforeScenario("pedido")]
        public void BeforeScenarioWithTag()
        {
            _pedidoPersistantGatewayMock = new Mock<IPedidoPersistenceGateway>();
            _clientePersistenceGatewayMock = new Mock<IClientePersistenceGateway>();
            _produtoPersistenceGatewayMock = new Mock<IProdutoPersistenceGateway>();


            _pedidoUseCases = new PedidoUseCase(_pedidoPersistantGatewayMock.Object,
                                                _clientePersistenceGatewayMock.Object,
                                                _produtoPersistenceGatewayMock.Object);

            _produtoAggregateList = new List<ProdutoAggregate>
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
            _clienteAggregate = new ClienteAggregate
            {
                CPF = new CPF("234.456.876-98"),
                Email = new Email("bruna@gmail.com"),
                Nome = "Joao Antonio"
            };
            _clientePersistenceGatewayMock
               .Setup(x => x.GetClienteByCPF(It.IsAny<string>()))
               .ReturnsAsync(_clienteAggregate);

            _produtoPersistenceGatewayMock
                .Setup(p => p.GetProdutosByIdsAsync(It.IsAny<IEnumerable<int>>()))
                .ReturnsAsync(_produtoAggregateList);

            _pedidoPersistantGatewayMock
                .Setup(x => x.SavePedidoAsync(It.IsAny<PedidoAggregate>()))
                .ReturnsAsync(true);
        }

        [Given("the customer has provided the order description")]
        public void GivenTheCustomerHasProvidedTheOrderDescription()
        {
            _pedidoDto = new CriaPedidoDto
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
        }

        [When("the customer creates the order")]
        public void WhenTheCustomerCreatesTheOrder()
        {
            _result = _pedidoUseCases.RealizarPedidoAsync(_pedidoDto);
        }

        [Then("the order should be saved in the system")]
        public void ThenTheOrderShouldBeSavedInTheSystem()
        {
            _pedidoPersistantGatewayMock.Verify(p => p.SavePedidoAsync(It.IsAny<PedidoAggregate>()), Times.Once);
        }
    }
}
