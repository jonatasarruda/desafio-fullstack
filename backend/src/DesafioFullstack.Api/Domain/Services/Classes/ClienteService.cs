using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DesafioFullstack.Api.Contract.Cliente;
using DesafioFullstack.Api.Domain.Models;
using DesafioFullstack.Api.Domain.Repositories.Interfaces;
using DesafioFullstack.Api.Domain.Services.Interfaces;

namespace DesafioFullstack.Api.Domain.Services.Classes
{
    public class ClienteService : IClienteService
    {

        private readonly IClienteRepository _clienteRepository;
        private readonly IMapper _mapper;

        public ClienteService(
            IClienteRepository clienteRepository,
            IMapper mapper)
        {
            _clienteRepository = clienteRepository;
            _mapper = mapper;
        }

        public async Task<ClienteResponseContract> Adicionar(ClienteRequestContract entidade)
        {
            var cliente = _mapper.Map<Cliente>(entidade);

            cliente.DataCadastro = DateTime.Now;

            cliente = await _clienteRepository.Adicionar(cliente);

            return _mapper.Map<ClienteResponseContract>(cliente);
        }

        public async Task<ClienteResponseContract> Atualizar(long id, ClienteRequestContract entidade, long idUsuario)
        {
            _ = await Obter(id) ?? throw new NotImplementedException();

            var cliente = _mapper.Map<Cliente>(entidade);

            cliente = await _clienteRepository.Atualizar(cliente);
            
            return _mapper.Map<ClienteResponseContract>(cliente);
        }

        public Task Inativar(long id, long idUsuario)
        {
            throw new NotImplementedException();
        }

        public async Task<ClienteResponseContract?> Obter(string nome)
        {
            var cliente = await _clienteRepository.Obter(nome);
            return _mapper.Map<ClienteResponseContract>(cliente);
        }

        public async Task<IEnumerable<ClienteResponseContract>> Obter(long idUsuario)
        {
             var clientes = await _clienteRepository.Obter();

            return clientes.Select(cliente => _mapper.Map<ClienteResponseContract>(cliente));
        }

        public async Task<ClienteResponseContract> Obter(long id, long idUsuario)
        {
            var cliente = await _clienteRepository.Obter(id);
            return _mapper.Map<ClienteResponseContract>(cliente);
        }
    }
}