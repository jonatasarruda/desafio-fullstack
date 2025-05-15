using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DesafioFullstack.Api.Contract.Cliente;

namespace DesafioFullstack.Api.Domain.Services.Interfaces
{
    public interface IClienteService : IService<ClienteRequestContract, ClienteResponseContract, long>
    {
        Task<ClienteResponseContract?> Obter(string nome);
    }
}