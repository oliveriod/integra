using Integra.Shared;
using Integra.Shared.Base;
using Integra.Shared.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Integra.Web.Services
{
	public class ArtículoSubTipoDataService : IArtículoSubTipoDataService, IInventarioAPI
	{
		private readonly IConsumirAPIService _consumirAPIService;
		private readonly ILogger<ArtículoSubTipoDto> _logger;
		private readonly string _MyStringUri;

		/// <summary>
		/// 20210313 Nunca se te ocurra quitar httpClient. Si lo quitas no funciona esto.
		/// BTW ya lo hiciste y fue un lío encontrar la solución.
		/// Just DON'T!
		/// </summary>
		/// <param name="httpClient"></param>
		/// <param name="consumirAPIService"></param>
		/// <param name="logger"></param>
		public ArtículoSubTipoDataService(HttpClient httpClient, IConsumirAPIService consumirAPIService, ILogger<ArtículoSubTipoDto> logger)
		{
			_consumirAPIService = consumirAPIService;
			_logger = logger;
			_MyStringUri = "api/articulosubtipos";

		}

		/// <summary>
		/// Actualiza un artículo
		/// </summary>
		/// <param name="ArtículoSubTipo"></param>
		/// <returns></returns>
		public async Task<ArtículoSubTipoDto> ActualizarAsync(ArtículoSubTipoDto ArtículoSubTipo)
		{
			HttpResponseMessage response;

			var elUri = $"{_MyStringUri}/Actualizar";

			string json = JsonSerializer.Serialize(ArtículoSubTipo);
			try
			{
				response = await _consumirAPIService.PUTRequestAsync(elUri, json);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return null;
			}
			return JsonSerializer.Deserialize<ArtículoSubTipoDto>(response.Content.ReadAsStringAsync().Result, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
			;
		}

		/// <summary>
		/// Crea un artículo en la base de datos
		/// </summary>
		/// <param name="ArtículoSubTipo"></param>
		/// <returns></returns>
		public async Task<ArtículoSubTipoDto> AdicionarAsync(ArtículoSubTipoDto ArtículoSubTipo)
		{
			HttpResponseMessage response;

			var elUri = $"{_MyStringUri}/Adicionar";

			ArtículoSubTipo.EstadoId = EstadoEnum.Activo;

			string json = JsonSerializer.Serialize(ArtículoSubTipo);
			try
			{
				response = await _consumirAPIService.POSTRequestAsync(elUri, json);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return null;
			}
			return JsonSerializer.Deserialize<ArtículoSubTipoDto>(response.Content.ReadAsStringAsync().Result, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
			;
		}

		/// <summary>
		/// Eliminar un artículo por Id
		/// </summary>
		/// <param name="artículoId"></param>
		/// <returns></returns>
		public async Task<bool> EliminarAsync(ArtículoSubTipoDto ArtículoSubTipo)
		{
			HttpResponseMessage response;

			var elUri = $"{_MyStringUri}/Eliminar";

			string json = JsonSerializer.Serialize(ArtículoSubTipo);
			try
			{
				response = await _consumirAPIService.DELETERequestAsync(elUri, json);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return false;
			}
			return true; ;
		}


		/// <summary>
		/// Buscar artículos por nombre y devolver IEnumerable
		/// </summary>
		/// <param name="loQueBusco"></param>
		/// <param name="página"></param>
		/// <returns></returns>
		public async Task<IEnumerable<ArtículoSubTipoDto>> TraerAyuda(int artículoTipoId, string loQueBusco)
		{
			HttpResponseMessage response;
			IEnumerable<ArtículoSubTipoDto> Artículos;

			var elUri = $"{_MyStringUri}/TraerAyuda?articuloTipoId={artículoTipoId}&loquebusco={loQueBusco}";

			try
			{
				response = await _consumirAPIService.GETRequestAsync(elUri, string.Empty);
				var ElJson = response.Content.ReadAsStringAsync().Result;
				Artículos = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<ArtículoSubTipoDto>>(ElJson, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				IEnumerable<ArtículoSubTipoDto> vacio = new List<ArtículoSubTipoDto>();
				return vacio;
			}
			return Artículos;
		}

		/// <summary>
		/// Buscar artículos por nombre y devolver IEnumerable
		/// </summary>
		/// <param name="loQueBusco"></param>
		/// <param name="página"></param>
		/// <returns></returns>
		public async Task<IEnumerable<ArtículoSubTipoDto>> TraerTodosAsync(int artículoTipoId, string loQueBusco)
		{
			HttpResponseMessage response;
			IEnumerable<ArtículoSubTipoDto> Artículos;

			var elUri = $"{_MyStringUri}/TraerAyuda?articuloTipoId={artículoTipoId}&loquebusco={loQueBusco}";

			try
			{
				response = await _consumirAPIService.GETRequestAsync(elUri, string.Empty);
				var ElJson = response.Content.ReadAsStringAsync().Result;
				Artículos = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<ArtículoSubTipoDto>>(ElJson, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				IEnumerable<ArtículoSubTipoDto> vacio = new List<ArtículoSubTipoDto>();
				return vacio;
			}
			return Artículos;
		}

		/// <summary>
		/// <summary>
		/// Busca un artículo por Id
		/// </summary>
		/// <param name="artículoSubTipoId"></param>
		/// <returns></returns>
		public async Task<ArtículoSubTipoDto> TraerUnoPorIdAsync(int artículoSubTipoId)
		{
			HttpResponseMessage response;

			var elUri = $"{_MyStringUri}/TraerUnoPorId/{artículoSubTipoId}";

			try
			{
				response = await _consumirAPIService.GETRequestAsync(elUri, string.Empty);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return null;
			}
			return JsonSerializer.Deserialize<ArtículoSubTipoDto>(response.Content.ReadAsStringAsync().Result, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }); ;
		}

		/// <summary>
		/// Buscar artículos por nombre y devolver una página
		/// </summary>
		/// <param name="loQueBusco"></param>
		/// <param name="página"></param>
		/// <returns></returns>
		public async Task<PaginatedList<ArtículoSubTipoDto>> TraerPáginaAsync(string loQueBusco, int númeroDePágina = 1, int tamañoDePágina = 10)
		{
			HttpResponseMessage response;
			PaginatedList<ArtículoSubTipoDto> Artículos;

			var elUri = $"{_MyStringUri}/TraerPagina?loquebusco={loQueBusco}&pagina={númeroDePágina}";

			try
			{
				response = await _consumirAPIService.GETRequestAsync(elUri, string.Empty);
				var ElJson = response.Content.ReadAsStringAsync().Result;
				Artículos = System.Text.Json.JsonSerializer.Deserialize<PaginatedList<ArtículoSubTipoDto>>(ElJson, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				PaginatedList<ArtículoSubTipoDto> vacio = new();
				return vacio;
			}
			return Artículos;
		}

	}
}
