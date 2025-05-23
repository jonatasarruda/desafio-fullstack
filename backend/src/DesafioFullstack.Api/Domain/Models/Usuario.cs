using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DesafioFullstack.Api.Domain.Models
{
    public class Usuario
    {
        [Key]
        public long Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Campo de email é obrigatório")]
        public string Email { get; set; } = string.Empty;
        [Required(ErrorMessage = "Campo de senha é obrigatório")]
        public string Senha { get; set; } = string.Empty;
        public DateTime DataCadastro { get; set; }
        public bool Ativo { get; set; }
        public virtual ICollection<Atendimento> Atendimentos { get; set; }
        public virtual ICollection<Parecer> Pareceres { get; set; }
        
    }
}