using Integra.Shared.Base;
using Integra.Shared.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Integra.DataAccess.Repositories
{
	public class GenéricoRepository<T> : IGenéricoRepository<T> where T : class
	{
		protected readonly IntegraDbContext _context;

		public GenéricoRepository(IntegraDbContext context)
		{
			_context = context;
		}

		public virtual T Actualizar(T entity)
		{
			return _context.Update(entity)
				.Entity;
		}

		public virtual T Adicionar(T entity)
		{
			return _context.Set<T>().Add(entity).Entity;
		}

		public virtual void AdicionarVarios(IEnumerable<T> entities)
		{
			_context.Set<T>().AddRange(entities);
		}

		public virtual IEnumerable<T> Buscar(Expression<Func<T, bool>> elWhere)
		{
			return _context.Set<T>().Where(elWhere);
		}

		public virtual T BuscarPrimero(Expression<Func<T, bool>> elWhere)
		{
			return _context.Set<T>().Where(elWhere).FirstOrDefault();
		}

		public void Eliminar(T entity)
		{
			_context.Set<T>().Remove(entity);
		}

		public virtual void EliminarVarios(IEnumerable<T> entities)
		{
			_context.Set<T>().RemoveRange(entities);
		}

		public virtual bool Existe(uint id)
		{
			throw new NotImplementedException();
		}

		public virtual bool Existe(ulong id)
		{
			throw new NotImplementedException();
		}

		public void SaveChanges()
		{
			_context.SaveChanges();
		}

		public virtual IEnumerable<T> TraerTodos(Expression<Func<T, bool>> elWhere)
		{
			return TraerTodos(elWhere, null);
		}
		public virtual IEnumerable<T> TraerTodos(Expression<Func<T, bool>> elWhere, Expression<Func<T, string>> elOrderBy)
		{
			var Entidad = from a in _context.Set<T>()
						  select a;

			if (elWhere != null)
				Entidad = Entidad.Where(elWhere);

			if (elOrderBy != null)
				Entidad = Entidad.OrderBy(elOrderBy);

			return Entidad.ToList<T>();
		}

		public virtual async Task<List<T>> TraerTodosAsync(Expression<Func<T, bool>> elWhere, Expression<Func<T, string>> elOrderBy)
		{
			var Entidad = from a in _context.Set<T>()
						  select a;

			if (elWhere != null)
				Entidad = Entidad.Where(elWhere);

			if (elOrderBy != null)
				Entidad = Entidad.OrderBy(elOrderBy);

			return await Entidad.ToListAsync<T>();
		}

		public async Task<T> TraerUnoAsync(Expression<Func<T, bool>> elWhere)
		{
			return await TraerUnoAsync(elWhere, null);
		}


		public async Task<T> TraerUnoAsync(Expression<Func<T, bool>> elWhere, IList<string> losIncludes)
		{
			var Entidad = from a in _context.Set<T>()
						  select a;

			Entidad = Entidad.Where(elWhere);

			if (losIncludes != null)
			{
				foreach (var elInclude in losIncludes)
				{
					Entidad = Entidad.Include(elInclude);
				}
			}
			return await Entidad.FirstOrDefaultAsync();
		}

		public virtual T TraerUnoPorId(ulong id)
		{
			return _context.Set<T>().Find(id);
		}

		public virtual T TraerUnoPorId(uint id)
		{
			return _context.Set<T>().Find(id);
		}

		public virtual T TraerUnoPorId(ushort id)
		{
			return _context.Set<T>().Find(id);
		}

		public virtual T TraerUnoPorId(byte id)
		{
			return _context.Set<T>().Find(id);
		}

		public async Task<PaginatedList<T>> TraerVariosAsync(int númeroDePágina = 1, int tamañoDePágina = 10)
		{
			return await TraerVariosAsync(null, null, null, númeroDePágina, tamañoDePágina);
		}

		public async Task<PaginatedList<T>> TraerVariosAsync(Expression<Func<T, bool>> elWhere, int númeroDePágina = 1, int tamañoDePágina = 10)
		{
			return await TraerVariosAsync(elWhere, null, null, númeroDePágina, tamañoDePágina);
		}

		public async Task<PaginatedList<T>> TraerVariosAsync(Expression<Func<T, bool>> elWhere, Expression<Func<T, string>> elOrderBy, int númeroDePágina = 1, int tamañoDePágina = 10)
		{
			return await TraerVariosAsync(elWhere, elOrderBy, null, númeroDePágina, tamañoDePágina);
		}

		public async Task<PaginatedList<T>> TraerVariosAsync(Expression<Func<T, bool>> elWhere, Expression<Func<T, string>> elOrderBy, IList<string> losIncludes, int númeroDePágina = 1, int tamañoDePágina = 10)
		{
			var Entidad = from a in _context.Set<T>()
						  select a;

			if (losIncludes != null)
			{
				foreach (var elInclude in losIncludes)
				{
					Entidad = Entidad.Include(elInclude);
				}
			}
			if (elWhere != null)
				Entidad = Entidad.Where(elWhere);

			if (elOrderBy != null)
				Entidad = Entidad.OrderBy(elOrderBy);

			return await PaginatedList<T>.CreateAsync(Entidad, númeroDePágina , tamañoDePágina);
		}

		public async Task <IEnumerable<T>> TraerVariosPTAAsync(Expression<Func<T, bool>> elWhere, Expression<Func<T, string>> elOrderBy, int tamañoDePágina = 50)
		{
			var Entidad = from a in _context.Set<T>()
						  select a;

			if (elWhere != null)
				Entidad = Entidad.Where(elWhere);

			if (elOrderBy != null)
				Entidad = Entidad.OrderBy(elOrderBy);

			return await Entidad.Take(tamañoDePágina).ToListAsync();
		}

		public async Task<IEnumerable<T>> TraerVariosSinTopeAsync(Expression<Func<T, bool>> elWhere, IList<string> losIncludes, Expression<Func<T, string>> elOrderBy)
		{
			var Entidad = from a in _context.Set<T>()
						  select a;

			if (elWhere != null)
				Entidad = Entidad.Where(elWhere);

			if (elOrderBy != null)
				Entidad = Entidad.OrderBy(elOrderBy);

			if (losIncludes != null)
			{
				foreach (var elInclude in losIncludes)
				{
					Entidad = Entidad.Include(elInclude);
				}
			}

			return await Entidad.ToListAsync();
		}




	}
}
