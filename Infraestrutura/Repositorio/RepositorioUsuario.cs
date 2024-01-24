using Dominio.Interfaces;
using Entidades.Entidades;
using Infraestrutura.Configuracoes;
using Infraestrutura.Repositorio.Genericos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestrutura.Repositorio
{
	public class RepositorioUsuario : RepositorioGenerico<ApplicationUser>, IUsuario
	{
		private readonly DbContextOptions<Contexto> _optionbuilder;

		public RepositorioUsuario()
		{
			_optionbuilder = new DbContextOptions<Contexto>();
		}

		public async Task<bool> AdicionarUsuario(string email, string senha, int idade, string celular)
		{
			try
			{
				using (var data = new Contexto(_optionbuilder))
				{
					await data.ApplicationUser.AddAsync(
						new ApplicationUser()
						{
							Email = email,
							PasswordHash = senha,
							Idade = idade,
							Celular = celular,
							Tipo = Entidades.Enums.TipoUsuario.Comum
						});

					await data.SaveChangesAsync();
				}
			}
			catch (Exception)
			{
				return false;
			}
			
			return true;
		}

		public async Task<bool> ExisteUsuario(string email, string senha)
		{
			try
			{
				using (var data = new Contexto(_optionbuilder))
				{
					return await data.ApplicationUser.
						Where(u => u.Email.Equals(email) && u.PasswordHash.Equals(senha))
						.AsNoTracking()
						.AnyAsync();
				}
			}
			catch (Exception)
			{
				return false;
			}
		}

		public async Task<string> RetornaUsuario(string email)
		{
			try
			{
				using (var data = new Contexto(_optionbuilder))
				{
					var usuario = await data.ApplicationUser.
						Where(u => u.Email.Equals(email))
						.AsNoTracking()
						.FirstOrDefaultAsync();

					return usuario.Id;
				}
			}
			catch (Exception)
			{
				return string.Empty;
			}
		}
	}
}
