﻿using Dominio.Interfaces;
using Entidades.Entidades;
using Infraestrutura.Configuracoes;
using Infraestrutura.Repositorio.Genericos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infraestrutura.Repositorio
{
	public class RepositorioNoticia : RepositorioGenerico<Noticia>, INoticia
	{
		private readonly DbContextOptions<Contexto> _optionbuilder;

		public RepositorioNoticia()
		{
			_optionbuilder = new DbContextOptions<Contexto>();
		} 

		public async Task<List<Noticia>> listarNoticias(Expression<Func<Noticia, bool>> exNoticia)
		{
			using(var banco = new Contexto(_optionbuilder))
			{
				return await banco.Noticia.Where(exNoticia).AsNoTracking().ToListAsync();
			}
		}
	}
}
