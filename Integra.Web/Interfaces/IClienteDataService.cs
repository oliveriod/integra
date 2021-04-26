using Integra.Shared.Base;
using Integra.Shared.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Integra.Web.Services
{
	public interface IClienteDataService
	{
		Task<ClienteDto> ActualizarAsync(ClienteDto Cliente);
		Task<ClienteDto> AdicionarAsync(ClienteDto Cliente);
		Task<bool> EliminarAsync(ClienteDto Cliente);
		Task<IEnumerable<ClienteDto>> TraerAyuda(string loQueBusco);

		Task<PaginatedList<ClienteDto>> TraerPáginaAsync(string loQueBusco, int númeroDePágina = 1, int tamañoDePágina = 10);
		Task<ClienteDto> TraerUnoPorIdAsync(int artículoId);

	}
}
