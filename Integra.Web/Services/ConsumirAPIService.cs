using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Blazored.LocalStorage;

namespace Integra.Web.Services
{
	public class ConsumirAPIService : IConsumirAPIService, IInventarioAPI
	{
		private readonly HttpClient _httpClient;

		/// <summary>
		/// 20210313 Nunca se te ocurra quitar httpClient. Si lo quitas no funciona esto.
		/// BTW ya lo hiciste y fue un lío encontrar la solución.
		/// Just DON'T!
		/// </summary>
		/// <param name="httpClient"></param>
		/// <param name="localStorage"></param>
		public ConsumirAPIService(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}
		/// <summary>
		/// Ejecutar un GET (buscar) contra el API
		/// </summary>
		/// <param name="ElUriString"></param>
		/// <param name="Contenido"></param>
		/// <returns></returns>
		public HttpResponseMessage GETRequest(string ElUriString, string Contenido)
		{
			using var miRequestMessage = new HttpRequestMessage(new HttpMethod("GET"), ElUriString);
			{
				miRequestMessage.Content = new StringContent(Contenido);
				miRequestMessage.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

				HttpResponseMessage responseMessage = null;
				try
				{
					responseMessage = _httpClient.Send(miRequestMessage);
				}
				catch (Exception ex)
				{
					if (responseMessage == null)
					{
						responseMessage = new HttpResponseMessage();
					}
					responseMessage.StatusCode = System.Net.HttpStatusCode.InternalServerError;
					responseMessage.ReasonPhrase = ex.Message;
				}
				Console.WriteLine(responseMessage.ToString());
				return responseMessage;
			}
		}

		/// <summary>
		/// Ejecutar un GET (buscar) contra el API
		/// </summary>
		/// <param name="ElUriString"></param>
		/// <param name="Contenido"></param>
		/// <returns></returns>
		public async Task<HttpResponseMessage> GETRequestAsync(string ElUriString, string Contenido)
		{
			using var miRequestMessage = new HttpRequestMessage(new HttpMethod("GET"), ElUriString);
			{
				miRequestMessage.Content = new StringContent(Contenido);
				miRequestMessage.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

				HttpResponseMessage responseMessage = null;
				try
				{
					responseMessage = await _httpClient.SendAsync(miRequestMessage);
				}
				catch (Exception ex)
				{
					if (responseMessage == null)
					{
						responseMessage = new HttpResponseMessage();
					}
					responseMessage.StatusCode = System.Net.HttpStatusCode.InternalServerError;
					responseMessage.ReasonPhrase = ex.Message;
				}
				Console.WriteLine(responseMessage.ToString());
				return responseMessage;
			}
		}

		/// <summary>
		/// Ejecutar un POST (crear) contra el API
		/// </summary>
		/// <param name="ElUriString"></param>
		/// <param name="Contenido"></param>
		/// <returns></returns>
		public async Task<HttpResponseMessage> POSTRequestAsync(string ElUriString, string Contenido)
		{
			using (var miRequestMessage = new HttpRequestMessage(new HttpMethod("POST"), ElUriString))
			{
				miRequestMessage.Content = new StringContent(Contenido);
				miRequestMessage.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

				HttpResponseMessage responseMessage = null;
				try
				{
					responseMessage = await _httpClient.SendAsync(miRequestMessage);
				}
				catch (Exception ex)
				{
					if (responseMessage == null)
					{
						responseMessage = new HttpResponseMessage();
					}
					responseMessage.StatusCode = System.Net.HttpStatusCode.InternalServerError;
					responseMessage.ReasonPhrase = ex.Message;
				}
				Console.WriteLine(responseMessage.ToString());
				return responseMessage;
			}
		}

		/// <summary>
		/// Ejecutar un POST (actualizar) contra el API
		/// </summary>
		/// <param name="ElUriString"></param>
		/// <param name="Contenido"></param>
		/// <returns></returns>
		public async Task<HttpResponseMessage> PUTRequestAsync(string ElUriString, string Contenido)
		{
			//TODO esto es para cuando pongamos el token
			//var token = _localStorage.GetItem<string>("JWT-Token");
			//_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

			using (var miRequestMessage = new HttpRequestMessage(new HttpMethod("PUT"), ElUriString))
			{
				miRequestMessage.Content = new StringContent(Contenido);
				miRequestMessage.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

				HttpResponseMessage responseMessage = null;
				try
				{
					responseMessage = await _httpClient.SendAsync(miRequestMessage);
				}
				catch (Exception ex)
				{
					if (responseMessage == null)
					{
						responseMessage = new HttpResponseMessage();
					}
					responseMessage.StatusCode = System.Net.HttpStatusCode.InternalServerError;
					responseMessage.ReasonPhrase = ex.Message;
				}
				Console.WriteLine(responseMessage.ToString());
				return responseMessage;
			}
		}

		/// <summary>
		/// Ejecutar un DELETE (actualizar) contra el API
		/// </summary>
		/// <param name="ElUriString"></param>
		/// <param name="Contenido"></param>
		/// <returns></returns>
		public async Task<HttpResponseMessage> DELETERequestAsync(string ElUriString, string Contenido)
		{
			//TODO esto es para cuando pongamos el token
			//var token = _localStorage.GetItem<string>("JWT-Token");
			//_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

			using (var miRequestMessage = new HttpRequestMessage(new HttpMethod("DELETE"), ElUriString))
			{
				miRequestMessage.Content = new StringContent(Contenido);
				miRequestMessage.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

				HttpResponseMessage responseMessage = null;
				try
				{
					responseMessage = await _httpClient.SendAsync(miRequestMessage);
				}
				catch (Exception ex)
				{
					if (responseMessage == null)
					{
						responseMessage = new HttpResponseMessage();
					}
					responseMessage.StatusCode = System.Net.HttpStatusCode.InternalServerError;
					responseMessage.ReasonPhrase = ex.Message;
				}
				Console.WriteLine(responseMessage.ToString());
				return responseMessage;
			}
		}

	}
}
