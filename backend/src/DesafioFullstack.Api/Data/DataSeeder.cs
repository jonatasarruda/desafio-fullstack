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
            if (await _context.Usuarios.AnyAsync() || await _context.Clientes.AnyAsync())
            {
                return; 
            }

            var random = new Random();
            var usuarios = new List<Usuario>();
            var clientes = new List<Cliente>();
            var atendimentos = new List<Atendimento>();
            var pareceres = new List<Parecer>();


            for (int i = 1; i <= 10; i++)
            {
                var user = new Usuario
                {
                    Nome = i == 1 ? "Administrador Master" : $"Usuário Padrão {i}",
                    Email = i == 1 ? "admin@admin.com" : $"user{i}@exemplo.com",
                    Senha = SeedHelper.GerarHashSenha(i == 1 ? "admin123" : $"user{i}pass"),
                    DataCadastro = DateTime.SpecifyKind(DateTime.UtcNow.AddDays(-random.Next(0, 30)), DateTimeKind.Unspecified),
                    Ativo = true
                };
                usuarios.Add(user);
            }

            await _context.Usuarios.AddRangeAsync(usuarios);
            await _context.SaveChangesAsync(); 


            for (int i = 1; i <= 10; i++)
            {
                var cliente = new Cliente
                {
                    Nome = $"Cliente Exemplo {i}",
                    DataCadastro = DateTime.SpecifyKind(DateTime.UtcNow.AddDays(-random.Next(0, 60)), DateTimeKind.Unspecified),
                    Ativo = true
                };
                clientes.Add(cliente);
            }

            await _context.Clientes.AddRangeAsync(clientes);
            await _context.SaveChangesAsync(); 

   
            var seededUsuarios = await _context.Usuarios.ToListAsync();
            var seededClientes = await _context.Clientes.ToListAsync();

            string[] statusPossiveis = { "aberto", "em_andamento", "concluido", "cancelado" };

            for (int i = 0; i < 10; i++)
            {

                var clienteParaAtendimento = seededClientes[random.Next(seededClientes.Count)];
                var usuarioParaAtendimento = seededUsuarios[random.Next(seededUsuarios.Count)];


                var dataAtendimentoBaseUtc = DateTime.UtcNow.AddDays(-random.Next(0, 61));
                var dataAtendimentoParaSalvar = DateTime.SpecifyKind(dataAtendimentoBaseUtc, DateTimeKind.Unspecified);

  
                if (dataAtendimentoParaSalvar < clienteParaAtendimento.DataCadastro)
                {
                    dataAtendimentoParaSalvar = clienteParaAtendimento.DataCadastro.AddHours(1); 
                }
                if (dataAtendimentoParaSalvar < usuarioParaAtendimento.DataCadastro)
                {
                    dataAtendimentoParaSalvar = usuarioParaAtendimento.DataCadastro.AddHours(1);
                }

                var atendimento = new Atendimento
                {
                    ClienteId = clienteParaAtendimento.Id,
                    UsuarioId = usuarioParaAtendimento.Id,
                    TextoAberturaAtendimento = $"Descrição do atendimento {i + 1} para o cliente {clienteParaAtendimento.Nome}.",
                    DataCadastro = dataAtendimentoParaSalvar,
                    Status = statusPossiveis[random.Next(statusPossiveis.Length)]
                };
                atendimentos.Add(atendimento);
            }

            await _context.Atendimentos.AddRangeAsync(atendimentos);
            await _context.SaveChangesAsync(); 

 
            var seededAtendimentos = await _context.Atendimentos.OrderBy(a => a.DataCadastro).ToListAsync();


            for (int i = 0; i < 10; i++)
            {
                var atendimentoParaParecer = seededAtendimentos[i % seededAtendimentos.Count];
                var usuarioParaParecer = seededUsuarios[random.Next(seededUsuarios.Count)];

                var dataParecer = atendimentoParaParecer.DataCadastro.AddMinutes(random.Next(5, 120)); 

                var parecer = new Parecer
                {
                    AtendimentoId = atendimentoParaParecer.Id,
                    UsuarioId = usuarioParaParecer.Id,
                    DescricaoParecer = $"Parecer {i + 1} sobre o atendimento ID {atendimentoParaParecer.Id}.",
                    DataCadastro = dataParecer
                };
                pareceres.Add(parecer);
            }
            await _context.Pareceres.AddRangeAsync(pareceres);

            await _context.SaveChangesAsync();
        }
    }
}