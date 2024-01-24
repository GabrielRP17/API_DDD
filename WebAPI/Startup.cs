
using Aplicacao.Aplicacoes;
using Aplicacao.Interfaces;
using Dominio.Interfaces;
using Dominio.Interfaces.Genericos;
using Dominio.Interfaces.InterfacesServicos;
using Dominio.Servicos;
using Entidades.Entidades;
using Infraestrutura.Configuracoes;
using Infraestrutura.Repositorio;
using Infraestrutura.Repositorio.Genericos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebAPI.Token;

namespace WebAPI
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddDbContext<Contexto>(options => options.UseSqlServer(
				Configuration.GetConnectionString("DefaultConnection")));
			services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
				.AddEntityFrameworkStores<Contexto>();

			//Interface e Repositorio
			services.AddSingleton(typeof(IGenericos<>), typeof(RepositorioGenerico<>));
			services.AddSingleton<INoticia, RepositorioNoticia>();
			services.AddSingleton<IUsuario, RepositorioUsuario>();

			//Servico Dominio
			services.AddSingleton<IServicoNoticia, ServicoNoticia>();

			//Interface Aplicacao
			services.AddSingleton<IAplicacaoNoticia, AplicacaoNoticia>();
			services.AddSingleton<IAplicacaoUsuario, AplicacaoUsuario>();

			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
			.AddJwtBearer(option =>
			{
				option.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = false,
					ValidateAudience = false,
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,

					ValidIssuer = "Teste.Securiry.Bearer",
					ValidAudience = "Teste.Securiry.Bearer",
					IssuerSigningKey = JwtSecurityKey.Create("Secret_key-12345678")
				};

				option.Events = new JwtBearerEvents
				{
					OnAuthenticationFailed = context =>
					{
						Console.WriteLine("OnAuthenticationFailed: " + context.Exception.Message);
						return Task.CompletedTask;
					},
					OnTokenValidated = context =>
					{
						Console.WriteLine("OnTokenValidated: " + context.SecurityToken);
						return Task.CompletedTask;
					}
				};
			});



			services.AddControllers();
			services.AddEndpointsApiExplorer();
			services.AddSwaggerGen();

		}

		public void Configure(WebApplication app, IWebHostEnvironment env)
		{
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseAuthorization();
			app.UseAuthentication();

			app.MapControllers();
		}
	}
}
