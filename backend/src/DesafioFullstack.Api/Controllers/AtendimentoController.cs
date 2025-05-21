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
    [ApiController]
    [Route("atendimentos")]
    public class AtendimentoController : BaseController
    {
        private readonly IAtendimentoService _atendimentoService;

        public AtendimentoController(
            IAtendimentoService atendimentoService)
        {
            _atendimentoService = atendimentoService;
        }

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