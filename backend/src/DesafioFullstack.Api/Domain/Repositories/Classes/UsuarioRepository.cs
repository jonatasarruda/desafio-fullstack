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
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly ApplicationContext _contexto;
        public UsuarioRepository(ApplicationContext context)
        {
            _contexto = context;
        }

        public async Task<Usuario> Adicionar(Usuario entidade)
        {
            await _contexto.Usuarios.AddAsync(entidade);
            await _contexto.SaveChangesAsync();
            
            return entidade;
        }

        public async Task<Usuario> Atualizar(Usuario entidade)
        {
            Usuario? EntidadeBanco = await _contexto.Usuarios.Where(x => x.Id == entidade.Id)
                                    .FirstOrDefaultAsync();
            
            if(EntidadeBanco == null)
                return null;

            _contexto.Entry(EntidadeBanco).CurrentValues.SetValues(entidade);
            await _contexto.SaveChangesAsync();

            return EntidadeBanco;

        }

        public async Task<bool> Inativar(Usuario entidade)
        {
            Usuario? EntidadeBanco = await _contexto.Usuarios.Where(x => x.Id == entidade.Id)
                                    .FirstOrDefaultAsync();
            
            if(EntidadeBanco == null)
                return false;

            _contexto.Entry(EntidadeBanco).CurrentValues.SetValues(entidade.DataInativacao);
            await _contexto.SaveChangesAsync();

            return true;
        }

        public async Task<Usuario?> Obter(string email)
        {
            return await _contexto.Usuarios.Where(x => x.Email == email)
                                    .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Usuario>> Obter()
        {
            return await _contexto.Usuarios.OrderBy(u => u.Id)
                                           .ToListAsync();
        }

        public async Task<Usuario?> Obter(long id)
        {
            return await _contexto.Usuarios.Where(x => x.Id  == id)
                                    .FirstOrDefaultAsync();
        }
    }
}