using Integra.Shared.Base;
using Integra.Shared.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Integra.Web.Services
{
	public interface IArtículoTipoDataService
	{
		Task<ArtículoTipoDto> ActualizarAsync(ArtículoTipoDto ArtículoTipo);
		Task<ArtículoTipoDto> AdicionarAsync(ArtículoTipoDto ArtículoTipo);
		Task<bool> EliminarAsync(ArtículoTipoDto ArtículoTipo);
		Task<IEnumerable<ArtículoTipoDto>>		TraerAyuda(string loQueBusco);
		Task<IEnumerable<ArtículoTipoDto>>		TraerTodosAsync();
		Task<PaginatedList<ArtículoTipoDto>>	TraerPáginaAsync(string loQueBusco, int númeroDePágina = 1, int tamañoDePágina = 10);
		Task<ArtículoTipoDto> TraerUnoPorIdAsync(int artículoTipoId);
	}
}
