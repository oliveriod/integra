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
	public class VendedorDataService : IVendedorDataService, IInventarioAPI
	{
		private readonly IConsumirAPIService _consumirAPIService;
		private readonly ILogger<VendedorDto> _logger;
		private readonly string _MyStringUri;

		/// <summary>
		/// 20210313 Nunca se te ocurra quitar httpClient. Si lo quitas no funciona esto.
		/// BTW ya lo hiciste y fue un lío encontrar la solución.
		/// Just DON'T!
		/// </summary>
		/// <param name="httpClient"></param>
		/// <param name="consumirAPIService"></param>
		/// <param name="logger"></param>
		public VendedorDataService(HttpClient httpClient, IConsumirAPIService consumirAPIService, ILogger<VendedorDto> logger)
		{
			_consumirAPIService = consumirAPIService;
			_logger = logger;
			_MyStringUri = "api/vendedores";

		}

		/// <summary>
		/// Actualiza un Vendedor
		/// </summary>
		/// <param name="Vendedor"></param>
		/// <returns></returns>
		public async Task<VendedorDto> ActualizarAsync(VendedorDto Vendedor)
		{
			HttpResponseMessage response;

			var elUri = $"{_MyStringUri}/Actualizar";

			string json = JsonSerializer.Serialize(Vendedor);
			try
			{
				response = await _consumirAPIService.PUTRequestAsync(elUri, json);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return null;
			}
			return JsonSerializer.Deserialize<VendedorDto>(response.Content.ReadAsStringAsync().Result, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
			;
		}

		/// <summary>
		/// Crea un vendedor en la base de datos
		/// </summary>
		/// <param name="Vendedor"></param>
		/// <returns></returns>
		public async Task<VendedorDto> AdicionarAsync(VendedorDto Vendedor)
		{
			HttpResponseMessage response;

			var elUri = $"{_MyStringUri}/Actualizar";

			Vendedor.EstadoId = EstadoEnum.Activo;

			string json = JsonSerializer.Serialize(Vendedor);
			try
			{
				response = await _consumirAPIService.POSTRequestAsync(elUri, json);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return null;
			}
			return JsonSerializer.Deserialize<VendedorDto>(response.Content.ReadAsStringAsync().Result, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
			;
		}

		/// <summary>
		/// Eliminar un vendedor
		/// </summary>
		/// <param name="Vendedor"></param>
		/// <returns></returns>
		public async Task<bool> EliminarAsync(VendedorDto Vendedor)
		{
			HttpResponseMessage response;

			var elUri = $"{_MyStringUri}/Eliminar";

			string json = JsonSerializer.Serialize(Vendedor);
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
		/// Buscar vendedores por nombre y devolver IEnumerable
		/// </summary>
		/// <param name="loQueBusco"></param>
		/// <param name="página"></param>
		/// <returns></returns>
		public async Task<IEnumerable<VendedorDto>> TraerAyuda(string loQueBusco)
		{
			HttpResponseMessage response;
			IEnumerable<VendedorDto> Vendedores;

			var elUri = $"{_MyStringUri}/TraerAyuda?loquebusco={loQueBusco}";

			try
			{
				response = await _consumirAPIService.GETRequestAsync(elUri, string.Empty);
				var ElJson = response.Content.ReadAsStringAsync().Result;
				Vendedores = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<VendedorDto>>(ElJson, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				IEnumerable<VendedorDto> vacio = new List<VendedorDto>();
				return vacio;
			}
			return Vendedores;
		}

		/// <summary>
		/// Buscar vendedores por nombre y devolver una página
		/// </summary>
		/// <param name="loQueBusco"></param>
		/// <param name="númeroDePágina"></param>
		/// <param name="tamañoDePágina"></param>
		/// <returns></returns>
		public async Task<PaginatedList<VendedorDto>> TraerPáginaAsync(string loQueBusco, int númeroDePágina = 1, int tamañoDePágina = 10)
		{
			HttpResponseMessage response;
			PaginatedList<VendedorDto> Vendedores;

			var elUri = $"{_MyStringUri}/TraerPagina?loquebusco={loQueBusco}&pagina={númeroDePágina}";

			try
			{
				response = await _consumirAPIService.GETRequestAsync(elUri, string.Empty);
				var ElJson = response.Content.ReadAsStringAsync().Result;
				Vendedores = System.Text.Json.JsonSerializer.Deserialize<PaginatedList<VendedorDto>>(ElJson, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				_logger.LogError(ex.Message);
				PaginatedList<VendedorDto> vacio = new();
				return vacio;
			}
			return Vendedores;
		}

		/// <summary>
		/// Busca un vendedor por Id
		/// </summary>
		/// <param name="vendedorId"></param>
		/// <returns></returns>
		public async Task<VendedorDto> TraerUnoPorIdAsync(int vendedorId)
		{
			HttpResponseMessage response;

			var elUri = $"{_MyStringUri}/TraerUnoPorId/{vendedorId}";

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
			return JsonSerializer.Deserialize<VendedorDto>(response.Content.ReadAsStringAsync().Result, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }); ;
		}

	}
}
