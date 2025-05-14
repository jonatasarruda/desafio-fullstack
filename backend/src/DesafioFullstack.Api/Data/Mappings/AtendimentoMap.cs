using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DesafioFullstack.Api.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DesafioFullstack.Api.Data.Mappings
{
    public class AtendimentoMap
    {
             public void Configure(EntityTypeBuilder<Atendimento> builder)
        {
            builder.ToTable("Atendimento")
                .HasKey(x => x.Id);

            builder.Property(x => x.TextoAberturaAtendimento)
                .IsRequired()
                .HasColumnType("VARCHAR");

            builder.Property(x => x.DataCadastro)
                .IsRequired()
                .HasColumnType("TIMESTAMP");

            builder.Property(x => x.DataInativacao)
                .HasColumnType("TIMESTAMP");
            
            builder.HasOne(p => p.Cliente)     
                .WithMany(u => u.Atendimentos) 
                .HasForeignKey(p => p.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.HasOne(p => p.Usuario)     
                .WithMany(u => u.Atendimentos) 
                .HasForeignKey(p => p.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}