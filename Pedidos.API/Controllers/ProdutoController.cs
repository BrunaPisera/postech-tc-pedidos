﻿using Microsoft.AspNetCore.Mvc;
using Pedidos.Core.Entities.Enums;
using Pedidos.UseCases.DTOs;
using Pedidos.UseCases.Exceptions.Produto;
using Pedidos.UseCases.Interfaces;

namespace Pedidos.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProdutoController : Controller
    {
        private readonly IProdutoUseCases ProdutoApplication;

        public ProdutoController(IProdutoUseCases produtoApplication)
        {
            ProdutoApplication = produtoApplication;
        }

        [HttpPost]
        [Route("cadastro")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CadastraProduto([FromBody] CadastraProdutoDto produto)
        {
            try
            {
                await ProdutoApplication.CadastraProdutoAsync(produto);

                return Ok(produto);
            }
            catch (CadastrarProdutoException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ProdutoJaCadastradoException ex)
            {
                return Conflict(ex.Message);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao processar a requisição, tente novamente mais tarde.");
            }
        }

        [HttpGet]
        [Route("todos")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> BuscaProdutos()
        {
            try
            {
                var produtos = await ProdutoApplication.BuscaProdutosAsync();

                return produtos.Any() ? Ok(produtos) : NotFound();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao processar a requisição, tente novamente mais tarde.");
            }
        }

        [HttpGet]
        [Route("categoria/{categoria}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> BuscaProdutosPorCategoria([FromRoute] Categoria categoria)
        {
            try
            {
                var produtos = await ProdutoApplication.BuscaProdutosAsync(categoria);

                return produtos.Any() ? Ok(produtos) : NotFound();
            }
            catch (CategoriaInvalidaException ex)
            {
                return BadRequest(ex.Message);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao processar a requisição, tente novamente mais tarde.");
            }
        }

        [HttpDelete]
        [Route("remover/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RemoveProduto([FromRoute] int id)
        {
            try
            {
                if (id <= 0) return BadRequest("Id nao pode ser menor ou igual a zero.");

                await ProdutoApplication.RemoveProdutoAsync(id);

                return NoContent();
            }
            catch (ProdutoNaoCadastradoException ex)
            {
                return NotFound(ex.Message);
            }
            catch (RemoveProdutoException ex)
            {
                return BadRequest(ex.Message);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao processar a requisição, tente novamente mais tarde.");
            }
        }

        [HttpPut]
        [Route("atualizar/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AtualizarProduto([FromRoute] int id, [FromBody] AtualizaProdutoDto produto)
        {
            try
            {
                if (id <= 0) return BadRequest("Id nao pode ser menor ou igual a zero.");

                var produtoAtualizado = await ProdutoApplication.AtualizaProdutoAsync(id, produto);

                return Ok(produtoAtualizado);
            }
            catch (ProdutoNaoCadastradoException ex)
            {
                return NotFound(ex.Message);
            }
            catch (AtualizaProdutoException ex)
            {
                return BadRequest(ex.Message);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao processar a requisição, tente novamente mais tarde.");
            }
        }
    }
}
