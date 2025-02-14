using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Pedidos.API.Controllers;
using Pedidos.UseCases.DTOs;
using Pedidos.UseCases.Exceptions.Cliente;
using Pedidos.UseCases.Interfaces;

namespace Pedidos.API.Tests
{
    [TestFixture]
    public class ClienteControllerTests
    {
        private Mock<IClienteUseCases> _mockClientApplication;
        private ClienteController _controller;

        [SetUp]
        public void Setup()
        {
            _mockClientApplication = new Mock<IClienteUseCases>();
            _controller = new ClienteController(_mockClientApplication.Object);
        }

        [Test]
        public async Task CadastraCliente_DeveRetornarOk()
        {
            var cadastraClienteDto = new CadastraClienteDto();
            _mockClientApplication.Setup(x => x.CadastraClienteAsync(cadastraClienteDto)).Returns(Task.CompletedTask);

            var result = await _controller.CadastraCliente(cadastraClienteDto);

            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
            Assert.That(okResult.Value, Is.EqualTo(cadastraClienteDto));
        }

        [Test]
        public async Task CadastraCliente_DeveRetornarBadRequest_QuandoCadastrarClienteException()
        {
            var cadastraClienteDto = new CadastraClienteDto();
            _mockClientApplication.Setup(x => x.CadastraClienteAsync(cadastraClienteDto))
                .ThrowsAsync(new CadastrarClienteException("Erro ao cadastrar cliente"));

            var result = await _controller.CadastraCliente(cadastraClienteDto);

            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult, Is.Not.Null);
            Assert.That(badRequestResult.StatusCode, Is.EqualTo(StatusCodes.Status400BadRequest));
            Assert.That(badRequestResult.Value, Is.EqualTo("Erro ao cadastrar cliente"));
        }

        [Test]
        public async Task CadastraCliente_DeveRetornarConflict_QuandoClienteJaCadastradoException()
        {
            var cadastraClienteDto = new CadastraClienteDto();
            _mockClientApplication.Setup(x => x.CadastraClienteAsync(cadastraClienteDto))
                .ThrowsAsync(new ClienteJaCadastradoException("Cliente já cadastrado"));

            var result = await _controller.CadastraCliente(cadastraClienteDto);

            var conflictResult = result as ConflictObjectResult;
            Assert.That(conflictResult, Is.Not.Null);
            Assert.That(conflictResult.StatusCode, Is.EqualTo(StatusCodes.Status409Conflict));
            Assert.That(conflictResult.Value, Is.EqualTo("Cliente já cadastrado"));
        }

        [Test]
        public async Task CadastraCliente_DeveRetornarInternalServerError_EmCasoDeErro()
        {
            var cadastraClienteDto = new CadastraClienteDto();
            _mockClientApplication.Setup(x => x.CadastraClienteAsync(cadastraClienteDto))
                .ThrowsAsync(new System.Exception());
            
            var result = await _controller.CadastraCliente(cadastraClienteDto);

            var internalServerErrorResult = result as ObjectResult;
            Assert.That(internalServerErrorResult, Is.Not.Null);
            Assert.That(internalServerErrorResult.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));
            Assert.That(internalServerErrorResult.Value, Is.EqualTo("Erro ao processar a requisicao, tente novamente mais tarde."));
        }

        [Test]
        public async Task BuscaCliente_DeveRetornarBadRequest_QuandoGetClienteByCpfException()
        {
            var cpf = "123456789";
            _mockClientApplication.Setup(x => x.GetClienteByCPFAsync(cpf))
                .ThrowsAsync(new GetClienteByCpfException("Erro ao buscar cliente"));

            var result = await _controller.BuscaCliente(cpf);

            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult, Is.Not.Null);
            Assert.That(badRequestResult.StatusCode, Is.EqualTo(StatusCodes.Status400BadRequest));
            Assert.That(badRequestResult.Value, Is.EqualTo("Erro ao buscar cliente"));
        }

        [Test]
        public async Task BuscaCliente_DeveRetornarInternalServerError_EmCasoDeErro()
        {
            var cpf = "123456789";
            _mockClientApplication.Setup(x => x.GetClienteByCPFAsync(cpf))
                .ThrowsAsync(new System.Exception());

            var result = await _controller.BuscaCliente(cpf);

            var internalServerErrorResult = result as ObjectResult;
            Assert.That(internalServerErrorResult, Is.Not.Null);
            Assert.That(internalServerErrorResult.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));
            Assert.That(internalServerErrorResult.Value, Is.EqualTo("Erro ao processar a requisi��o, tente novamente mais tarde."));

        }
    }
}
