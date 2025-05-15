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
        private readonly IService<AtendimentoRequestContract, AtendimentoResponseContract, long> _atendimentoService;

        public AtendimentoController(
            IService<AtendimentoRequestContract, AtendimentoResponseContract, long> atendimentoService)
        {
            _atendimentoService = atendimentoService;
        }

        [HttpPost]
        [Authorize]
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
        [Authorize]
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
        [Authorize]
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
        [Authorize]
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
    }
}