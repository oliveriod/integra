using Integra.Shared.Base;
using Integra.Shared.Domain;
using System.Linq;
using System.Threading.Tasks;

namespace Integra.DataAccess.Repositories
{
	public class AcciónDeInventarioRepository : GenéricoRepository<AcciónDeInventario>, IAcciónDeInventarioRepository
	{
		public AcciónDeInventarioRepository(IntegraDbContext context) : base(context)
		{
		}

		public async Task<bool> ActualizaEstado(ulong AcciónDeInventarioId, EstadoAcciónDeInventarioEnum EstadoAcciónDeInventarioId)
		{
			var acción = _context.AccionesDeInventario.FirstOrDefault(a => a.AcciónDeInventarioId == AcciónDeInventarioId);

			acción.EstadoAcciónDeInventarioId = EstadoAcciónDeInventarioId;

			await _context.SaveChangesAsync();

			return true;
		}
	}
}
