using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DesafioFullstack.Api.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DesafioFullstack.Api.Data.Mappings
{
    public class ParecerMap
    {
    public void Configure(EntityTypeBuilder<Parecer> builder)
        {
            builder.ToTable("Pareceres")
                .HasKey(x => x.Id);

            builder.Property(x => x.DescricaoParecer)
                .IsRequired()
                .HasColumnType("VARCHAR");
            
            builder.Property(x => x.DataCadastro)
                .IsRequired()
                .HasColumnType("TIMESTAMP");

            builder.HasOne(x => x.Atendimento)
                .WithMany(x => x.Pareceres)
                .HasForeignKey(x => x.AtendimentoId);

            builder.HasOne(p => p.Usuario)     
                .WithMany(u => u.Pareceres) 
                .HasForeignKey(p => p.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);



        }
    }
}