using Integra.Shared.Base;
using Integra.Shared.DTO;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Integra.Web.Services
{
	public class CotizaciónDataService : ICotizaciónDataService, IInventarioAPI
	{
		private readonly IConsumirAPIService _consumirAPIService;
		private readonly ILogger<CotizaciónDto> _logger;
		private readonly string _MyStringUri;

		/// <summary>
		/// 20210313 Nunca se te ocurra quitar httpClient. Si lo quitas no funciona esto.
		/// BTW ya lo hiciste y fue un lío encontrar la solución.
		/// Just DON'T!
		/// </summary>
		/// <param name="httpClient"></param>
		/// <param name="consumirAPIService"></param>
		/// <param name="logger"></param>
		public CotizaciónDataService(HttpClient httpClient, IConsumirAPIService consumirAPIService, ILogger<CotizaciónDto> logger)
		{
			_consumirAPIService = consumirAPIService;
			_logger = logger;
			_MyStringUri = "api/cotizaciones";
		}

		/// <summary>
		/// Actualiza un cotización
		/// </summary>
		/// <param name="Cotización"></param>
		/// <returns></returns>
		public async Task<CotizaciónDto> ActualizarAsync(CotizaciónDto Cotización)
		{
			HttpResponseMessage response;

			var elUri = $"{_MyStringUri}/Actualizar";

			string json = JsonSerializer.Serialize(Cotización);
			try
			{
				response = await _consumirAPIService.PUTRequestAsync(elUri, json);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return null;
			}
			return JsonSerializer.Deserialize<CotizaciónDto>(response.Content.ReadAsStringAsync().Result, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
			;
		}

		/// <summary>
		/// Crea un cotización en la base de datos
		/// </summary>
		/// <param name="Cotización"></param>
		/// <returns></returns>
		public async Task<CotizaciónDto> AdicionarAsync(CotizaciónDto Cotización)
		{
			HttpResponseMessage response;

			var elUri = $"{_MyStringUri}/Adicionar";

			Cotización.EstadoId = EstadoEnum.Activo;

			string json = JsonSerializer.Serialize(Cotización);
			try
			{
				response = await _consumirAPIService.POSTRequestAsync(elUri, json);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return null;
			}
			return JsonSerializer.Deserialize<CotizaciónDto>(response.Content.ReadAsStringAsync().Result, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
			;
		}

		/// <summary>
		/// Eliminar un cotización
		/// </summary>
		/// <param name="Cotización"></param>
		/// <returns></returns>
		public async Task<bool> EliminarAsync(CotizaciónDto Cotización)
		{
			HttpResponseMessage response;

			var elUri = $"{_MyStringUri}/Eliminar";

			string json = JsonSerializer.Serialize(Cotización);
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
		public async Task<IEnumerable<CotizaciónDto>> TraerAyuda(string loQueBusco)
		{
			HttpResponseMessage response;
			IEnumerable<CotizaciónDto> Cotizaciones;

			var elUri = $"{_MyStringUri}/TraerAyuda?loquebusco={loQueBusco}";

			try
			{
				response = await _consumirAPIService.GETRequestAsync(elUri, string.Empty);
				var ElJson = response.Content.ReadAsStringAsync().Result;
				Cotizaciones = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<CotizaciónDto>>(ElJson, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				IEnumerable<CotizaciónDto> vacio = new List<CotizaciónDto>();
				return vacio;
			}
			return Cotizaciones;
		}

		/// <summary>
		/// Busca un cotización por Id
		/// </summary>
		/// <param name="cotizaciónId"></param>
		/// <returns></returns>
		public async Task<CotizaciónDto> TraerUnoPorIdAsync(int cotizaciónId)
		{
			HttpResponseMessage response;

			var elUri = $"{_MyStringUri}/TraerUnoPorId/{cotizaciónId}";

			try
			{
				response = await _consumirAPIService.GETRequestAsync(elUri, string.Empty);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return null;
			}
			return JsonSerializer.Deserialize<CotizaciónDto>(response.Content.ReadAsStringAsync().Result, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }); ;
		}

		/// <summary>
		/// Buscar cotizaciones por nombre y devolver una página
		/// </summary>
		/// <param name="loQueBusco"></param>
		/// <param name="página"></param>
		/// <returns></returns>
		public async Task<PaginatedList<CotizaciónDto>> TraerPáginaAsync(string loQueBusco, int númeroDePágina = 1, int tamañoDePágina = 10)
		{
			HttpResponseMessage response;
			PaginatedList<CotizaciónDto> Cotizaciones;

			var elUri = $"{_MyStringUri}/TraerPagina?loquebusco={loQueBusco}&pagina={númeroDePágina}&cuantospp={tamañoDePágina}";

			try
			{
				response = await _consumirAPIService.GETRequestAsync(elUri, string.Empty);
				var ElJson = response.Content.ReadAsStringAsync().Result;
				Cotizaciones = System.Text.Json.JsonSerializer.Deserialize<PaginatedList<CotizaciónDto>>(ElJson, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				PaginatedList<CotizaciónDto> vacio = new();
				return vacio;
			}
			return Cotizaciones;
		}

	}
}
