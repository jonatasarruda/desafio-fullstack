using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DesafioFullstack.Api.Contract.Atendimentos;
using DesafioFullstack.Api.Domain.Repositories.Interfaces;
using DesafioFullstack.Api.Domain.Services.Interfaces;
using DesafioFullstack.Api.Domain.Models;
using DesafioFullstack.Api.Domain.Repositories.Classes;

namespace DesafioFullstack.Api.Domain.Services.Classes
{
    public class AtendimentoService : IService<AtendimentoRequestContract, AtendimentoResponseContract, long>
    {
        private readonly IAtendimentoRepository _atendimentoRepository;
        private readonly IMapper _mapper;

        public AtendimentoService(IAtendimentoRepository atendimentoRepository, IMapper mapper)
        {
            _atendimentoRepository = atendimentoRepository;
            _mapper = mapper;
        }
        public async Task<AtendimentoResponseContract> Adicionar(AtendimentoRequestContract entidade)
        {
            var atendimento = _mapper.Map<Atendimento>(entidade);

            atendimento.DataCadastro = DateTime.Today;

            atendimento = await _atendimentoRepository.Adicionar(atendimento);

            return _mapper.Map<AtendimentoResponseContract>(atendimento);
        }

        public async Task<AtendimentoResponseContract> Atualizar(long id, AtendimentoRequestContract entidade, long idUsuario)
        {
            var atendimentoExistente = await _atendimentoRepository.Obter(id) ?? throw new NotImplementedException();

            var atendimento = _mapper.Map<Atendimento>(entidade);
            atendimento.Id = id;
            atendimento.DataCadastro = atendimentoExistente.DataCadastro;

            atendimento = await _atendimentoRepository.Atualizar(atendimento);

            return _mapper.Map<AtendimentoResponseContract>(atendimento);
        }

        public Task Inativar(long id, long idUsuario)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<AtendimentoResponseContract>> Obter(long idUsuario)
        {
            var atendimentos = await _atendimentoRepository.Obter();
            return atendimentos.Select(atendimento => _mapper.Map<AtendimentoResponseContract>(atendimento));
        }

        public async Task<AtendimentoResponseContract> Obter(long id, long idUsuario)
        {
            var atendimento = await _atendimentoRepository.Obter(id);
            return _mapper.Map<AtendimentoResponseContract>(atendimento);
        }
    }
}