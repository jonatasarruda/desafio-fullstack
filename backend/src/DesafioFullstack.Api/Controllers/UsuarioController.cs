using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;
using DesafioFullstack.Api.Contract.Usuario;
using DesafioFullstack.Api.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace DesafioFullstack.Api.Controllers
{
    /// <summary>
    /// Controlador para gerenciar operações relacionadas a usuários.
    /// </summary>
    [ApiController]
    [Route("usuarios")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        /// <summary>
        /// Construtor do UsuarioController.
        /// </summary>
        /// <param name="usuarioService">Serviço de usuário injetado.</param>
        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        /// <summary>
        /// Autentica um usuário e retorna um token de acesso.
        /// </summary>
        /// <param name="contrato">Dados de login do usuário.</param>
        /// <returns>Token de acesso se a autenticação for bem-sucedida.</returns>
        /// <response code="200">Retorna o token de acesso.</response>
        /// <response code="401">Credenciais inválidas.</response>
        /// <response code="500">Erro interno do servidor.</response>
        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(UsuarioLoginResponseContract), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Autenticar(UsuarioLoginRequestContract contrato)
        {
            try
            {
                return Ok(await _usuarioService.Autenticar(contrato));
            }
            catch (AuthenticationException ex)
            {
                return Unauthorized(new {StatusCode = 401, message = ex.Message});
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        /// <summary>
        /// Realiza o logout do usuário (simulado).
        /// </summary>
        /// <returns>Mensagem de sucesso do logout.</returns>
        /// <response code="200">Logout solicitado com sucesso.</response>
        [HttpPost("logout")]
        [Authorize] // Apenas usuários autenticados podem fazer logout
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
            public IActionResult Logout()
        {
            return Ok(new { message = "Logout solicitado com sucesso." });
        }

        /// <summary>
        /// Adiciona um novo usuário.
        /// </summary>
        /// <param name="contrato">Dados do novo usuário.</param>
        /// <returns>O usuário recém-criado.</returns>
        /// <response code="201">Retorna o usuário recém-criado.</response>
        /// <response code="400">Se os dados do usuário forem inválidos.</response>
        /// <response code="401">Se o usuário não estiver autenticado.</response>
        /// <response code="500">Erro interno do servidor.</response>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Adicionar(UsuarioRequestContract contrato)
        {
            try
            {
                return Created("", await _usuarioService.Adicionar(contrato));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        /// <summary>
        /// Obtém todos os usuários.
        /// </summary>
        /// <returns>Lista de todos os usuários.</returns>
        /// <response code="200">Retorna a lista de usuários.</response>
        /// <response code="401">Se o usuário não estiver autenticado.</response>
        /// <response code="500">Erro interno do servidor.</response>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Obter()
        {
            try
            {
                return Ok(await _usuarioService.Obter(0));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        /// <summary>
        /// Obtém um usuário específico pelo ID.
        /// </summary>
        /// <param name="id">ID do usuário.</param>
        /// <returns>O usuário correspondente ao ID.</returns>
        /// <response code="200">Retorna o usuário.</response>
        /// <response code="401">Se o usuário não estiver autenticado.</response>
        /// <response code="404">Se o usuário não for encontrado.</response>
        /// <response code="500">Erro interno do servidor.</response>
        [HttpGet]
        [Route("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(UsuarioResponseContract), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Obter(long id)
        {
            try
            {
                return Ok(await _usuarioService.Obter(id, 0));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        /// <summary>
        /// Atualiza um usuário existente.
        /// </summary>
        /// <param name="Id">ID do usuário a ser atualizado.</param>
        /// <param name="contrato">Novos dados do usuário.</param>
        /// <returns>O usuário atualizado.</returns>
        /// <response code="200">Retorna o usuário atualizado.</response>
        /// <response code="400">Se os dados do usuário forem inválidos.</response>
        /// <response code="401">Se o usuário não estiver autenticado.</response>
        /// <response code="404">Se o usuário não for encontrado.</response>
        /// <response code="500">Erro interno do servidor.</response>
        [HttpPut]
        [Route("{id}")]
        [Authorize]
        public async Task<IActionResult> Atualizar(long Id, UsuarioRequestContract contrato)
        {
            try
            {
                return Ok(await _usuarioService.Atualizar(Id, contrato, 0));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

    }
}