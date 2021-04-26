using Integra.Shared.Base;
using Integra.Shared.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Integra.Web.Services
{
	public interface IVendedorDataService
	{
		Task<VendedorDto> ActualizarAsync(VendedorDto Vendedor);
		Task<VendedorDto> AdicionarAsync(VendedorDto Vendedor);
		Task<bool> EliminarAsync(VendedorDto Vendedor);
		Task<IEnumerable<VendedorDto>> TraerAyuda(string loQueBusco);

		Task<PaginatedList<VendedorDto>> TraerPáginaAsync(string loQueBusco, int númeroDePágina, int tamañoDePágina = 10);
		Task<VendedorDto> TraerUnoPorIdAsync(int vendedorId);
	}
}
