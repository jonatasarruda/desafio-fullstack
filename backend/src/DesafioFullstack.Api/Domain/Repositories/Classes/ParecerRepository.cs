using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DesafioFullstack.Api.Data;
using DesafioFullstack.Api.Domain.Models;
using DesafioFullstack.Api.Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DesafioFullstack.Api.Domain.Repositories.Classes
{
    public class ParecerRepository : IParecerRepository
    {
        private readonly ApplicationContext _contexto;
        public ParecerRepository(ApplicationContext contexto)
        {
            _contexto = contexto;
        }

        
        public async Task<Parecer> Adicionar(Parecer entidade)
        {
            await _contexto.Pareceres.AddAsync(entidade);
            await _contexto.SaveChangesAsync();
            
            return entidade;
        }

        public async Task<Parecer?> Atualizar(Parecer entidade)
        {
            Parecer? EntidadeBanco = await _contexto.Pareceres.Where(x => x.Id == entidade.Id)
                                    .FirstOrDefaultAsync();
            
            if(EntidadeBanco == null)
                return null;

            _contexto.Entry(EntidadeBanco).CurrentValues.SetValues(entidade);
            await _contexto.SaveChangesAsync();

            return EntidadeBanco;
        }

        public async Task<bool> Inativar(Parecer entidade)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Parecer>> Obter()
        {
            throw new NotImplementedException();
        }

        public async Task<Parecer?> Obter(long id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Parecer>> ObterTodosPorAtendimento(long atendimentoId)
        {
            return await _contexto.Pareceres.Where(x => x.AtendimentoId == atendimentoId)
                                            .OrderBy(x => x.AtendimentoId == atendimentoId)
                                            .ToListAsync();
        }
    }
}