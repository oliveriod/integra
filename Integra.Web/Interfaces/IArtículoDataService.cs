using Integra.Shared.Base;
using Integra.Shared.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Integra.Web.Services
{
	public interface IArtículoDataService
	{
		Task<ArtículoDto> ActualizarAsync(ArtículoDto Artículo);
		Task<ArtículoDto> AdicionarAsync(ArtículoDto Artículo);
		Task<bool> EliminarAsync(ArtículoDto Artículo);
		Task<IEnumerable<ArtículoDto>>		TraerAyuda(string loQueBusco);
		Task<PaginatedList<ArtículoDto>>	TraerPáginaAsync(string loQueBusco, int númeroDePágina, int tamañoDePágina = 10);
		Task<ArtículoDto> TraerUnoPorIdAsync(int artículoId);
	}
}
