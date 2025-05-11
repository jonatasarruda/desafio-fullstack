using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DesafioFullstack.Api.Contract.Usuario;
using DesafioFullstack.Api.Domain.Models;
using DesafioFullstack.Api.Domain.Repositories.Interfaces;
using DesafioFullstack.Api.Domain.Services.Interfaces;

namespace DesafioFullstack.Api.Domain.Services.Classes
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IMapper _mapper;

        public UsuarioService(IUsuarioRepository usuarioRepository, IMapper mapper)
        {
            _usuarioRepository = usuarioRepository;
            _mapper = mapper;
        }
        public async Task<UsuarioResponseContract> Adicionar(UsuarioRequestContract entidade, long idUsuario)
        {
            
            Usuario usuario = _mapper.Map<Usuario>(entidade);

            usuario.Senha = GeradorHashSenha(usuario.Senha);

            usuario = await _usuarioRepository.Adicionar(usuario);

            return _mapper.Map<UsuarioResponseContract>(usuario);
        }

        public async Task<UsuarioResponseContract> Atualizar(UsuarioRequestContract entidade, long id, long idUsuario)
        {
            _ = await Obter(id) ?? throw new ArgumentException("Usuario não encontrado para atualização");

            var usuario = _mapper.Map<Usuario>(entidade);
            usuario.Senha = GeradorHashSenha(usuario.Senha);

            usuario = await _usuarioRepository.Atualizar(usuario);
           
           return _mapper.Map<UsuarioResponseContract>(usuario);
        }

        public Task<UsuarioLoginResponseContract> Autenticar(UsuarioLoginRequestContract usuarioLoginRequestContract)
        {
            throw new NotImplementedException();
        }

        public async Task Inativar(long id, long idUsuario)
        {
            UsuarioResponseContract usuario = await Obter(id);

            if (usuario == null){
                throw new ArgumentException("Usuario não encontrado");
            }
            
            await _usuarioRepository.Inativar(_mapper.Map<Usuario>(usuario));
        }

        public async Task<IEnumerable<UsuarioResponseContract>> Obter()
        {
            IEnumerable<Usuario> usuarios = await _usuarioRepository.Obter(); 
            return _mapper.Map<IEnumerable<UsuarioResponseContract>>(usuarios);
        }

        public async Task<UsuarioResponseContract> Obter(long id)
        {
            Usuario usuario = await _usuarioRepository.Obter(id);

            return _mapper.Map<UsuarioResponseContract>(usuario);
        }

        public async Task<UsuarioResponseContract> Obter(string Email)
        {
            Usuario usuario = await _usuarioRepository.Obter(Email);

            return _mapper.Map<UsuarioResponseContract>(usuario);
        }

        private string GeradorHashSenha(string senha)
        {
            string hashSenha;

            using(SHA256 sha256 = SHA256.Create())
            {
                byte[] bytesSenha = Encoding.UTF8.GetBytes(senha);
                byte[] byteHashSenha = sha256.ComputeHash(bytesSenha);
                hashSenha = BitConverter.ToString(byteHashSenha).ToLowerInvariant();
                
            }

            return hashSenha;
            
        }
    }
}