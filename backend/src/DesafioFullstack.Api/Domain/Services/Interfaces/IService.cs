using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DesafioFullstack.Api.Domain.Services.Interfaces
{
    /// <summary>
    /// Interface generica para criação de serviços.
    /// </summary>
    /// <typeparam name="RQ">Contrato de request</typeparam>
    /// <typeparam name="RS">Contrato de response</typeparam>
    /// <typeparam name="I">Tipo de identificador</typeparam>
    public interface IService<RQ, RS, I> where RQ : class
    {
        Task<IEnumerable<RS>> Obter(I idUsuario);
        Task<RS> Obter(I id, I idUsuario);
        Task<RS> Adicionar(RQ entidade);
        Task<RS> Atualizar(I id, RQ entidade, I idUsuario);
        Task Inativar(I id, I idUsuario);
    
    
    }
}