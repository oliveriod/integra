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
	public class ClienteDataService : IClienteDataService, IInventarioAPI
	{
		private readonly IConsumirAPIService _consumirAPIService;
		private readonly ILogger<ClienteDto> _logger;
		private readonly string _MyStringUri;

		/// <summary>
		/// 20210313 Nunca se te ocurra quitar httpClient. Si lo quitas no funciona esto.
		/// BTW ya lo hiciste y fue un lío encontrar la solución.
		/// Just DON'T!
		/// </summary>
		/// <param name="httpClient"></param>
		/// <param name="consumirAPIService"></param>
		/// <param name="logger"></param>
		public ClienteDataService(HttpClient httpClient, IConsumirAPIService consumirAPIService, ILogger<ClienteDto> logger)
		{
			_consumirAPIService = consumirAPIService;
			_logger = logger;
			_MyStringUri = "api/clientes";

		}

		/// <summary>
		/// Actualiza un artículo
		/// </summary>
		/// <param name="Cliente"></param>
		/// <returns></returns>
		public async Task<ClienteDto> ActualizarAsync(ClienteDto Cliente)
		{
			HttpResponseMessage response;
			HttpStatusCode responseStatusCode;

			var elUri = $"{_MyStringUri}/Actualizar";

			string json = JsonSerializer.Serialize(Cliente);
			try
			{
				response = await _consumirAPIService.PUTRequestAsync(elUri, json);
				responseStatusCode = response.StatusCode;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return null;
			}
			return JsonSerializer.Deserialize<ClienteDto>(response.Content.ReadAsStringAsync().Result, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
			;
		}

		/// <summary>
		/// Crea un artículo en la base de datos
		/// </summary>
		/// <param name="Cliente"></param>
		/// <returns></returns>
		public async Task<ClienteDto> AdicionarAsync(ClienteDto Cliente)
		{
			HttpResponseMessage response;

			var elUri = $"{_MyStringUri}/Adicionar";

			Cliente.EstadoId = EstadoEnum.Activo;

			string json = JsonSerializer.Serialize(Cliente);
			try
			{
				response = await _consumirAPIService.POSTRequestAsync(elUri, json);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return null;
			}
			return JsonSerializer.Deserialize<ClienteDto>(response.Content.ReadAsStringAsync().Result, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
			;
		}

		/// <summary>
		/// Eliminar un artículo por Id
		/// </summary>
		/// <param name="artículoId"></param>
		/// <returns></returns>
		public async Task<bool> EliminarAsync(ClienteDto Cliente)
		{
			HttpResponseMessage response;

			var elUri = $"{_MyStringUri}/Eliminar";

			string json = JsonSerializer.Serialize(Cliente);
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
		public async Task<IEnumerable<ClienteDto>> TraerAyuda(string loQueBusco)
		{
			HttpResponseMessage response;
			IEnumerable<ClienteDto> Clientes;

			var elUri = $"{_MyStringUri}/TraerAyuda?loquebusco={loQueBusco}";

			try
			{
				response = await _consumirAPIService.GETRequestAsync(elUri, string.Empty);
				var ElJson = response.Content.ReadAsStringAsync().Result;
				Clientes = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<ClienteDto>>(ElJson, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				IEnumerable<ClienteDto> vacio = new List<ClienteDto>();
				return vacio;
			}
			return Clientes;
		}

		/// <summary>
		/// Busca un artículo por Id
		/// </summary>
		/// <param name="artículoId"></param>
		/// <returns></returns>
		public async Task<ClienteDto> TraerUnoPorIdAsync(int artículoId)
		{
			HttpResponseMessage response;

			var elUri = $"{_MyStringUri}/TraerUnoPorId/{artículoId}";

			try
			{
				response = await _consumirAPIService.GETRequestAsync(elUri, string.Empty);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return null;
			}
			if (!response.IsSuccessStatusCode)
			{
				_logger.LogError(elUri + " - " + response.ToString());
				return null;
			}
			return JsonSerializer.Deserialize<ClienteDto>(response.Content.ReadAsStringAsync().Result, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }); ;
		}

		/// <summary>
		/// Buscar artículos por nombre y devolver una página
		/// </summary>
		/// <param name="loQueBusco"></param>
		/// <param name="página"></param>
		/// <returns></returns>
		public async Task<PaginatedList<ClienteDto>> TraerPáginaAsync(string loQueBusco, int númeroDePágina = 1, int tamañoDePágina = 10)
		{
			HttpResponseMessage response;
			PaginatedList<ClienteDto> Clientes;

			var elUri = $"{_MyStringUri}/TraerPagina?loquebusco={loQueBusco}&pagina={númeroDePágina}";

			try
			{
				response = await _consumirAPIService.GETRequestAsync(elUri, string.Empty);
				var ElJson = response.Content.ReadAsStringAsync().Result;
				Clientes = System.Text.Json.JsonSerializer.Deserialize<PaginatedList<ClienteDto>>(ElJson, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				PaginatedList<ClienteDto> vacio = new();
				return vacio;
			}
			return Clientes;
		}

	}
}
