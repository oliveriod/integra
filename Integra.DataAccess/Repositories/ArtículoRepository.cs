using Integra.Shared;
using Integra.Shared.Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Integra.DataAccess.Repositories
{
	public class ArtículoRepository : GenéricoRepository<Artículo>, IArtículoRepository
	{
		public ArtículoRepository(IntegraDbContext context) : base(context)
		{
		}
		public override Artículo Adicionar(Artículo entity)
		{
			var secuencia = _context.Secuencias
				.Single(c => c.Columna == "ArtículoId");
			secuencia.ValorActual += 1;
			entity.ArtículoId = (uint) secuencia.ValorActual;

			return base.Adicionar(entity);
		}

	}
}
