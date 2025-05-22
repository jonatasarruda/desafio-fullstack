using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DesafioFullstack.Api.Contract.Atendimentos;
using DesafioFullstack.Api.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DesafioFullstack.Api.Controllers
{
    
    /// <summary>
    /// Controlador para gerenciar operações relacionadas a atendimentos.
    /// </summary>
    [ApiController]
    [Route("atendimentos")]
    public class AtendimentoController : BaseController
    {
        private readonly IAtendimentoService _atendimentoService;

        /// <summary>
        /// Construtor do AtendimentoController.
        /// </summary>
        /// <param name="atendimentoService">Serviço de atendimento injetado.</param>
        public AtendimentoController(
            IAtendimentoService atendimentoService)
        {
            _atendimentoService = atendimentoService;
        }

        /// <summary>
        /// Adiciona um novo atendimento.
        /// </summary>
        /// <param name="contrato">Dados do novo atendimento.</param>
        /// <returns>O atendimento recém-criado.</returns>
        /// <response code="201">Retorna o atendimento recém-criado.</response>
        /// <response code="400">Se os dados do atendimento forem inválidos.</response>
        /// <response code="401">Se o usuário não estiver autenticado.</response>
        /// <response code="500">Erro interno do servidor.</response>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Adicionar(AtendimentoRequestContract contrato)
        {
            try
            {
                return Created("", await _atendimentoService.Adicionar(contrato));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        /// <summary>
        /// Obtém todos os atendimentos.
        /// </summary>
        /// <returns>Lista de todos os atendimentos.</returns>
        /// <response code="200">Retorna a lista de atendimentos.</response>
        /// <response code="401">Se o usuário não estiver autenticado.</response>
        /// <response code="500">Erro interno do servidor.</response>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Obter()
        {
            try
            {
                _idUsuario = ObterIdUsuarioLogado();
                return Ok(await _atendimentoService.Obter(_idUsuario));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        /// <summary>
        /// Obtém um atendimento específico pelo ID.
        /// </summary>
        /// <param name="id">ID do atendimento.</param>
        /// <returns>O atendimento correspondente ao ID.</returns>
        /// <response code="200">Retorna o atendimento.</response>
        /// <response code="401">Se o usuário não estiver autenticado.</response>
        /// <response code="404">Se o atendimento não for encontrado.</response>
        /// <response code="500">Erro interno do servidor.</response>
        [HttpGet]
        [Route("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Obter(long id)
        {
            try
            {
                _idUsuario = ObterIdUsuarioLogado();
                return Ok(await _atendimentoService.Obter(id, _idUsuario));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        /// <summary>
        /// Atualiza um atendimento existente.
        /// </summary>
        /// <param name="id">ID do atendimento a ser atualizado.</param>
        /// <param name="contrato">Novos dados do atendimento.</param>
        /// <returns>O atendimento atualizado.</returns>
        /// <response code="200">Retorna o atendimento atualizado.</response>
        /// <response code="400">Se os dados do atendimento forem inválidos.</response>
        /// <response code="401">Se o usuário não estiver autenticado.</response>
        /// <response code="404">Se o atendimento não for encontrado.</response>
        /// <response code="500">Erro interno do servidor.</response>
        [HttpPut]
        [Route("{id}")]
        [AllowAnonymous]

        public async Task<IActionResult> Atualizar(long id, AtendimentoRequestContract contrato)
        {
            try
            {
                _idUsuario = ObterIdUsuarioLogado();
                return Ok(await _atendimentoService.Atualizar(id, contrato, _idUsuario));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
        /// <summary>
        /// Obtém todos os atendimento específico pelo ID do cliente.
        /// </summary>
        /// <param name="idCliente">Id do cliente.</param>
        /// <returns>Lista de atendimentos do cliente.</returns>
        /// <response code="200">Retorna lista de atendimentos.</response>
        /// <response code="401">Se o usuário não estiver autenticado.</response>
        /// <response code="404">Se o cliente não for encontrado.</response>
        /// <response code="500">Erro interno do servidor.</response>
        [HttpGet]
        [Route("cliente/{idCliente}")]
        [AllowAnonymous]

        public async Task<IActionResult> ObterPorCliente(long idCliente)
        {
            try
            {
                _idUsuario = ObterIdUsuarioLogado();
                return Ok(await _atendimentoService.ObterPorCliente(idCliente));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
        /// <summary>
        /// Obtém todos os atendimento específico pelo ID do usuário.
        /// </summary>
        /// <param name="idUsuario">Id do usuário.</param>
        /// <returns>Lista de atendimentos do cliente.</returns>
        /// <response code="200">Retorna lista de atendimentos.</response>
        /// <response code="401">Se o usuário não estiver autenticado.</response>
        /// <response code="404">Se o usuário não for encontrado.</response>
        /// <response code="500">Erro interno do servidor.</response>
        [HttpGet]
        [Route("usuario/{idUsuario}")]
        [AllowAnonymous]

        public async Task<IActionResult> ObterPorUsuario(long idUsuario)
        {
            try
            {
                _idUsuario = ObterIdUsuarioLogado();
                return Ok(await _atendimentoService.ObterPorUsuario(idUsuario));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
        /// <summary>
        /// Obtém todos os atendimento baseado na data de cadastro.
        /// </summary>
        /// <param name="dataInicial">Data inicial.</param>
        /// <param name="dataFinal">Data final.</param>
        /// <returns>Lista de atendimentos do que estão com data de cadastro entre as datas passadas.</returns>
        /// <response code="200">Retorna lista de atendimentos.</response>
        /// <response code="401">Se o usuário não estiver autenticado.</response>
        /// <response code="500">Erro interno do servidor.</response>
        [HttpGet]
        [Route("{dataInicial}&&{dataFinal}")]
        [AllowAnonymous]

        public async Task<IActionResult> ObterPorData(DateTime dataInicial, DateTime dataFinal)
        {
            try
            {
                _idUsuario = ObterIdUsuarioLogado();
                return Ok(await _atendimentoService.ObterPorData(dataInicial, dataFinal));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}