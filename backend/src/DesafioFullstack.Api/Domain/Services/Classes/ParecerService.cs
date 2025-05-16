using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DesafioFullstack.Api.Contract.Pareceres;
using DesafioFullstack.Api.Domain.Models;
using DesafioFullstack.Api.Domain.Repositories.Interfaces;
using DesafioFullstack.Api.Domain.Services.Interfaces;

namespace DesafioFullstack.Api.Domain.Services.Classes
{
    public class ParecerService : IService<ParecerRequestContract, ParecerResponseContract, long>
    {
       private readonly IParecerRepository _parecerRepository;
        private readonly IMapper _mapper;

        public ParecerService(IParecerRepository parecerRepository, IMapper mapper)
        {
            _parecerRepository = parecerRepository;
            _mapper = mapper;
        }
        public async Task<ParecerResponseContract> Adicionar(ParecerRequestContract entidade)
        {
            var parecer = _mapper.Map<Parecer>(entidade);

            parecer.DataCadastro = DateTime.Now;

            parecer = await _parecerRepository.Adicionar(parecer);

            return _mapper.Map<ParecerResponseContract>(parecer);
        }

        public async Task<ParecerResponseContract> Atualizar(long id, ParecerRequestContract entidade, long idUsuario)
        {
            var parecerExistente = await _parecerRepository.Obter(id) ?? throw new NotImplementedException();

            var parecer = _mapper.Map<Parecer>(entidade);
            parecer.Id = id;
            parecer.DataCadastro = parecerExistente.DataCadastro;
            parecer = await _parecerRepository.Atualizar(parecer);

            return _mapper.Map<ParecerResponseContract>(parecer);
        }

        public Task Inativar(long id, long idUsuario)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ParecerResponseContract>> Obter(long idUsuario)
        {
            var pareceres = await _parecerRepository.Obter();
            return pareceres.Select(parecer => _mapper.Map<ParecerResponseContract>(parecer));
        }

        public async Task<ParecerResponseContract> Obter(long id, long idUsuario)
        {
            var parecer = await _parecerRepository.Obter(id);
            return _mapper.Map<ParecerResponseContract>(parecer);
        }
    }
}