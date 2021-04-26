using Integra.Shared;
using Integra.Shared.Domain;
using System.Linq;

namespace Integra.DataAccess.Repositories
{
	public class ProyectoRepository : GenéricoRepository<Proyecto>, IProyectoRepository
	{
		public ProyectoRepository(IntegraDbContext context) : base(context)
		{
		}

		//public override Proyecto Actualizar(Proyecto entity)
		//{
		//	var proyecto = _context.Proyectos
		//		.Single(c => c.ProyectoId == entity.ProyectoId);

		//	proyecto.Descripción = entity.Descripción;

		//	return base.Actualizar(proyecto);
		//}
	}
}
