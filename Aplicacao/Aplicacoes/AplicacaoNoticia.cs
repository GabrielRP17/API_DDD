using Aplicacao.Interfaces;
using Dominio.Interfaces;
using Dominio.Interfaces.InterfacesServicos;
using Entidades.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.Aplicacoes
{
	public class AplicacaoNoticia : IAplicacaoNoticia
	{
		INoticia _INoticia;
		IServicoNoticia _IServicoNoticia;

		public AplicacaoNoticia(INoticia INoticia, IServicoNoticia IServicoNoticia)
		{ 
			_INoticia = INoticia;
			_IServicoNoticia = IServicoNoticia;
		}

		public async Task AdicionarNoticia(Noticia noticia)
		{
			await _IServicoNoticia.AdicionarNoticia(noticia);
		}


		public async Task AtualizaNoticia(Noticia noticia)
		{
			await _IServicoNoticia.AtualizaNoticia(noticia);
		}

		public async Task<List<Noticia>> ListarNoticiasAtivas()
		{
			return await _IServicoNoticia.ListarNoticiasAtivas();
		}





		public async Task Adicionar(Noticia obj)
		{
			await _INoticia.Adicionar(obj);
		}

		public async Task Atualizar(Noticia obj)
		{
			await _INoticia.Atualizar(obj);
		}

		public async Task<Noticia> BuscarPorId(int id)
		{
			return await _INoticia.BuscarPorId(id);
		}

		public async Task Excluir(Noticia obj)
		{
			await _INoticia.Excluir(obj);
		}

		public async Task<List<Noticia>> Listar()
		{
			return await _INoticia.Listar();
		}


	}
}
