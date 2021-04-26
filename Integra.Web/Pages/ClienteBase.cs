using Integra.Shared.DTO;
using Integra.Shared.Base;
using Integra.Web.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Threading.Tasks;
using Blazored.Toast.Services;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace Integra.Web.Pages
{
	public class ClienteBase : ComponentBase
	{
		[Inject]
		public ILogger<ClienteBase> Logger { get; set; }

		[Inject]
		public IClienteDataService ClienteDataService { get; set; }

		[Inject]
		public IToastService ToastService { get; set; }

		[Inject]
		public Microsoft.AspNetCore.Components.NavigationManager UriHelper { get; set; }

		[Parameter]
		public string Page { get; set; } = "1";

		[Parameter]
		public string SearchTerm { get; set; } = string.Empty;

		protected PaginatedList<ClienteDto> LosClientes;
		protected ClienteDto ElCliente = new()
		{
			Nombre = "",
			PrimerApellido = "",
			SegundoApellido = ""
		};

		protected int ActualClienteId { get; set; }

		public string ElMensaje { get; set; }
		public string ElTítulo { get; private set; }
		public TipoEnteEnum ElTipoEnteId { get; set; }
		protected string ModalTitle { get; set; }
		protected bool EsAdicionar { get; set; }
		protected bool EsVer { get; set; }
		protected bool EsEliminar { get; set; }


		protected async Task Actualizar(int ClienteId)
		{
			ElCliente = (await ClienteDataService.TraerUnoPorIdAsync(ClienteId));
			ActualClienteId = ClienteId;
			ElTipoEnteId = ElCliente.TipoEnteId;

			this.EsAdicionar = true;
			this.ModalTitle = "Edit Cliente";
		}

		protected void AdicionarEmpresa()
		{
			this.EsAdicionar = true;
			this.ModalTitle = "Crear Cliente";
			ElTipoEnteId = TipoEnteEnum.Empresa;
		}
		protected void AdicionarPersona()
		{
			this.EsAdicionar = true;
			this.ModalTitle = "Crear Cliente";
			ElTipoEnteId = TipoEnteEnum.Persona;
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
			LosClientes = (await ClienteDataService.TraerPáginaAsync("", 1));
			StateHasChanged();
		}

		protected async Task BuscarClick()
		{
			if (string.IsNullOrEmpty(SearchTerm))
			{
				LosClientes = (await ClienteDataService.TraerPáginaAsync("", 1));
				return;
			}
			LosClientes = (await ClienteDataService.TraerPáginaAsync(SearchTerm, 1));
			StateHasChanged();
		}


		protected void CerrarFormaModal()
		{
			ElCliente = new ClienteDto
			{
				Nombre = "",
				PrimerApellido = "",
				SegundoApellido="",
				Descripción = ""

			};
			ActualClienteId = 0;
			this.EsAdicionar = false;
			this.EsVer = false;
			this.EsEliminar = false;
			StateHasChanged();
		}

		protected async Task Eliminar(int ClienteId)
		{
			try
			{
				ElCliente = (await ClienteDataService.TraerUnoPorIdAsync(ClienteId));
				ActualClienteId = ClienteId;
				this.EsVer = true;
				this.EsEliminar = true;
				this.ModalTitle = "Delete Cliente";
			}
			catch (Exception ex)
			{
				Logger.LogError(ex.Message);
				Logger.LogError(ex.StackTrace);
			}
		}

		protected bool IsLoggedOn()
		{
			//            return (localStorage.GetItem<string>("JWT-Token") != String.Empty && DateTime.Now.Ticks < Convert.ToInt64(localStorage.GetItem<string>("JWT-Time-Expire")));
			return true;
		}

		protected async void MiPaginadorPageChanged(int page)
		{
			LosClientes = await ClienteDataService.TraerPáginaAsync(SearchTerm, page);
			StateHasChanged();
		}

		protected async Task ModificarEnLaBaseDeDatos()
		{
			if (ActualClienteId == 0)
			{
				ElCliente.TipoEnteId = ElTipoEnteId;
				ElCliente = await ClienteDataService.AdicionarAsync(ElCliente);
			}
			else
			{
				if (!EsEliminar)
					ElCliente = await ClienteDataService.ActualizarAsync(ElCliente);
				else
					await ClienteDataService.EliminarAsync(ElCliente);
			}
			CerrarFormaModal();

			ElMensaje = "Cliente actualizado con éxito";
			ElTítulo = "Cliente Actualizado";
			ToastService.ShowSuccess(ElMensaje);

			await OnParametersSetAsync();
			LosClientes = (await ClienteDataService.TraerPáginaAsync(SearchTerm, int.Parse(Page)));

			StateHasChanged();

		}

		protected async Task NavegarAPágina(int Página)
		{
			if (string.IsNullOrEmpty(SearchTerm))
				LosClientes = (await ClienteDataService.TraerPáginaAsync("", Página));
			else
				LosClientes = (await ClienteDataService.TraerPáginaAsync(SearchTerm, Página));
			StateHasChanged();
		}

		protected override async Task OnInitializedAsync()
		{
			try
			{
				LosClientes = (await ClienteDataService.TraerPáginaAsync("", 1));
			}
			catch (Exception e)
			{
				Logger.LogError(e, MethodBase.GetCurrentMethod().Name );
				ElMensaje = "Something went wrong. " + e.ToString();
				ToastService.ShowError(ElMensaje);
			}

		}

		protected async Task Ver(int ClienteId)
		{
			ElCliente = (await ClienteDataService.TraerUnoPorIdAsync(ClienteId));
			ActualClienteId = ClienteId;
			this.EsVer = true;
			this.ModalTitle = "View Cliente";
		}
	}
}
