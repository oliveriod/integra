using Integra.Shared.DTO;
using Integra.Shared;
using Integra.Web.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Integra.Web.Pages
{
	public class PruebasTypeaheadBase : ComponentBase
	{
		[Inject]
		public IArtículoDataService ArtículoDataService { get; set; }
		[Inject]
		public IClienteDataService ClienteDataService { get; set; }

		[Inject]
		public Blazored.LocalStorage.ILocalStorageService LocalStorage { get; set; }

		[Inject]
		public Microsoft.AspNetCore.Components.NavigationManager UriHelper { get; set; }

		[Parameter]
		public string Page { get; set; } = "1";

		[Parameter]
		public string SearchTerm { get; set; } = string.Empty;


		public ArtículoDto ElArtículo;
		public ClienteDto ElCliente;

		protected override void OnInitialized()
		{

			ElArtículo = new ArtículoDto();
		}

		public async Task<IEnumerable<ArtículoDto>> GetLosArtículosLocal(string searchText)
		{
			if (string.IsNullOrEmpty(searchText))
			{
				IEnumerable<ArtículoDto> vacio = new List<ArtículoDto>();
				return vacio;
			}
			return (await ArtículoDataService.TraerAyuda(searchText));
		}

		public async Task<IEnumerable<ClienteDto>> GetLosClientesLocal(string searchText)
		{
			if (string.IsNullOrEmpty(searchText))
			{
				IEnumerable<ClienteDto> vacio = new List<ClienteDto>();
				return vacio;
			}
			return (await ClienteDataService.TraerAyuda(searchText));
		}

	}
}
