using Aplicacao.Interfaces;
using Entidades.Entidades;
using Entidades.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Identity.Client;
using System.Text;
using WebAPI.Models;
using WebAPI.Token;

namespace WebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UsuarioController : ControllerBase
	{
		private readonly IAplicacaoUsuario _IAplicacaoUsuario;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;


		public UsuarioController (IAplicacaoUsuario iAplicacaoUsuario, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
		{
			_IAplicacaoUsuario = iAplicacaoUsuario;
			_signInManager = signInManager;
			_userManager = userManager;
		}

		[AllowAnonymous]
		[Produces("application/json")]
		[HttpPost("/api/CriarToken")]
		public async Task<IActionResult> CriarToken([FromBody] Login login)
		{
			if(string.IsNullOrWhiteSpace(login.email) || string.IsNullOrWhiteSpace(login.senha))
				return Unauthorized();

			var resultado = await _IAplicacaoUsuario.ExisteUsuario(login.email, login.senha);
			if (resultado)
			{
				var token = new TokenJWTBuilder()
					.AddSecurityKey(JwtSecurityKey.Create("Secret_Key-12345678"))
					.AddSubject("Empresa - Generica")
					.AddIssuer("Teste.Securiry.Bearer")
					.AddAudience("Teste.Securiry.Bearer")
					.AddClaim("UsuarioAPINumero", "1")
					.AddExpiry(5)
					.Builder();

				return Ok(token.value);
			}
			else
			{
				return Unauthorized();
			}

		}

		[AllowAnonymous]
		[Produces("application/json")]
		[HttpPost("/api/AdicionarUsuario")]
		public async Task<IActionResult> AdicionarUsuario([FromBody] Login login)
		{
			if (string.IsNullOrWhiteSpace(login.email) || string.IsNullOrWhiteSpace(login.senha))
				return Ok("Falta alguns dados");

			var resultado = await
				_IAplicacaoUsuario.AdicionarUsuario(login.email, login.senha, login.idade, login.celular);

			if (resultado)
				return Ok("Usuario Adicionado com Sucesso");
			else
				return Ok("Erro ao adicionar usuário");
		}


		[AllowAnonymous]
		[Produces("application/json")]
		[HttpPost("/api/CriarTokenIdentity")]
		public async Task<IActionResult> CriarTokenIdentity([FromBody] Login login)
		{
			if (string.IsNullOrWhiteSpace(login.email) || string.IsNullOrWhiteSpace(login.senha))
				return Unauthorized();


			var resultado = await
				_signInManager.PasswordSignInAsync(login.email, login.senha, false, lockoutOnFailure: false);

			if (resultado.Succeeded)
			{
				var idUsuario = await _IAplicacaoUsuario.RetornaUsuario(login.email);

				var token = new TokenJWTBuilder()
					.AddSecurityKey(JwtSecurityKey.Create("Secret_Key-12345678"))
					.AddSubject("Empresa - Generica")
					.AddIssuer("Teste.Securiry.Bearer")
					.AddAudience("Teste.Securiry.Bearer")
					.AddClaim("idUsuario", idUsuario)
					.AddExpiry(5)
					.Builder();

				return Ok(token.value);
			}
			else
			{
				return Unauthorized();
			}

		}

		[AllowAnonymous]
		[Produces("application/json")]
		[HttpPost("/api/AdicionarUsuarioIdentity")]
		public async Task<IActionResult> AdicionarUsuarioIdentity([FromBody] Login login)
		{
			if (string.IsNullOrWhiteSpace(login.email) || string.IsNullOrWhiteSpace(login.senha))
				return Ok("Falta alguns dados");

			var user = new ApplicationUser
			{
				UserName = login.email,
				Email = login.email,
				Celular = login.celular,
				Tipo = TipoUsuario.Comum,
			};
			var resultado = await _userManager.CreateAsync(user, login.senha);

			if (resultado.Errors.Any())
			{
				return Ok(resultado.Errors);
			}

			//Geração de Confirmação caso precise
			var userId = await _userManager.GetUserIdAsync(user);
			var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
			code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

			//Retorno Email
			code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
			var resultado2 = await _userManager.CreateAsync(user, code);

			if (resultado2.Succeeded)
				return Ok("Usuario adicionado com sucesso");
			else
				return Ok("Erro ao confirmar usuarios");

		}

	}
}
