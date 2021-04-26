using Integra.Shared.Base;
using Integra.Shared.DTO;
using Integra.Web.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Threading.Tasks;
using Blazored.Toast.Services;
using System.Collections.Generic;
using System.Linq;

namespace Integra.Web.Pages
{
	public class ArticuloBase : ComponentBase
	{
		[Inject]
		public IArtículoDataService ArtículoDataService { get; set; }
		[Inject]
		public IArtículoSubTipoDataService ArtículoSubTipoDataService { get; set; }
		[Inject]
		public IArtículoTipoDataService ArtículoTipoDataService { get; set; }
		[Inject]
		public Blazored.LocalStorage.ILocalStorageService LocalStorage { get; set; }
		[Inject]
		public IToastService ToastService { get; set; }
		[Inject]
		public Microsoft.AspNetCore.Components.NavigationManager UriHelper { get; set; }

		[Parameter]
		public string Page { get; set; } = "1";

		[Parameter]
		public string SearchTerm { get; set; } = string.Empty;

		// Artículo
		protected ArtículoDto ElArtículo = new()
		{
			Nombre = "",
			Descripción = "",
			Precio = 0
		};
		protected PaginatedList<ArtículoDto> LosArtículos;
		protected int ActualArtículoId { get; set; }


		// ArtículoTipo
		protected ArtículoTipoDto ElArtículoTipo = new()
		{
			Código = "",
			Nombre = "",
			Descripción = ""

		};
		protected IEnumerable<ArtículoTipoDto> LosArtículoTipos;
		protected IEnumerable<ArtículoSubTipoDto> LosArtículoSubTipos;
		protected int ActualArtículoTipoId { get; set; }
		protected string ActualArtículoTipoNombre { get; set; }
		protected int ActualArtículoSubTipoId { get; set; }
		protected string ActualArtículoSubTipoNombre { get; set; }



		public string ElMensaje { get; set; }
		public string ElTítulo { get; set; }

		public string ElToasterMensaje { get; set; }
		protected string ModalTitle { get; set; }
		protected bool EsAdicionar { get; set; }
		protected bool EsVer { get; set; }
		protected bool EsEliminar { get; set; }




		protected async Task Actualizar(int ArtículoId)
		{
			ElArtículo = (await ArtículoDataService.TraerUnoPorIdAsync(ArtículoId));
			ActualArtículoTipoId = ElArtículo.ArtículoSubTipo.ArtículoTipo.ArtículoTipoId;
			ActualArtículoTipoNombre = ElArtículo.ArtículoSubTipo.ArtículoTipo.Nombre;
			ActualArtículoSubTipoId = ElArtículo.ArtículoSubTipo.ArtículoSubTipoId;
			ActualArtículoSubTipoNombre = ElArtículo.ArtículoSubTipo.Nombre;

			LosArtículoSubTipos = await ArtículoSubTipoDataService.TraerAyuda(ActualArtículoTipoId, "");

			ActualArtículoId = ArtículoId;
			this.EsAdicionar = true;
			this.ModalTitle = "Edit Artículo";
		}

		protected void Adicionar()
		{
			this.EsAdicionar = true;
			this.ModalTitle = "Crear Artículo";
		}

		protected async Task BuscarCajaDeBúsquedaKeyPress(KeyboardEventArgs ev)
		{
			if (ev.Key == "Enter")
			{
				await BuscarClick();
			}
		}

		protected async Task BuscarCajaDeBúsquedaLimpiar()
		{
			SearchTerm = string.Empty;
			LosArtículos = await ArtículoDataService.TraerPáginaAsync("", 1);
			StateHasChanged();
		}

		protected async Task BuscarClick()
		{
			if (string.IsNullOrEmpty(SearchTerm))
			{
				LosArtículos = (await ArtículoDataService.TraerPáginaAsync("", 1));
				return;
			}
			LosArtículos = (await ArtículoDataService.TraerPáginaAsync(SearchTerm, 1));
			StateHasChanged();
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
			this.ModalTitle = "Eliminar Artículo";
		}

		public async Task<IEnumerable<ArtículoSubTipoDto>> GetLosArtículoSubTipoLocal(string searchText)
		{
			if (string.IsNullOrEmpty(searchText))
			{
				IEnumerable<ArtículoSubTipoDto> vacio = new List<ArtículoSubTipoDto>();
				return vacio;
			}
			return await ArtículoSubTipoDataService.TraerAyuda(ActualArtículoTipoId, searchText);
		}

