using Aplicacao.Interfaces;
using Entidades.Entidades;
using Entidades.Notificacoes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;

namespace WebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class NoticiaController : ControllerBase
	{

		private readonly IAplicacaoNoticia _IAplicacaoNoticia;

		public NoticiaController(IAplicacaoNoticia iAplicacaoNoticia)
		{
			_IAplicacaoNoticia = iAplicacaoNoticia;
		}

		[Authorize]
		[Produces("application/json")]
		[HttpPost("/api/ListarNoticias")]
		public async Task<List<Noticia>> ListarNoticias()
		{
			return await _IAplicacaoNoticia.ListarNoticiasAtivas();
		}

		[Authorize]
		[Produces("application/json")]
		[HttpPost("/api/AdicionarNoticia")]
		public async Task<List<Notifica>> AdicionarNoticia(NoticiaModel noticia)
		{
			var novaNoticia = new Noticia();
			novaNoticia.Titulo = noticia.Titulo;
			novaNoticia.Informacao = noticia.Informacao;
			novaNoticia.UserId = await RetornarUsuarioLogado();
			await _IAplicacaoNoticia.AdicionarNoticia(novaNoticia);

			return novaNoticia.Notificacoes;

		}

		[Authorize]
		[Produces("application/json")]
		[HttpPost("/api/AtualizaNoticia")]
		public async Task<List<Notifica>> AtualizaNoticia(NoticiaModel noticia)
		{
			var novaNoticia = await _IAplicacaoNoticia.BuscarPorId(noticia.IdNoticia);
			novaNoticia.Titulo = noticia.Titulo;
			novaNoticia.Informacao = noticia.Informacao;
			novaNoticia.UserId = await RetornarUsuarioLogado();
			await _IAplicacaoNoticia.AtualizaNoticia(novaNoticia);

			return novaNoticia.Notificacoes;

		}

		[Authorize]
		[Produces("application/json")]
		[HttpPost("/api/ExcluirNoticia")]
		public async Task<List<Notifica>> ExcluirNoticia(NoticiaModel noticia)
		{
			var novaNoticia = await _IAplicacaoNoticia.BuscarPorId(noticia.IdNoticia);
	
			await _IAplicacaoNoticia.Excluir(novaNoticia);

			return novaNoticia.Notificacoes;

		}

		[Authorize]
		[Produces("application/json")]
		[HttpPost("/api/BuscarPorId")]
		public async Task <Noticia> BuscarPorId(NoticiaModel noticia)
		{
			var novaNoticia = await _IAplicacaoNoticia.BuscarPorId(noticia.IdNoticia);

			await _IAplicacaoNoticia.Excluir(novaNoticia);

			return novaNoticia;

		}

		private async Task<string> RetornarUsuarioLogado()
		{
			if(User != null)
			{
				var idUsuario = User.FindFirst("IdUsuario");
				return idUsuario.Value;
			}
			else
			{
				return string.Empty;
			}
		}
	}
}
