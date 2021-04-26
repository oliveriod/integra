using Integra.Shared;
using Integra.Shared.Domain;
using System.Linq;

namespace Integra.DataAccess.Repositories
{
	public class ProveedorRepository : GenéricoRepository<Proveedor>, IProveedorRepository
	{
		public ProveedorRepository(IntegraDbContext context) : base(context)
		{
		}

		public override Proveedor Actualizar(Proveedor entity)
		{
			var proveedor = _context.Proveedores
				.Single(c => c.ProveedorId == entity.ProveedorId);

			proveedor.Nombre = entity.Nombre;
			proveedor.PrimerApellido = entity.PrimerApellido;
			proveedor.SegundoApellido = entity.SegundoApellido;
			proveedor.Teléfono = entity.Teléfono;

			return base.Actualizar(proveedor);
		}
	}
}
