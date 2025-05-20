using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DesafioFullstack.Api.Contract.Atendimentos;

namespace DesafioFullstack.Api.Contract.Cliente
{
    public class ClienteResponseContract
    {
        public long Id { get; set; }
        public string Nome { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool Ativo { get; set; }
        public virtual ICollection<AtendimentoResponseContract> Atendimentos { get; set; }
         
    }
}
