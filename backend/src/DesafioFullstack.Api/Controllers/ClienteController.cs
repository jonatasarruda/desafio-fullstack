using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DesafioFullstack.Api.Contract.Cliente;
using DesafioFullstack.Api.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DesafioFullstack.Api.Controllers
{
    [ApiController]
    [Route("clientes")]
    public class ClienteController : BaseController
    {
        private readonly IClienteService _clienteService;

        public ClienteController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        /// <summary>
        /// Adiciona um novo cliente.
        /// </summary>
        /// <param name="contrato">Dados do novo cliente.</param>
        /// <returns>O cliente recém-criado.</returns>
        /// <response code="201">Retorna o cliente recém-criado.</response>
        /// <response code="400">Se os dados do cliente forem inválidos.</response>
        /// <response code="401">Se o usuário não estiver autenticado.</response>
        /// <response code="500">Erro interno do servidor.</response>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Adicionar(ClienteRequestContract contrato)
        {
            try
            {
                return Created("", await _clienteService.Adicionar(contrato));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        /// <summary>
        /// Obtém todos os clientes.
        /// </summary>
        /// <returns>Lista de todos os clientes.</returns>
        /// <response code="200">Retorna a lista de clientes.</response>
        /// <response code="401">Se o usuário não estiver autenticado.</response>
        /// <response code="500">Erro interno do servidor.</response>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Obter()
        {
            try
            {
                _idUsuario = ObterIdUsuarioLogado();
                return Ok(await _clienteService.Obter(_idUsuario));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        /// <summary>
        /// Obtém um cliente específico pelo ID.
        /// </summary>
        /// <param name="id">ID do cliente.</param>
        /// <returns>O cliente correspondente ao ID.</returns>
        /// <response code="200">Retorna o cliente.</response>
        /// <response code="401">Se o usuário não estiver autenticado.</response>
        /// <response code="404">Se o cliente não for encontrado.</response>
        /// <response code="500">Erro interno do servidor.</response>
        [HttpGet]
        [Route("{id:long}")]
        [Authorize]
        public async Task<IActionResult> Obter(long id)
        {
            try
            {
                _idUsuario = ObterIdUsuarioLogado();
                return Ok(await _clienteService.Obter(id, _idUsuario));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        /// <summary>
        /// Obtém um cliente específico pelo ID.
        /// </summary>
        /// <param name="nome">Nome do cliente.</param>
        /// <returns>O cliente correspondente ao ID.</returns>
        /// <response code="200">Retorna o cliente.</response>
        /// <response code="401">Se o usuário não estiver autenticado.</response>
        /// <response code="404">Se o cliente não for encontrado.</response>
        /// <response code="500">Erro interno do servidor.</response>
        [HttpGet]
        [Route("{nome}")]
        [Authorize]
        public async Task<IActionResult> ObterPorNome(string nome)
        {
            try
            {
                _idUsuario = ObterIdUsuarioLogado();
                return Ok(await _clienteService.Obter(nome));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        /// <summary>
        /// Atualiza um cliente existente.
        /// </summary>
        /// <param name="id">ID do cliente a ser atualizado.</param>
        /// <param name="contrato">Novos dados do cliente.</param>
        /// <returns>O cliente atualizado.</returns>
        /// <response code="200">Retorna o cliente atualizado.</response>
        /// <response code="400">Se os dados do cliente forem inválidos.</response>
        /// <response code="401">Se o usuário não estiver autenticado.</response>
        /// <response code="404">Se o cliente não for encontrado.</response>
        /// <response code="500">Erro interno do servidor.</response>
        [HttpPut]
        [Route("{id}")]
        [Authorize]
        public async Task<IActionResult> Atualizar(long id, ClienteRequestContract contrato)
        {
            try
            {
                _idUsuario = ObterIdUsuarioLogado();
                return Ok(await _clienteService.Atualizar(id, contrato, _idUsuario));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
 }