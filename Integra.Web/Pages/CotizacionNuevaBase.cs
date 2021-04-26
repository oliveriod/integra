using Integra.Shared.Base;
using Integra.Shared.DTO;
using Integra.Web.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Threading.Tasks;
using Blazored.Toast.Services;
using System.Collections.Generic;

namespace Integra.Web.Pages
{
	public class CotizaciónNuevaBase : ComponentBase
	{
		// Dataservices
		[Inject]
		public IClienteDataService ClienteDataService { get; set; }
		[Inject]
		public ICotizaciónDataService CotizaciónDataService { get; set; }

		// lo otro
		[Inject]
		public Blazored.LocalStorage.ILocalStorageService LocalStorage { get; set; }
		[Inject]
		public Microsoft.AspNetCore.Components.NavigationManager UriHelper { get; set; }
		[Inject]
		public IToastService ToastService { get; set; }

		[Parameter]
		public string Page { get; set; } = "1";

		[Parameter]
		public string SearchTerm { get; set; } = string.Empty;


		protected int ActualCotizaciónId { get; set; }
		protected CotizaciónDto LaCotización = new()
		{
			CotizaciónId = 0,
			ClienteId = 0,
			Total = 0
		};
		protected PaginatedList<CotizaciónLíneaDto> LasCotizacionesLíneas;


		public ClienteDto ElCliente;
		public string ElMensaje { get; set; }
		public string ElTítulo { get; private set; }
		public string ElToasterMensaje { get; set; }
		protected bool EsAdicionar { get; set; }
		protected bool EsEliminar { get; set; }
		protected bool EsVer { get; set; }
		protected string ModalTitle { get; set; }


		protected async Task Actualizar(int CotizaciónId)
		{
			LaCotización = (await CotizaciónDataService.TraerUnoPorIdAsync(CotizaciónId));
			ActualCotizaciónId = CotizaciónId;
			this.EsAdicionar = true;
			this.ModalTitle = "Editar Cotización";
		}

		protected void Adicionar()
		{
			this.EsAdicionar = true;
			this.ModalTitle = "Crear Cotización";
		}



		protected void CerrarFormaModal()
		{
			LaCotización = new CotizaciónDto
			{
				CotizaciónId = 0,
				ClienteId = 0,
				Total = 0
			};
			ActualCotizaciónId = 0;
			this.EsAdicionar = false;
			this.EsVer = false;
			this.EsEliminar = false;
			StateHasChanged();
		}

		protected async Task Eliminar(int CotizaciónId)
		{
			LaCotización = (await CotizaciónDataService.TraerUnoPorIdAsync(CotizaciónId));
			ActualCotizaciónId = CotizaciónId;
			this.EsVer = true;
			this.EsEliminar = true;
			this.ModalTitle = "Eliminar Cotización";
		}

		public async Task<IEnumerable<ClienteDto>> GetLosClientesLocal(string searchText)
		{
			if (string.IsNullOrEmpty(searchText))
			{
				IEnumerable<ClienteDto> vacio = new List<ClienteDto>();
				return vacio;
			}
			return await ClienteDataService.TraerAyuda(searchText);
		}

		protected bool IsLoggedOn()
		{
			//            return (localStorage.GetItem<string>("JWT-Token") != String.Empty && DateTime.Now.Ticks < Convert.ToInt64(localStorage.GetItem<string>("JWT-Time-Expire")));
			return true;
		}


		protected async Task ModificarEnLaBaseDeDatos()
		{
			if (ActualCotizaciónId == 0)
			{
				LaCotización = await CotizaciónDataService.AdicionarAsync(LaCotización);
			}
			else
			{
				if (!EsEliminar)
					LaCotización = await CotizaciónDataService.ActualizarAsync(LaCotización);
				else
					await CotizaciónDataService.EliminarAsync(LaCotización);
			}
			CerrarFormaModal();

			ElMensaje = "Cotización actualizada con éxito";
			ElTítulo = "Cotización Actualizada";
			ToastService.ShowSuccess(ElMensaje);

			await OnParametersSetAsync();

			StateHasChanged();

		}


		protected async Task Ver(int CotizaciónId)
		{
			LaCotización = (await CotizaciónDataService.TraerUnoPorIdAsync(CotizaciónId));
			ActualCotizaciónId = CotizaciónId;
			this.EsVer = true;
			this.ModalTitle = "Ver Cotización";
		}
	}
}
