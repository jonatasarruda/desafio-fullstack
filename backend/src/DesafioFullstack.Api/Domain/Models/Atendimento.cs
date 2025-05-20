using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DesafioFullstack.Api.Domain.Models
{
    public class Atendimento
    {
        [Key]
        public long Id { get; set; }     
        [Required(ErrorMessage = "Informe o texto de abertura do atendimento")]
        public string TextoAberturaAtendimento { get; set;} = string.Empty;
        public DateTime DataCadastro { get; set; }
        [Required(ErrorMessage = "Informe o usuário do atendimento")]
        public long UsuarioId { get; set; }
        [ForeignKey("UsuarioId")]
        public Usuario Usuario { get; set; }
        [Required(ErrorMessage = "Informe o cliente do atendimento")]
        public long ClienteId { get; set; }
        [ForeignKey("ClienteId")]
        public Cliente Cliente { get; set; }
        public string Status { get; set; }
        
        public virtual ICollection<Parecer> Pareceres { get; set; }
        
    }
}