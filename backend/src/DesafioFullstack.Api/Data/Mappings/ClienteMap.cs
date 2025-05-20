using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DesafioFullstack.Api.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DesafioFullstack.Api.Data.Mappings
{
    public class ClienteMap : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> builder)
        {
            builder.ToTable("Cliente")
                .HasKey(x => x.Id);

            builder.Property(x => x.Nome)
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