using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DesafioFullstack.Api.Domain.Models;

namespace DesafioFullstack.Api.Domain.Repositories.Interfaces
{
    public interface IParecerRepository : IRepository<Parecer, long>
    {
        Task<IEnumerable<Parecer>> ObterTodosPorAtendimento(long atendimentoId);
    }
}