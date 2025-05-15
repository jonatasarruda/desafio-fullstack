using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DesafioFullstack.Api.Domain.Models;

namespace DesafioFullstack.Api.Domain.Repositories.Interfaces
{
    public interface IClienteRepository : IRepository<Cliente, long>
    {
        Task<Cliente?> Obter(string nome);
    }

}