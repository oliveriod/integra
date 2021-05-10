using Integra.Shared.Base;
using Integra.Shared.Domain;
using Integra.Shared.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Integra.API.Services
{
	public interface IInventarioService
	{
		Inventario Actualizar( Inventario algoParaActualizar);
		Inventario Adicionar( Inventario algoParaAdicionar);
		bool AplicaAcciónDeInventario(AcciónDeInventario acción);
		IEnumerable<Inventario> TraerAyuda(string loquebusco, int cuantospp = 50);
		Task<PaginatedList<InventarioDto>> TraerPaginaAsync(ushort bodegaId, string loquebusco, int númeroDePágina, int tamañoDePágina = 10);
		Inventario TraerUno(uint artículoId, ushort bodegaId);
	}
}
