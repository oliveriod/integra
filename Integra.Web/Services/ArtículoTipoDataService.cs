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
	public class ArtículoTipoDataService : IArtículoTipoDataService, IInventarioAPI
	{
		private readonly IConsumirAPIService _consumirAPIService;
		private readonly ILogger<ArtículoTipoDto> _logger;
		private readonly string _MyStringUri;

		/// <summary>
		/// 20210313 Nunca se te ocurra quitar httpClient. Si lo quitas no funciona esto.
		/// BTW ya lo hiciste y fue un lío encontrar la solución.
		/// Just DON'T!
		/// </summary>
		/// <param name="httpClient"></param>
		/// <param name="consumirAPIService"></param>
		/// <param name="logger"></param>
		public ArtículoTipoDataService(HttpClient httpClient, IConsumirAPIService consumirAPIService, ILogger<ArtículoTipoDto> logger)
		{
			_consumirAPIService = consumirAPIService;
			_logger = logger;
			_MyStringUri = "api/articulotipos";

		}

		/// <summary>
		/// Actualiza un artículo
		/// </summary>
		/// <param name="ArtículoTipo"></param>
		/// <returns></returns>
		public async Task<ArtículoTipoDto> ActualizarAsync(ArtículoTipoDto ArtículoTipo)
		{
			HttpResponseMessage response;

			var elUri = $"{_MyStringUri}/Actualizar";

			string json = JsonSerializer.Serialize(ArtículoTipo);
			try
			{
				response = await _consumirAPIService.PUTRequestAsync(elUri, json);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return null;
			}
			return JsonSerializer.Deserialize<ArtículoTipoDto>(response.Content.ReadAsStringAsync().Result, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
			;
		}

		/// <summary>
		/// Crea un artículo en la base de datos
		/// </summary>
		/// <param name="ArtículoTipo"></param>
		/// <returns></returns>
		public async Task<ArtículoTipoDto> AdicionarAsync(ArtículoTipoDto ArtículoTipo)
		{
			HttpResponseMessage response;

			var elUri = $"{_MyStringUri}/Adicionar";

			ArtículoTipo.Estado = EstadoEnum.Activo;

			string json = JsonSerializer.Serialize(ArtículoTipo);
			try
			{
				response = await _consumirAPIService.POSTRequestAsync(elUri, json);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return null;
			}
			return JsonSerializer.Deserialize<ArtículoTipoDto>(response.Content.ReadAsStringAsync().Result, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
			;
		}

		/// <summary>
		/// Eliminar un artículo por Id
		/// </summary>
		/// <param name="artículoId"></param>
		/// <returns></returns>
		public async Task<bool> EliminarAsync(ArtículoTipoDto ArtículoTipo)
		{
			HttpResponseMessage response;

			var elUri = $"{_MyStringUri}/Actualizar";

			string json = JsonSerializer.Serialize(ArtículoTipo);
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
		public async Task<IEnumerable<ArtículoTipoDto>> TraerAyuda(string loQueBusco)
		{
			HttpResponseMessage response;
			IEnumerable<ArtículoTipoDto> ArtículoTipos;

			var elUri = $"{_MyStringUri}/TraerAyuda?loquebusco={loQueBusco}";

			try
			{
				response = await _consumirAPIService.GETRequestAsync(elUri, string.Empty);
				var ElJson = response.Content.ReadAsStringAsync().Result;
				ArtículoTipos = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<ArtículoTipoDto>>(ElJson, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				IEnumerable<ArtículoTipoDto> vacio = new List<ArtículoTipoDto>();
				return vacio;
			}
			return ArtículoTipos;
		}

		/// <summary>
		/// Buscar artículos por nombre y devolver IEnumerable
		/// </summary>
		/// <param name="loQueBusco"></param>
		/// <param name="página"></param>
		/// <returns></returns>
		public async Task<IEnumerable<ArtículoTipoDto>> TraerTodosAsync()
		{
			HttpResponseMessage response;
			List<ArtículoTipoDto> ArtículoTipos;

			var elUri = $"{_MyStringUri}/TraerTodos";

			try
			{
				response = await _consumirAPIService.GETRequestAsync(elUri, string.Empty);
				var ElJson = response.Content.ReadAsStringAsync().Result;
				ArtículoTipos = System.Text.Json.JsonSerializer.Deserialize<List<ArtículoTipoDto>>(ElJson, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				IEnumerable<ArtículoTipoDto> vacio = new List<ArtículoTipoDto>();
				return vacio;
			}
			return ArtículoTipos;
		}

		/// <summary>
		/// Busca un artículo por Id
		/// </summary>
		/// <param name="artículoTipoId"></param>
		/// <returns></returns>
		public async Task<ArtículoTipoDto> TraerUnoPorIdAsync(int artículoTipoId)
		{
			HttpResponseMessage response;

			var elUri = $"{_MyStringUri}/TraerUnoPorId/{artículoTipoId}";

			try
			{
				response = await _consumirAPIService.GETRequestAsync(elUri, string.Empty);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return null;
			}
			return JsonSerializer.Deserialize<ArtículoTipoDto>(response.Content.ReadAsStringAsync().Result, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }); ;
		}

		/// <summary>
		/// Buscar artículos por nombre y devolver una página
		/// </summary>
		/// <param name="loQueBusco"></param>
		/// <param name="página"></param>
		/// <returns></returns>
		public async Task<PaginatedList<ArtículoTipoDto>> TraerPáginaAsync(string loQueBusco, int númeroDePágina = 1, int tamañoDePágina = 10)
		{
			HttpResponseMessage response;
			PaginatedList<ArtículoTipoDto> ArtículoTipos;

			var elUri = $"{_MyStringUri}/TraerPagina?loquebusco={loQueBusco}&pagina={númeroDePágina}";

			try
			{
				response = await _consumirAPIService.GETRequestAsync(elUri, string.Empty);
				var ElJson = response.Content.ReadAsStringAsync().Result;
				ArtículoTipos = System.Text.Json.JsonSerializer.Deserialize<PaginatedList<ArtículoTipoDto>>(ElJson, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				PaginatedList<ArtículoTipoDto> vacio = new();
				return vacio;
			}
			return ArtículoTipos;
		}
	}
}
