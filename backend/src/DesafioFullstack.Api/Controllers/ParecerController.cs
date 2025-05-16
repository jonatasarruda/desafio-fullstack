using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DesafioFullstack.Api.Contract.Pareceres;
using DesafioFullstack.Api.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DesafioFullstack.Api.Controllers
{
    [ApiController]
    [Route("pareceres")]
    public class ParecerController : BaseController
    {
        private readonly IService<ParecerRequestContract, ParecerResponseContract, long> _parecerService;

        public ParecerController(IService<ParecerRequestContract, ParecerResponseContract, long> parecerService)
        {
            _parecerService = parecerService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Adicionar(ParecerRequestContract contrato)
        {
            try
            {
                return Created("", await _parecerService.Adicionar(contrato));
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
                return Ok(await _parecerService.Obter(_idUsuario));
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
                return Ok(await _parecerService.Obter(id, _idUsuario));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPut]
        [Route("{id}")]
        [Authorize]
        public async Task<IActionResult> Atualizar(long id, ParecerRequestContract contrato)
        {
            try
            {
                _idUsuario = ObterIdUsuarioLogado();
                return Ok(await _parecerService.Atualizar(id, contrato, _idUsuario));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}