using Dominio.Interfaces;
using Dominio.Interfaces.InterfacesServicos;
using Entidades.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Servicos
{
	public class ServicoNoticia : IServicoNoticia
	{
		private readonly INoticia _INoticia;

		public ServicoNoticia(INoticia iNoticia)
		{
			_INoticia = iNoticia;
		}

		public async Task AdicionarNoticia(Noticia noticia)
		{
			var validarTitulo = noticia.ValidarPropriedadeString(noticia.Titulo, "Titulo");
			var validarInfomacao = noticia.ValidarPropriedadeString(noticia.Titulo, "Informacao");

			if(validarTitulo &&  validarInfomacao)
			{
				noticia.DataAlteracao = DateTime.Now;
				noticia.DataCadastro = DateTime.Now;
				noticia.Ativo = true;
				await _INoticia.Adicionar(noticia);
			}
		}

		public async Task AtualizaNoticia(Noticia noticia)
		{
			var validarTitulo = noticia.ValidarPropriedadeString(noticia.Titulo, "Titulo");
			var validarInfomacao = noticia.ValidarPropriedadeString(noticia.Titulo, "Informacao");

			if (validarTitulo && validarInfomacao)
			{
				noticia.DataAlteracao = DateTime.Now;
				noticia.DataCadastro = DateTime.Now;
				noticia.Ativo = true;
				await _INoticia.Atualizar(noticia);
			}
		}

		public async Task<List<Noticia>> ListarNoticiasAtivas()
		{
			return await _INoticia.listarNoticias(n => n.Ativo);
		}
	}
}
