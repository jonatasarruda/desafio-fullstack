using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DesafioFullstack.Api.Domain.Models;

namespace DesafioFullstack.Api.Domain.Repositories.Interfaces
{
    public interface IAtendimentoRepository : IRepository<Atendimento, long>
    {
        public Task<IEnumerable<Atendimento>> ObterPorCliente(long ClienteId);
        public Task<IEnumerable<Atendimento>> ObterPorUsuario(long UsuarioId);
        public Task<IEnumerable<Atendimento>> ObterPorData(DateTime dataInicial, DateTime dataFinal);
    }
    
};