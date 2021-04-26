using Integra.Shared.Base;
using Integra.Shared.Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Integra.DataAccess.Repositories
{
	public class ArtículoSubTipoRepository : GenéricoRepository<ArtículoSubTipo>, IArtículoSubTipoRepository
	{
		public ArtículoSubTipoRepository(IntegraDbContext context) : base(context)
		{

		}

		public override ArtículoSubTipo Actualizar(ArtículoSubTipo entity)
		{
			var artículoSubTipo = _context.ArtículoSubTipos
				.Single(c => c.ArtículoSubTipoId == entity.ArtículoSubTipoId);

			artículoSubTipo.Nombre = entity.Nombre;
			artículoSubTipo.Código = entity.Código;

			return base.Actualizar(artículoSubTipo);
		}

		public override bool Existe(uint id)
		{
			return _context.ArtículoSubTipos.Any(articulo => articulo.ArtículoSubTipoId == id);
		}

		public async Task <PaginatedList<ArtículoSubTipo>> TraerVarios(string nombre, int? númeroDePágina, int tamañoDePágina = 10)
		{
			var ArtículoSubTipos = from a in _context.ArtículoSubTipos
							select a;

			if (!string.IsNullOrEmpty(nombre))
				ArtículoSubTipos = ArtículoSubTipos.Where(s => s.Nombre.ToLower().Contains(nombre.ToLower()));

			return await PaginatedList<ArtículoSubTipo>.CreateAsync(ArtículoSubTipos.AsNoTracking(), númeroDePágina ?? 1, tamañoDePágina);
		}

 	}
}
