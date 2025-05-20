using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DesafioFullstack.Api.Contract.Atendimentos
{
    public class AtendimentoRequestContract
    {
        public string TextoAberturaAtendimento { get; set; } = string.Empty;
        public long UsuarioId { get; set; }
        public long ClienteId { get; set; }
        public string Status { get; set; } = string.Empty;

    }

}
