using Integra.Shared;
using Integra.Shared.Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Integra.DataAccess.Repositories
{
	public class UbicaciónRepository : GenéricoRepository<Ubicación>, IUbicaciónRepository
	{
		public UbicaciónRepository(IntegraDbContext context) : base(context)
		{
		}
		public override Ubicación Adicionar(Ubicación entity)
		{
			var secuencia = _context.Secuencias
				.Single(c => c.Columna == "UbicaciónId");
			secuencia.ValorActual += 1;
			entity.UbicaciónId = (ushort) secuencia.ValorActual;

			return base.Adicionar(entity);
		}

	}
}
