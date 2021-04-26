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
	public class ProyectoDataService : IProyectoDataService, IInventarioAPI
	{
		private readonly IConsumirAPIService _consumirAPIService;
		private readonly ILogger<ProyectoDto> _logger;
		private readonly string _MyStringUri;

		/// <summary>
		/// 20210313 Nunca se te ocurra quitar httpClient. Si lo quitas no funciona esto.
		/// BTW ya lo hiciste y fue un lío encontrar la solución.
		/// Just DON'T!
		/// </summary>
		/// <param name="httpClient"></param>
		/// <param name="consumirAPIService"></param>
		/// <param name="logger"></param>
		public ProyectoDataService(HttpClient httpClient, IConsumirAPIService consumirAPIService, ILogger<ProyectoDto> logger)
		{
			_consumirAPIService = consumirAPIService;
			_logger = logger;
			_MyStringUri = "api/proyectos";

		}

		/// <summary>
		/// Actualiza un Proyecto
		/// </summary>
		/// <param name="Proyecto"></param>
		/// <returns></returns>
		public async Task<ProyectoDto> ActualizarAsync(ProyectoDto Proyecto)
		{
			HttpResponseMessage response;

			var elUri = $"{_MyStringUri}/Actualizar";

			string json = JsonSerializer.Serialize(Proyecto);
			try
			{
				response = await _consumirAPIService.PUTRequestAsync(elUri, json);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return null;
			}
			return JsonSerializer.Deserialize<ProyectoDto>(response.Content.ReadAsStringAsync().Result, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
			;
		}

		/// <summary>
		/// Crea un Proyecto en la base de datos
		/// </summary>
		/// <param name="Proyecto"></param>
		/// <returns></returns>
		public async Task<ProyectoDto> AdicionarAsync(ProyectoDto Proyecto)
		{
			HttpResponseMessage response;

			var elUri = $"{_MyStringUri}/Adicionar";

			Proyecto.EstadoId = EstadoEnum.Activo;

			string json = JsonSerializer.Serialize(Proyecto);
			try
			{
				response = await _consumirAPIService.POSTRequestAsync(elUri, json);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return null;
			}
			return JsonSerializer.Deserialize<ProyectoDto>(response.Content.ReadAsStringAsync().Result, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
			;
		}

		/// <summary>
		/// Eliminar un proyecto
		/// </summary>
		/// <param name="Proyecto"></param>
		/// <returns></returns>
		public async Task<bool> EliminarAsync(ProyectoDto Proyecto)
		{
			HttpResponseMessage response;

			var elUri = $"{_MyStringUri}/Eliminar";

			string json = JsonSerializer.Serialize(Proyecto);
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
		/// Buscar proyectos por nombre y devolver IEnumerable
		/// </summary>
		/// <param name="loQueBusco"></param>
		/// <param name="página"></param>
		/// <returns></returns>
		public async Task<IEnumerable<ProyectoDto>> TraerAyuda(string loQueBusco)
		{
			HttpResponseMessage response;
			IEnumerable<ProyectoDto> Proyectos;

			var elUri = $"{_MyStringUri}/TraerAyuda?loquebusco={loQueBusco}";

			try
			{
				response = await _consumirAPIService.GETRequestAsync(elUri, string.Empty);
				var ElJson = response.Content.ReadAsStringAsync().Result;
				Proyectos = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<ProyectoDto>>(ElJson, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				IEnumerable<ProyectoDto> vacio = new List<ProyectoDto>();
				return vacio;
			}
			return Proyectos;
		}

		/// <summary>
		/// Busca un Proyecto por Id
		/// </summary>
		/// <param name="proyectoId"></param>
		/// <returns></returns>
		public async Task<ProyectoDto> TraerUnoPorIdAsync(int proyectoId)
		{
			HttpResponseMessage response;

			var elUri = $"{_MyStringUri}/TraerUnoPorId/{proyectoId}";

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
			return JsonSerializer.Deserialize<ProyectoDto>(response.Content.ReadAsStringAsync().Result, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }); ;
		}

		/// <summary>
		/// Buscar proyectos por nombre y devolver una página
		/// </summary>
		/// <param name="loQueBusco"></param>
		/// <param name="página"></param>
		/// <returns></returns>
		public async Task<PaginatedList<ProyectoDto>> TraerPáginaAsync(string loQueBusco, int númeroDePágina = 1, int tamañoDePágina = 10)
		{
			HttpResponseMessage response;
			PaginatedList<ProyectoDto> Proyectos;

			var elUri = $"{_MyStringUri}/TraerPagina?loquebusco={loQueBusco}&pagina={númeroDePágina}&cuantospp={tamañoDePágina}";

			try
			{
				response = await _consumirAPIService.GETRequestAsync(elUri, string.Empty);
				var ElJson = response.Content.ReadAsStringAsync().Result;
				Proyectos = System.Text.Json.JsonSerializer.Deserialize<PaginatedList<ProyectoDto>>(ElJson, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				PaginatedList<ProyectoDto> vacio = new();
				return vacio;
			}
			return Proyectos;
		}

	}
}
