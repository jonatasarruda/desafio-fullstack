using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DesafioFullstack.Api.Contract.Pareceres;

namespace DesafioFullstack.Api.Contract.Atendimentos
{
    public class AtendimentoResponseContract
    {
        public string TextoAberturaAtendimento { get; set; } = string.Empty;
        public long UsuarioId { get; set; }
        public long ClienteId { get; set; }
        public long Id { get; set; }
        public DateTime DataCadastro { get; set; }
        public string Status { get; set; } = string.Empty;
        public virtual ICollection<ParecerResponseContract> Pareceres { get; set; }
    }
}
