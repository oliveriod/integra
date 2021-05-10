using Integra.Shared.Base;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Integra.DataAccess.Repositories
{
	public interface IGenéricoRepository<T> where T : class
	{
		T Actualizar(T entity);
		T Adicionar(T entity);
		void AdicionarVarios(IEnumerable<T> entities);
		IEnumerable<T> Buscar(Expression<Func<T, bool>> predicate);
		T BuscarPrimero(Expression<Func<T, bool>> predicate);
		void Eliminar(T entity);
		void EliminarVarios(IEnumerable<T> entities);
		bool Existe(uint id);
		void SaveChanges();
		IEnumerable<T> TraerTodos(Expression<Func<T, bool>> elWhere);
		IEnumerable<T> TraerTodos(Expression<Func<T, bool>> elWhere, Expression<Func<T, string>> elOrderBy);
		Task<List<T>> TraerTodosAsync(Expression<Func<T, bool>> elWhere, Expression<Func<T, string>> elOrderBy);
		Task<T> TraerUnoAsync(Expression<Func<T, bool>> elWhere);
		Task<T> TraerUnoAsync(Expression<Func<T, bool>> elWhere, IList<string> losIncludes);
		T TraerUnoPorId(ulong id);
		T TraerUnoPorId(uint id);
		T TraerUnoPorId(ushort id);
		T TraerUnoPorId(byte id);
		Task<PaginatedList<T>> TraerVariosAsync(int númeroDePágina = 1, int tamañoDePágina = 10);
		Task<PaginatedList<T>> TraerVariosAsync(Expression<Func<T, bool>> elWhere, int númeroDePágina = 1, int tamañoDePágina = 10);
		Task<PaginatedList<T>> TraerVariosAsync(Expression<Func<T, bool>> elWhere, Expression<Func<T, string>> elOrderBy, int númeroDePágina = 1, int tamañoDePágina = 10);
		Task<PaginatedList<T>> TraerVariosAsync(Expression<Func<T, bool>> elWhere, Expression<Func<T, string>> elOrderBy, IList<string> losIncludes, int númeroDePágina = 1, int tamañoDePágina = 10);
		Task<IEnumerable<T>> TraerVariosPTAAsync(Expression<Func<T, bool>> elWhere, Expression<Func<T, string>> elOrderBy, int tamañoDePágina = 50);
		Task<IEnumerable<T>> TraerVariosSinTopeAsync(Expression<Func<T, bool>> elWhere, IList<string> losIncludes, Expression<Func<T, string>> elOrderBy);

	}
}

