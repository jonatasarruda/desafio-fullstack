using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DesafioFullstack.Api.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DesafioFullstack.Api.Data.Mappings
{
    public class UsuarioMap : IEntityTypeConfiguration<Usuario>
    {
        
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("Usuario")
                .HasKey(x => x.Id);

            builder.Property(x => x.Nome)
                .IsRequired()
                .HasColumnType("VARCHAR");

            builder.Property(x => x.Email)
                .IsRequired()
                .HasColumnType("VARCHAR");

            builder.Property(x => x.Senha)
                .IsRequired()
                .HasColumnType("VARCHAR");

            builder.Property(x => x.DataCadastro)
                .IsRequired()
                .HasColumnType("TIMESTAMP");

            builder.Property(x => x.Ativo)
                .HasColumnType("BOOLEAN");
        }
}
}