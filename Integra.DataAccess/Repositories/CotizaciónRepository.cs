using Integra.Shared;
using Integra.Shared.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Integra.DataAccess.Repositories
{
	public class CotizaciónRepository : GenéricoRepository<Cotización>, ICotizaciónRepository
	{
		public CotizaciónRepository(IntegraDbContext context) : base(context)
		{
		}

		public override bool Existe(uint id)
		{
			return _context.Artículos.Any(articulo => articulo.ArtículoId == id);
		}

		public override IEnumerable<Cotización> Buscar(Expression<Func<Cotización, bool>> predicate)
		{
			return _context.Cotizaciones
				.Include(cotización => cotización.Cliente)
				.Include(cotización => cotización.Vendedor)
				.Include(cotización => cotización.Proyecto)
				.Include(cotización => cotización.CotizaciónLíneas)
				.ThenInclude(lineItem => lineItem.Artículo)
				.Where(predicate).ToList();
		}



		public override Cotización TraerUnoPorId(uint cotizaciónId)
		{
			return _context.Cotizaciones
				.Include(cotización => cotización.Cliente)
				.Include(cotización => cotización.Vendedor)
				.Include(cotización => cotización.Proyecto)
				.Include(cotización => cotización.CotizaciónLíneas)
				.ThenInclude(linea => linea.Artículo)
				.FirstOrDefault(o => o.CotizaciónId == cotizaciónId)
				;
		}

		public Cotización TraerUnoPorIdConTodo(uint cotizaciónId)
		{
			var Cotizaciones = from a in _context.Cotizaciones
							select a;

			Cotizaciones
				.Include(cotización => cotización.Cliente)
				.Include(cotización => cotización.Vendedor)
				.Include(cotización => cotización.Proyecto)
				.Include(cotización => cotización.CotizaciónLíneas)
				.ThenInclude(linea => linea.Artículo)
				.FirstOrDefault(o => o.CotizaciónId == cotizaciónId)
				;
			return Cotizaciones.FirstOrDefault();
		}

		public override Cotización Actualizar(Cotización entity)
		{
			var order = _context.Cotizaciones
				.Include(o => o.CotizaciónLíneas)
				.ThenInclude(lineItem => lineItem.Artículo)
				.Single(o => o.CotizaciónId == entity.CotizaciónId);

			order.FechaCotización = entity.FechaCotización;
			order.CotizaciónLíneas = entity.CotizaciónLíneas;

			return base.Actualizar(order);
		}
	}
}
