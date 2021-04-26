using Integra.Shared.Base;
using Integra.Shared.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Integra.Web.Services
{
	public interface ICotizaciónDataService
	{
		Task<CotizaciónDto> ActualizarAsync(CotizaciónDto Cotización);
		Task<CotizaciónDto> AdicionarAsync(CotizaciónDto Cotización);
		Task<bool> EliminarAsync(CotizaciónDto Cotización);
		Task<IEnumerable<CotizaciónDto>> TraerAyuda(string loQueBusco);

		Task<PaginatedList<CotizaciónDto>> TraerPáginaAsync(string loQueBusco, int númeroDePágina = 1, int tamañoDePágina = 10);
		Task<CotizaciónDto> TraerUnoPorIdAsync(int cotizaciónId);
	}
}
