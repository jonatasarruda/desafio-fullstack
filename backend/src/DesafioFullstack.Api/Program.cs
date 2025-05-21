using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using DesafioFullstack.Api.Data;
using DesafioFullstack.Api.Domain.Models;
using DesafioFullstack.Api.Domain.Repositories.Classes;
using DesafioFullstack.Api.Domain.Repositories.Interfaces;
using DesafioFullstack.Api.Domain.Services.Interfaces;
using DesafioFullstack.Api.Domain.Services.Classes;
using Microsoft.EntityFrameworkCore;
using DesafioFullstack.Api.AutoMapper;
using DesafioFullstack.Api.Contract.Atendimentos;
using DesafioFullstack.Api.Contract.Pareceres;

var builder = WebApplication.CreateBuilder(args);

ConfigurarServices(builder);

ConfigurarInjecaoDeDependencia(builder);
builder.Services.AddScoped<DataSeeder>(); // Adiciona o DataSeeder

var app = builder.Build();

// Aplica migrações e executa o seed dos dados
await EnsureDatabaseCreatedAndSeeded(app);

ConfigurarAplicacao(app);

app.Run();

#region Métodos de Configuração

// Metodo que configrua as injeções de dependencia do projeto.
static void ConfigurarInjecaoDeDependencia(WebApplicationBuilder builder)
{
    string? connectionString = builder.Configuration.GetConnectionString("PADRAO");

    builder.Services.AddDbContext<ApplicationContext>(options =>
        options.UseNpgsql(connectionString), ServiceLifetime.Scoped); // Alterado para Scoped, que é mais comum para DbContext

    var config = new MapperConfiguration(cfg =>
    {
        cfg.AddProfile<UsuarioProfile>();
        cfg.AddProfile<ClienteProfile>();
        cfg.AddProfile<AtendimentoProfile>();
        cfg.AddProfile<ParecerProfile>();
    });

    IMapper mapper = config.CreateMapper();

    builder.Services
    .AddSingleton(builder.Configuration)
    .AddSingleton(builder.Environment)
    .AddSingleton(mapper)
    .AddScoped<TokenService>()
    .AddScoped<IUsuarioRepository, UsuarioRepository>()
    .AddScoped<IUsuarioService, UsuarioService>()
    .AddScoped<IClienteRepository, ClienteRepository>()
    .AddScoped<IClienteService, ClienteService>()
    .AddScoped<IAtendimentoRepository, AtendimentoRepository>()
    .AddScoped<IAtendimentoService, AtendimentoService>()
    .AddScoped<IParecerRepository, ParecerRepository>()
    .AddScoped<IService<ParecerRequestContract, ParecerResponseContract, long>, ParecerService>()
    
    ;

}

// Configura o serviços da API.
static void ConfigurarServices(WebApplicationBuilder builder)
{

    builder.Services
    .AddCors()
    .AddControllers().ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressModelStateInvalidFilter = true;
    }).AddNewtonsoftJson();

    builder.Services.AddSwaggerGen(c =>
    {



        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "JTW Authorization header using the Beaerer scheme (Example: 'Bearer 12345abcdef')",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });

        c.SwaggerDoc("v1", new OpenApiInfo { Title = "DesafioFullstack.Api", Version = "v1" });   
        
        // Configurar o Swagger para usar o arquivo XML gerado
        var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        if (File.Exists(xmlPath)) // Adicionar uma verificação se o arquivo existe é uma boa prática
        {
            c.IncludeXmlComments(xmlPath);
        }
    });

    builder.Services.AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })

    .AddJwtBearer(x =>
    {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["KeySecret"])),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });
}

// Configura os serviços na aplicação.
static void ConfigurarAplicacao(WebApplication app)
{
    // Configura o contexto do postgreSql para usar timestamp sem time zone.
    AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    app.UseDeveloperExceptionPage()
        .UseRouting();

    app.UseSwagger()
        .UseSwaggerUI(c =>
        {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "DesafioFullstack.Api v1");
                c.RoutePrefix = string.Empty;
        });

    app.UseCors(x => x
        .AllowAnyOrigin() // Permite todas as origens
        .AllowAnyMethod() // Permite todos os métodos
        .AllowAnyHeader()) // Permite todos os cabeçalhos
        .UseAuthentication();

    app.UseAuthorization();

    app.UseEndpoints(endpoints => endpoints.MapControllers());

    app.MapControllers();
}

static async Task EnsureDatabaseCreatedAndSeeded(WebApplication app)
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<ApplicationContext>();
            
            // Aplica quaisquer migrações pendentes.
            // É uma boa prática garantir que o schema do banco esteja atualizado antes do seed.
            await context.Database.MigrateAsync();

            // Executa o seed apenas em ambiente de desenvolvimento ou conforme necessidade
            if (app.Environment.IsDevelopment()) // Ou outra lógica de sua preferência
            {
                var seeder = services.GetRequiredService<DataSeeder>();
                await seeder.SeedAsync();
            }
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "Um erro ocorreu ao aplicar migrações ou ao popular o banco de dados (seed).");
            // Considere o que fazer em caso de erro: parar a aplicação, logar e continuar, etc.
        }
    }
}
#endregion
