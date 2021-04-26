using Integra.Shared.Base;
using Integra.Shared.Domain;
using System.Threading.Tasks;

namespace Integra.DataAccess.Repositories
{
	public interface IAcciónDeInventarioRepository : IGenéricoRepository<AcciónDeInventario>
	{
		Task<bool> ActualizaEstado(ulong AcciónDeInventarioId, EstadoAcciónDeInventarioEnum EstadoAcciónDeInventarioId);
	}
}
