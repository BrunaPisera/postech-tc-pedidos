using Moq;
using Pedidos.Core.Entities;
using Pedidos.Core.Entities.ValueObjects;
using Pedidos.UseCases.DTOs;
using Pedidos.UseCases.Gateway;

namespace Pedidos.UseCases.Tests
{
    public class ClienteUseCasesTests
    {
        private ClienteUseCases _clienteUseCases {  get; set; }

        private Mock<IClientePersistenceGateway> _clientePersistenceGateway { get; set; }

        [SetUp]
        public void Setup()
        {
            _clientePersistenceGateway = new Mock<IClientePersistenceGateway>();

            _clienteUseCases = new ClienteUseCases(_clientePersistenceGateway.Object);
        }

        [Test]
        public void Can_Create()
        {
            Assert.That(_clienteUseCases, Is.Not.Null);
        }

        [Test]
        public async Task CadastraClienteAsync_ReturnsExcepetion_WhenClientDtio_InNull()
        {
            var exception = _clienteUseCases.CadastraClienteAsync(null);

            Assert.That("Ocorreu um erro ao cadastrar o cliente.", Is.EqualTo(exception.Exception.InnerException.Message));
        }

        [Test]
        public async Task CadastraClienteAsync_ReturnsExcepetion_WhenCliente_IsAlreadyCadastrado()
        {
            var cliente = new CadastraClienteDto
            {
                CPF = "234.543.345-98"
            };

            var clientesCadastradosMock = new ClienteAggregate
            {
                CPF = new CPF("234.543.345-98")
            };

            _clientePersistenceGateway
                .Setup(x => x.GetClienteByCPF(It.IsAny<string>()))
                .ReturnsAsync(clientesCadastradosMock);

            var exception = _clienteUseCases.CadastraClienteAsync(cliente);

            Assert.That("Cliente ja possui cadastro.", Is.EqualTo(exception.Exception.InnerException.Message));
        }

        [Test]
        public async Task CadastraClienteAsync_CadastraCliente()
        {
            var cliente = new CadastraClienteDto
            {
                CPF = "234.543.345-98",
                EnderecoEmail = "banana@gmail.com",
                Nome = "Jon Carlos"
            };
      
            _clientePersistenceGateway
                .Setup(x => x.SalvarClienteAsync(It.IsAny<ClienteAggregate>()))
                .ReturnsAsync(true);

            var result = _clienteUseCases.CadastraClienteAsync(cliente);

            _clientePersistenceGateway.Verify(p => p.SalvarClienteAsync(It.IsAny<ClienteAggregate>()), Times.Once);
        }

        [Test]
        public async Task CadastraClienteAsync_ReturnsExcepetion_WhenIsNotPossible_ToCadastarCliente()
        {
            var cliente = new CadastraClienteDto
            {
                CPF = "234.543.345-98",
                EnderecoEmail = "banana@gmail.com",
                Nome = "Jon Carlos"
            };

            _clientePersistenceGateway
                .Setup(x => x.SalvarClienteAsync(It.IsAny<ClienteAggregate>()))
                .ReturnsAsync(false);

            var exception = _clienteUseCases.CadastraClienteAsync(cliente);

            Assert.That("Ocorreu um erro ao cadastrar o cliente.", Is.EqualTo(exception.Exception.InnerException.Message));
        }
    }
}