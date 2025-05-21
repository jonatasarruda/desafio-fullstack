using DesafioFullstack.Api.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DesafioFullstack.Api.Data
{
    public class DataSeeder
    {
        private readonly ApplicationContext _context;

        public DataSeeder(ApplicationContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            // Verifica se já existem usuários, indicando que o seed possivelmente já foi executado.
            if (await _context.Usuarios.AnyAsync())
            {
                return; 
            }

            var usuarios = new List<Usuario>();
            var clientes = new List<Cliente>();
            var atendimentos = new List<Atendimento>();
            var pareceres = new List<Parecer>();

            // Seed Usuarios
            var adminUser = new Usuario
            {
                Nome = "Administrador",
                Email = "admin@example.com",
                Senha = SeedHelper.GerarHashSenha("admin123"), // Lembre-se de usar senhas seguras em produção
                DataCadastro = DateTime.UtcNow,
                Ativo = true
            };
            usuarios.Add(adminUser);

            var regularUser = new Usuario
            {
                Nome = "Usuário Comum",
                Email = "user@example.com",
                Senha = SeedHelper.GerarHashSenha("user123"),
                DataCadastro = DateTime.UtcNow,
                Ativo = true
            };
            usuarios.Add(regularUser);

            await _context.Usuarios.AddRangeAsync(usuarios);
            await _context.SaveChangesAsync(); // Salva para obter os IDs gerados

            // Seed Clientes
            var cliente1 = new Cliente
            {
                Nome = "João Silva",
                // CPF = "11111111111", // Considere validações e formatação de CPF
                // Telefone = "11999998888",
                DataCadastro = DateTime.UtcNow,
                Ativo = true
            };
            clientes.Add(cliente1);

            var cliente2 = new Cliente
            {
                Nome = "Maria Oliveira",
                // CPF = "22222222222",
                // Telefone = "21988887777",
                DataCadastro = DateTime.UtcNow,
                Ativo = true
            };
            clientes.Add(cliente2);
            
            await _context.Clientes.AddRangeAsync(clientes);
            await _context.SaveChangesAsync(); // Salva para obter os IDs gerados

            // Seed Atendimentos
            var atendimento1 = new Atendimento
            {
                ClienteId = cliente1.Id,
                UsuarioId = adminUser.Id,
                // Descricao = "Problema inicial reportado pelo cliente João Silva.",
                DataCadastro = DateTime.UtcNow,
                Status = "Aberto" // Ex: "Aberto", "Em Andamento", "Concluído"
            };
            atendimentos.Add(atendimento1);
            await _context.Atendimentos.AddRangeAsync(atendimentos);
            await _context.SaveChangesAsync(); // Salva para obter os IDs gerados

            // Seed Pareceres
            var parecer1 = new Parecer
            {
                AtendimentoId = atendimento1.Id,
                // Descricao = "Primeiro parecer sobre o atendimento do cliente João.",
                DataCadastro = DateTime.UtcNow.AddMinutes(5) // Um pouco depois do atendimento
            };
            pareceres.Add(parecer1);
            await _context.Pareceres.AddRangeAsync(pareceres);

            await _context.SaveChangesAsync();
        }
    }
}