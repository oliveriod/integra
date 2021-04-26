using Integra.Shared.Base;
using Integra.Shared.DTO;
using Integra.Web.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Threading.Tasks;

namespace Integra.Web.Pages
{
	public class PruebasBase : ComponentBase
	{
		[Inject]
		public IArtículoDataService ArtículoDataService { get; set; }

		[Inject]
		public Blazored.LocalStorage.ILocalStorageService LocalStorage { get; set; }

		[Inject]
		public Microsoft.AspNetCore.Components.NavigationManager UriHelper { get; set; }

		[Parameter]
		public string Page { get; set; } = "1";

		[Parameter]
		public string SearchTerm { get; set; } = string.Empty;

//		public PaginatedList<ArtículoDto> LosArtículosFiltrados => ArtículoDataService.TraerVarios(SearchTerm, 1);
		protected PaginatedList<ArtículoDto> LosArtículosFiltrados;


		protected ArtículoDto ElArtículo = new()
		{
			Nombre = "",
			Descripción = "",
			Precio = 0
		};

		protected int ActualArtículoId { get; set; }

		public string ElMensaje { get; set; }
		public string ElToasterMensaje { get; set; }
		protected string ModalTitle { get; set; }
		protected bool EsAdicionar { get; set; }
		protected bool EsVer { get; set; }
		protected bool EsEliminar { get; set; }


		protected async Task BuscarCajaDeBúsquedaKeyPress(KeyboardEventArgs ev)
		{
			if (ev.Key == "Enter")
			{
				await BuscarClick();
			}

		}

		protected async Task BuscarClick()
		{
			if (string.IsNullOrEmpty(SearchTerm))
			{
				LosArtículosFiltrados = (await ArtículoDataService.TraerPáginaAsync("", 1));
				return;
			}
			LosArtículosFiltrados = (await ArtículoDataService.TraerPáginaAsync(SearchTerm, 1));
			StateHasChanged();
		}

		protected async Task Actualizar(int ArtículoId)
		{
			ElArtículo = (await ArtículoDataService.TraerUnoPorIdAsync(ArtículoId));
			ActualArtículoId = ArtículoId;
			this.EsAdicionar = true;
			this.ModalTitle = "Edit Artículo";
		}

		protected void Adicionar()
		{
			this.EsAdicionar = true;
			this.ModalTitle = "Crear Artículo";
		}





		protected void CerrarFormaModal()
		{
			ElArtículo = new ArtículoDto
			{
				Nombre = "",
				Descripción = "",
				Precio = 0
			};
			ActualArtículoId = 0;
			this.EsAdicionar = false;
			this.EsVer = false;
			this.EsEliminar = false;
			StateHasChanged();
		}

		protected async Task Eliminar(int ArtículoId)
		{
			ElArtículo = (await ArtículoDataService.TraerUnoPorIdAsync(ArtículoId));
			ActualArtículoId = ArtículoId;
			this.EsVer = true;
			this.EsEliminar = true;
			this.ModalTitle = "Delete Artículo";
		}

		protected bool IsLoggedOn()
		{
			//            return (localStorage.GetItem<string>("JWT-Token") != String.Empty && DateTime.Now.Ticks < Convert.ToInt64(localStorage.GetItem<string>("JWT-Time-Expire")));
			return true;
		}




		protected async Task Ver(int ArtículoId)
		{
			ElArtículo = (await ArtículoDataService.TraerUnoPorIdAsync(ArtículoId));
			ActualArtículoId = ArtículoId;
			this.EsVer = true;
			this.ModalTitle = "View Artículo";
		}


	}
}
