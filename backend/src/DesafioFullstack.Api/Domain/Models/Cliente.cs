using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DesafioFullstack.Api.Domain.Models
{
    public class Cliente
    {
        [Key]
        public long Id { get; set; }
        [Required(ErrorMessage = "Campo de nome é obrigatório")]
        public string Nome { get; set; } = string.Empty;
        public DateTime DataCadastro { get; set; }
        public DateTime? DataInativacao { get; set; }
        public virtual ICollection<Atendimento> Atendimentos { get; set;}
    
    }
}