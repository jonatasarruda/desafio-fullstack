using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DesafioFullstack.Api.Contract.Atendimentos;
using DesafioFullstack.Api.Domain.Models;

namespace DesafioFullstack.Api.Domain.Services.Interfaces
{
    public interface IAtendimentoService : IService<AtendimentoRequestContract, AtendimentoResponseContract, long>
    {
        Task<IEnumerable<AtendimentoResponseContract?>> ObterPorCliente(long clienteId);
        Task<IEnumerable<AtendimentoResponseContract?>> ObterPorUsuario(long usuarioId);
        Task<IEnumerable<AtendimentoResponseContract?>> ObterPorData(DateTime dataInicial, DateTime dataFinal);

    }
}