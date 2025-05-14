using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DesafioFullstack.Api.Contract.Pareceres
{
    public class ParecerResponseContract
    {
        public long Id { get; set; }
        public string DescricaoParecer { get; set; }
        public DateTime DataCadastro { get; set; }
        public long UsuarioId { get; set; }
        public long AtendimentoId { get; set; }
        
    }
}
