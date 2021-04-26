using System.Net.Http;
using System.Threading.Tasks;

namespace Integra.Web.Services
{
	public interface IConsumirAPIService
	{
		public HttpResponseMessage GETRequest(string ElUriString, string Contenido);
		public Task<HttpResponseMessage> GETRequestAsync(string ElUriString, string Contenido);
		public Task<HttpResponseMessage> POSTRequestAsync(string ElUriString, string Contenido);
		public Task<HttpResponseMessage> PUTRequestAsync(string ElUriString, string Contenido);
		public Task<HttpResponseMessage> DELETERequestAsync(string ElUriString, string Contenido);
	}
}