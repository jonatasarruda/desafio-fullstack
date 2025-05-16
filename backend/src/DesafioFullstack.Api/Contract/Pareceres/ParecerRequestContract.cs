using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DesafioFullstack.Api.Contract.Pareceres
{
    public class ParecerRequestContract
    {
        public string DescricaoParecer { get; set; }
        public string AtendimentoId { get; set; }
        public string UsuarioId { get; set; }
    }
}