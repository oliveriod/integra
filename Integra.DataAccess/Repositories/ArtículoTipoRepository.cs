using Integra.Shared.Base;
using Integra.Shared.Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Integra.DataAccess.Repositories
{
	public class ArtículoTipoRepository : GenéricoRepository<ArtículoTipo>, IArtículoTipoRepository
	{
		public ArtículoTipoRepository(IntegraDbContext context) : base(context)
		{

		}

		public override ArtículoTipo Actualizar(ArtículoTipo entity)
		{
			var artículoTipo = _context.ArtículoTipos
				.Single(c => c.ArtículoTipoId == entity.ArtículoTipoId);

			artículoTipo.Nombre = entity.Nombre;
			artículoTipo.Código = entity.Código;

			return base.Actualizar(artículoTipo);
		}

		public override bool Existe(uint id)
		{
			return _context.ArtículoTipos.Any(articulo => articulo.ArtículoTipoId == id);
		}

		public async Task <PaginatedList<ArtículoTipo>> TraerVarios(string nombre, int? númeroDePágina, int tamañoDePágina = 10)
		{
			var ArtículoTipos = from a in _context.ArtículoTipos
							select a;

			if (!string.IsNullOrEmpty(nombre))
				ArtículoTipos = ArtículoTipos.Where(s => s.Nombre.ToLower().Contains(nombre.ToLower()));

			return await PaginatedList<ArtículoTipo>.CreateAsync(ArtículoTipos.AsNoTracking(), númeroDePágina ?? 1, tamañoDePágina);
		}

 	}
}
