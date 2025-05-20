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

        [HttpGet]
        [AllowAnonymous]
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

        [HttpGet]
        [Route("{id:long}")]
        [AllowAnonymous]
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