using Integra.Shared;
using Integra.Shared.Domain;
using System.Linq;

namespace Integra.DataAccess.Repositories
{
	public class VendedorRepository : GenéricoRepository<Vendedor>
	{
		public VendedorRepository(IntegraDbContext context) : base(context)
		{
		}

		public override Vendedor Actualizar(Vendedor entity)
		{
			var cliente = _context.Vendedores
				.Single(c => c.VendedorId == entity.VendedorId);

			cliente.Nombre = entity.Nombre;
			cliente.PrimerApellido = entity.PrimerApellido;
			cliente.SegundoApellido = entity.SegundoApellido;
			cliente.Teléfono = entity.Teléfono;
			cliente.Identificación = entity.Identificación;

			return base.Actualizar(cliente);
		}
	}
}