		public async Task<IEnumerable<ArtículoTipoDto>> GetLosArtículoTipoLocal(string searchText)
		{
			if (string.IsNullOrEmpty(searchText))
			{
				IEnumerable<ArtículoTipoDto> vacio = new List<ArtículoTipoDto>();
				return vacio;
			}
			return (await ArtículoTipoDataService.TraerAyuda(searchText));
		}

		protected bool IsLoggedOn()
		{
			//            return (localStorage.GetItem<string>("JWT-Token") != String.Empty && DateTime.Now.Ticks < Convert.ToInt64(localStorage.GetItem<string>("JWT-Time-Expire")));
			return true;
		}

		protected async void MiPaginadorPageChanged(int page)
		{
			LosArtículos = await ArtículoDataService.TraerPáginaAsync(SearchTerm, page);
			StateHasChanged();
		}

		protected async Task ModificarEnLaBaseDeDatos()
		{

			if (ActualArtículoId == 0)
			{
				ElArtículo = await ArtículoDataService.AdicionarAsync(ElArtículo);
			}
			else
			{
				if (!EsEliminar)
					ElArtículo = await ArtículoDataService.ActualizarAsync(ElArtículo);
				else
					await ArtículoDataService.EliminarAsync(ElArtículo);
			}
			CerrarFormaModal();

			await OnParametersSetAsync();

			ElMensaje = "Artículo actualizado con éxito";
			ElTítulo = "Artículo Actualizado";
			ToastService.ShowSuccess(ElMensaje);

			LosArtículos = (await ArtículoDataService.TraerPáginaAsync(SearchTerm, int.Parse(Page)));

			StateHasChanged();

		}

		protected async Task NavegarAPágina(int Página)
		{
			if (string.IsNullOrEmpty(SearchTerm))
				LosArtículos = (await ArtículoDataService.TraerPáginaAsync("", Página));
			else
				LosArtículos = (await ArtículoDataService.TraerPáginaAsync(SearchTerm, Página));
			StateHasChanged();
		}

		protected override async Task OnInitializedAsync()
		{
			try
			{
				LosArtículoTipos = (await ArtículoTipoDataService.TraerTodosAsync());
				LosArtículos = (await ArtículoDataService.TraerPáginaAsync("", 1));
			}
			catch (Exception e)
			{
				ElMensaje = "Algo salió mal. " + e.ToString();
				ToastService.ShowError(ElMensaje);
			}

		}


		protected async Task Ver(int ArtículoId)
		{
			ElArtículo = (await ArtículoDataService.TraerUnoPorIdAsync(ArtículoId));
			ActualArtículoId = ArtículoId;
			this.EsVer = true;
			this.ModalTitle = "Ver Artículo";
		}


		protected async void ArtículoTipoClicked(ChangeEventArgs artículoTipoEvent)
		{
			ActualArtículoSubTipoNombre = string.Empty;
			var tempo = artículoTipoEvent.Value.ToString();
			if (!string.IsNullOrEmpty(tempo))
			{
				ActualArtículoTipoId = int.Parse(tempo);
				ActualArtículoTipoNombre = LosArtículoTipos.FirstOrDefault(s => s.ArtículoTipoId == ActualArtículoTipoId).Nombre;
				LosArtículoSubTipos = await ArtículoSubTipoDataService.TraerAyuda(ActualArtículoTipoId, "");
				this.StateHasChanged();
			}
		}

		protected void ArtículoSubTipoClicked(ChangeEventArgs artículoSubTipoEvent)
		{
			var tempo = artículoSubTipoEvent.Value.ToString();
			ActualArtículoSubTipoId = int.Parse(tempo);
			if (!string.IsNullOrEmpty(tempo))
			{
				var ElArtículoSubTipo = LosArtículoSubTipos.FirstOrDefault(s => s.ArtículoSubTipoId == ActualArtículoSubTipoId);
				ActualArtículoSubTipoNombre = ElArtículoSubTipo.Nombre;
				ElArtículo.ArtículoSubTipoId = ActualArtículoSubTipoId;
				ElArtículo.ArtículoSubTipo = ElArtículoSubTipo;
				this.StateHasChanged();
			}
		}

	}
}
