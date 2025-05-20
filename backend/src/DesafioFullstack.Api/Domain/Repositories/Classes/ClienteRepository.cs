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
    public class ClienteRepository : IClienteRepository
    {
        private readonly ApplicationContext _contexto;

        public ClienteRepository(ApplicationContext context)
        {
            _contexto = context;
        }
        public async Task<Cliente> Adicionar(Cliente entidade)
        {
            await _contexto.Clientes.AddAsync(entidade);
            await _contexto.SaveChangesAsync();

            return entidade;
        }

        public async Task<Cliente?> Atualizar(Cliente entidade)
        {
            Cliente? EntidadeBanco = _contexto.Clientes.Where(x => x.Id == entidade.Id)
                                    .FirstOrDefault();

            if (EntidadeBanco == null)
                return null;

            _contexto.Entry(EntidadeBanco).CurrentValues.SetValues(entidade);
            _contexto.SaveChanges();

            return EntidadeBanco;
        }

        public async Task<bool> Inativar(Cliente entidade)
        {
            Cliente? EntidadeBanco = await _contexto.Clientes.Where(x => x.Id == entidade.Id)
                                    .FirstOrDefaultAsync();

            if (EntidadeBanco == null) 
            return false;

            _contexto.Entry(EntidadeBanco).CurrentValues.SetValues(entidade.Ativo);
            await _contexto.SaveChangesAsync();

            return true;
        }

        public async Task<Cliente?> Obter(string nome)
        {
            return await _contexto.Clientes.Where(x => x.Nome == nome)
                                        .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Cliente>> Obter()
        {
            return await _contexto.Clientes.OrderBy(c => c.Id)
                                        .Include(x => x.Atendimentos)
                                        .ToListAsync();
        }

        public async Task<Cliente?> Obter(long id)
        {
            return await _contexto.Clientes.Where(x => x.Id == id)
                                        .FirstOrDefaultAsync();
        }
    }
}