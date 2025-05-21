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
    public class AtendimentoRepository : IAtendimentoRepository
    {
        private readonly ApplicationContext _contexto;

        public AtendimentoRepository(ApplicationContext contexto)
        {
            _contexto = contexto;
        }

        public async Task<Atendimento> Adicionar(Atendimento entidade)
        {
            await _contexto.Atendimentos.AddAsync(entidade);
            await _contexto.SaveChangesAsync();

            return entidade;
        }

        public async Task<Atendimento?> Atualizar(Atendimento entidade)
        {
            Atendimento? EntidadeBanco = await _contexto.Atendimentos.Where(x => x.Id == entidade.Id)
                                        .FirstOrDefaultAsync();

            if (EntidadeBanco == null)
                return null;

            _contexto.Entry(EntidadeBanco).CurrentValues.SetValues(entidade);
            await _contexto.SaveChangesAsync();

            return EntidadeBanco;
        }

        public async Task<bool> Inativar(Atendimento entidade)
        {
            Atendimento? EntidadeBanco = await _contexto.Atendimentos.Where(x => x.Id == entidade.Id)
                                    .FirstOrDefaultAsync();
            
            if(EntidadeBanco == null)
                return false;

            _contexto.Entry(EntidadeBanco).CurrentValues.SetValues(entidade.Status);
            await _contexto.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<Atendimento>> Obter()
        {
            return await _contexto.Atendimentos.OrderBy(u => u.Id)
                                                .Include(u => u.Pareceres)
                                                .ToListAsync();
                                                
        }

        public async Task<IEnumerable<Atendimento>> ObterPorCliente(long clienteId)
        {
            return await _contexto.Atendimentos.Where(x => x.ClienteId == clienteId)
                                                .OrderBy(x => x.ClienteId == clienteId)
                                                .Include(u => u.Pareceres)
                                                .ToListAsync();
        }
        
         public async Task<IEnumerable<Atendimento>> ObterPorUsuario(long usuarioId)
        {
            return await _contexto.Atendimentos.Where(x => x.UsuarioId == usuarioId)
                                                .OrderBy(x => x.UsuarioId == usuarioId)
                                                .Include(u => u.Pareceres)
                                                .ToListAsync();
        }

        public async Task<Atendimento?> Obter(long id)
        {
            return await _contexto.Atendimentos.Where(x => x.Id == id)
                                                   .Include(u => u.Pareceres)
                                                   .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Atendimento>> ObterPorData(DateTime dataInicial, DateTime dataFinal)
        {
             return await _contexto.Atendimentos.Where(x => x.DataCadastro >= dataInicial && x.DataCadastro <= dataFinal)
                                                .OrderBy(x => x.DataCadastro == dataInicial)
                                                .Include(u => u.Pareceres)
                                                .ToListAsync();
        }
    }
}