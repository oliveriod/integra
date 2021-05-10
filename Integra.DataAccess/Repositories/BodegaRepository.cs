using Integra.Shared;
using Integra.Shared.Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Integra.DataAccess.Repositories
{
	public class BodegaRepository : GenéricoRepository<Bodega>, IBodegaRepository
	{
		public BodegaRepository(IntegraDbContext context) : base(context)
		{
		}
		public override Bodega Adicionar(Bodega entity)
		{
			var secuencia = _context.Secuencias
				.Single(c => c.Columna == "BodegaId");
			secuencia.ValorActual += 1;
			entity.BodegaId = (ushort) secuencia.ValorActual;

			return base.Adicionar(entity);
		}

	}
}
