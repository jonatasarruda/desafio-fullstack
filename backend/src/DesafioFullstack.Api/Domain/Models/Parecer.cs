using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DesafioFullstack.Api.Domain.Models
{
    public class Parecer
    {
        [Key]
        public long Id { get; set; }

        [Required(ErrorMessage = "Informe a descrição do parecer")]
        public string DescricaoParecer { get; set;} = string.Empty;
        public DateTime DataCadastro { get; set; }

        [Required(ErrorMessage = "Informe o usuário do parecer")]
        public Usuario Usuario { get; set; }

        public long UsuarioId { get; set; }

        [Required(ErrorMessage = "Informe o atendimento do parecer")]
        public long AtendimentoId { get; set; }
        [ForeignKey("AtendimentoId")]
        public Atendimento Atendimento { get; set; }
         
    }
}