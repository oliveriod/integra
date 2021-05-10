using Integra.Shared.Base;
using Integra.Shared.Domain;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Integra.API.Services
{
	public interface IArtículoService
	{
		Artículo Actualizar( Artículo algoParaActualizar);
		Artículo AdicionarArtículo( Artículo algoParaAdicionar);
		Ubicación AdicionarUbicación(Ubicación algoParaAdicionar);
		bool AplicaAcciónDeInventario(AcciónDeInventario acción);
		bool Eliminar( Artículo algoParaEliminar);
		bool Producir(ushort recetaId, decimal CantidadAProducir, ushort BodegaId);
		IEnumerable<Artículo> TraerAyuda( string loquebusco, int cuantospp = 50);
		PaginatedList<Artículo> TraerPagina( string loquebusco, int pagina, int cuantospp = 10);
		Artículo TraerUnoPorId(int artículoId);
	}
}
