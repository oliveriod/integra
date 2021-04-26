using Integra.Shared.Base;
using Integra.Shared;
using Integra.Shared.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Integra.Web.Services
{
	public interface IArtículoSubTipoDataService
	{
		Task<ArtículoSubTipoDto> ActualizarAsync(ArtículoSubTipoDto ArtículoSubTipo);
		Task<ArtículoSubTipoDto> AdicionarAsync(ArtículoSubTipoDto ArtículoSubTipo);
		Task<bool> EliminarAsync(ArtículoSubTipoDto ArtículoSubTipo);
		Task<IEnumerable<ArtículoSubTipoDto>> TraerAyuda(int artículoTipoId, string loQueBusco);
		Task<PaginatedList<ArtículoSubTipoDto>>	TraerPáginaAsync(string loQueBusco, int númeroDePágina, int tamañoDePágina = 10);
		Task<ArtículoSubTipoDto> TraerUnoPorIdAsync(int artículoSubTipoId);
	}
}
