using Integra.Shared.Base;
using Integra.Shared.Domain;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Integra.API.Services
{
	public interface IArtículoService
	{
		Artículo Actualizar([FromBody] Artículo algoParaActualizar);
		Artículo Adicionar([FromBody] Artículo algoParaAdicionar);
		bool AplicaAcciónDeInventario(AcciónDeInventario acción);
		bool Eliminar([FromBody] Artículo algoParaEliminar);
		bool Producir(ushort recetaId, decimal CantidadAProducir, ushort BodegaId);
		IEnumerable<Artículo> TraerAyuda([FromQuery] string loquebusco, int cuantospp = 50);
		PaginatedList<Artículo> TraerPagina([FromQuery] string loquebusco, int pagina, int cuantospp = 10);
		Artículo TraerUnoPorId(int artículoId);
	}
}