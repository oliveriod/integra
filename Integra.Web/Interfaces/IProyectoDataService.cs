using Integra.Shared.Base;
using Integra.Shared.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Integra.Web.Services
{
	public interface IProyectoDataService
	{
		Task<ProyectoDto> ActualizarAsync(ProyectoDto Proyecto);
		Task<ProyectoDto> AdicionarAsync(ProyectoDto Proyecto);
		Task<bool> EliminarAsync(ProyectoDto Proyecto);
		Task<IEnumerable<ProyectoDto>> TraerAyuda(string loQueBusco);

		Task<PaginatedList<ProyectoDto>> TraerPáginaAsync(string loQueBusco, int númeroDePágina = 1, int tamañoDePágina = 10);
		Task<ProyectoDto> TraerUnoPorIdAsync(int proyectoId);
	}
}
