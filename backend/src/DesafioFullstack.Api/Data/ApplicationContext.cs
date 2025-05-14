using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DesafioFullstack.Api.Domain.Models;
using DesafioFullstack.Api.Data.Mappings;

namespace DesafioFullstack.Api.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Atendimento> Atendimentos { get; set; }
        public DbSet<Parecer> Pareceres { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options){

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UsuarioMap());
            modelBuilder.ApplyConfiguration(new ClienteMap());
            modelBuilder.ApplyConfiguration(new ParecerMap());
            modelBuilder.ApplyConfiguration(new AtendimentoMap());

        }

    }
}