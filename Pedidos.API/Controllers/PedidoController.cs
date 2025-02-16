using Microsoft.AspNetCore.Mvc;
using Pedidos.Infrastructure.Broker;
using Pedidos.UseCases.DTOs;
using Pedidos.UseCases.Exceptions.Produto;
using Pedidos.UseCases.Interfaces;
using System.Text.Json;

namespace ControleDePedidos.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PedidoController : ControllerBase
    {
        private readonly IPedidoUseCases PedidoUseCase;     
        private readonly IBrokerPublisher BrokerPublisher;
        private readonly string Exchange = "pedidosOperations";
        public PedidoController(IPedidoUseCases pedidoUseCase, IBrokerPublisher brokerPublisher)
        {
            PedidoUseCase = pedidoUseCase;
            BrokerPublisher = brokerPublisher;
       
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RealizaPedido([FromBody] CriaPedidoDto pedidoDto)
        {
            if (pedidoDto == null) return BadRequest("Pedido nao pode ser nulo.");

            try
            {
                var pedidoRealizadoDto = await PedidoUseCase.RealizarPedidoAsync(pedidoDto);            
               
                BrokerPublisher.PublishMessage(Exchange, JsonSerializer.Serialize(pedidoRealizadoDto), "pedidoRealizado");

                return Ok();
            }
            catch (ProdutoNaoCadastradoException ex)
            {
                return BadRequest(ex.Message);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao processar a requisição, tente novamente mais tarde.");
            }
        }
    
        [HttpGet("todos")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllPedidos()
        {
            try
            {
                var pedidos = await PedidoUseCase.GetAllPedidosAsync();

                return Ok(pedidos);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao processar a requisição, tente novamente mais tarde.");
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPedidoById(Guid id)
        {
            try
            {
                var pedidos = await PedidoUseCase.GetPedidoByIdAsync(id);

                return Ok(pedidos);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao processar a requisição, tente novamente mais tarde.");
            }
        }
    }
}
