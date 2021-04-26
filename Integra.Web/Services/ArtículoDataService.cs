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
	public class ArtículoDataService : IArtículoDataService, IInventarioAPI
	{
		private readonly IConsumirAPIService _consumirAPIService;
		private readonly ILogger<ArtículoDto> _logger;
		private readonly string _MyStringUri;

		/// <summary>
		/// 20210313 Nunca se te ocurra quitar httpClient. Si lo quitas no funciona esto.
		/// BTW ya lo hiciste y fue un lío encontrar la solución.
		/// Just DON'T!
		/// </summary>
		/// <param name="httpClient"></param>
		/// <param name="consumirAPIService"></param>
		/// <param name="logger"></param>
		public ArtículoDataService(HttpClient httpClient, IConsumirAPIService consumirAPIService, ILogger<ArtículoDto> logger)
		{
			_consumirAPIService = consumirAPIService;
			_logger = logger;
			_MyStringUri = "api/articulos";

		}

		/// <summary>
		/// Actualiza un artículo
		/// </summary>
		/// <param name="Artículo"></param>
		/// <returns></returns>
		public async Task<ArtículoDto> ActualizarAsync(ArtículoDto Artículo)
		{
			HttpResponseMessage response;

			var elUri = $"{_MyStringUri}/Actualizar";

			string json = JsonSerializer.Serialize(Artículo);
			try
			{
				response = await _consumirAPIService.PUTRequestAsync(elUri, json);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return null;
			}
			return JsonSerializer.Deserialize<ArtículoDto>(response.Content.ReadAsStringAsync().Result, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
			;
		}

		/// <summary>
		/// Crea un artículo en la base de datos
		/// </summary>
		/// <param name="Artículo"></param>
		/// <returns></returns>
		public async Task<ArtículoDto> AdicionarAsync(ArtículoDto Artículo)
		{
			HttpResponseMessage response;

			var elUri = $"{_MyStringUri}/Adicionar";

			Artículo.EstadoId = EstadoEnum.Activo;

			string json = JsonSerializer.Serialize(Artículo);
			try
			{
				response = await _consumirAPIService.POSTRequestAsync(elUri, json);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return null;
			}
			return JsonSerializer.Deserialize<ArtículoDto>(response.Content.ReadAsStringAsync().Result, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
			;
		}

		/// <summary>
		/// Eliminar un artículo por Id
		/// </summary>
		/// <param name="artículoId"></param>
		/// <returns></returns>
		public async Task<bool> EliminarAsync(ArtículoDto Artículo)
		{
			HttpResponseMessage response;
			var elUri = $"{_MyStringUri}/Eliminar";

			string json = JsonSerializer.Serialize(Artículo);
			try
			{
				response = await _consumirAPIService.DELETERequestAsync(elUri, json);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
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
		public async Task<IEnumerable<ArtículoDto>> TraerAyuda(string loQueBusco)
		{
			HttpResponseMessage response;
			IEnumerable<ArtículoDto> Artículos;

			var elUri = $"{_MyStringUri}/TraerAyuda?loquebusco={loQueBusco}";

			try
			{
				response = await _consumirAPIService.GETRequestAsync(elUri, string.Empty);
				var ElJson = response.Content.ReadAsStringAsync().Result;
				Artículos = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<ArtículoDto>>(ElJson, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				_logger.LogError(ex.Message);
				IEnumerable<ArtículoDto> vacio = new List<ArtículoDto>();
				return vacio;
			}
			return Artículos;
		}

		/// <summary>
		/// Buscar artículos por nombre y devolver una página
		/// </summary>
		/// <param name="loQueBusco"></param>
		/// <param name="página"></param>
		/// <returns></returns>
		public async Task<PaginatedList<ArtículoDto>> TraerPáginaAsync(string loQueBusco, int númeroDePágina = 1, int tamañoDePágina = 10)
		{
			HttpResponseMessage response;
			PaginatedList<ArtículoDto> Artículos;

			var elUri = $"{_MyStringUri}/TraerPagina?loquebusco={loQueBusco}&pagina={númeroDePágina}";

			try
			{
				response = await _consumirAPIService.GETRequestAsync(elUri, string.Empty);
				var ElJson = response.Content.ReadAsStringAsync().Result;
				Artículos = System.Text.Json.JsonSerializer.Deserialize<PaginatedList<ArtículoDto>>(ElJson, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				_logger.LogError(ex.Message);
				PaginatedList<ArtículoDto> vacio = new();
				return vacio;
			}
			return Artículos;
		}

		/// <summary>
		/// Busca un artículo por Id
		/// </summary>
		/// <param name="artículoId"></param>
		/// <returns></returns>
		public async Task<ArtículoDto> TraerUnoPorIdAsync(int artículoId)
		{
			HttpResponseMessage response;
			var elUri = $"{_MyStringUri}/TraerUnoPorId/{artículoId}";

			try
			{
				response = await _consumirAPIService.GETRequestAsync(elUri, string.Empty);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				_logger.LogError(ex.Message);
				return null;
			}
			if (!response.IsSuccessStatusCode)
			{
				_logger.LogError(elUri + " - " + response.ToString());
				return null;
			}
			return JsonSerializer.Deserialize<ArtículoDto>(response.Content.ReadAsStringAsync().Result, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }); ;
		}

	}
}
