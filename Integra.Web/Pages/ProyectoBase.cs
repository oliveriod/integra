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
	public class ProyectoBase : ComponentBase
	{
        [Inject]
        public IProyectoDataService ProyectoDataService { get; set; }
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

        protected PaginatedList<ProyectoDto> LosProyectos;
        protected ProyectoDto ElProyecto = new()
		{
            Nombre = "",
            Código = "",
            Descripción = ""
        };

		protected int ActualProyectoId { get; set; }

		public string ElMensaje { get; set; }
		public string ElTítulo { get; private set; }
		public string ElToasterMensaje { get; set; }
		protected string ModalTitle { get; set; }
        protected bool EsAdicionar { get; set; }
        protected bool EsVer { get; set; }
        protected bool EsEliminar { get; set; }

		public ClienteDto ElCliente;


		protected async Task Actualizar(int ProyectoId)
		{
			ElProyecto = (await ProyectoDataService.TraerUnoPorIdAsync(ProyectoId));
			ActualProyectoId = ProyectoId;
			ElCliente = ElProyecto.Cliente;
			this.EsAdicionar = true;
			this.ModalTitle = "Edit Proyecto";
		}

		protected void Adicionar()
		{
			this.EsAdicionar = true;
			this.ModalTitle = "Crear Proyecto";
			ActualProyectoId = 0;
			ElCliente = new();
			ElProyecto = new()
			{
				Nombre = "",
				Código = "",
				ClienteId=0,
				ProyectoId=0,
				Descripción = ""
			};

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
			LosProyectos = (await ProyectoDataService.TraerPáginaAsync("", 1));
			StateHasChanged();
		}

		protected async Task BuscarClick()
		{
			if (string.IsNullOrEmpty(SearchTerm))
			{
				LosProyectos = (await ProyectoDataService.TraerPáginaAsync("", 1));
				return;
			}
			LosProyectos = (await ProyectoDataService.TraerPáginaAsync(SearchTerm, 1));
			StateHasChanged();
		}

		protected void CerrarFormaModal()
		{
			ElProyecto = new ProyectoDto
			{
				Nombre = "",
				Código = "",
				Descripción = ""

			};
			ActualProyectoId = 0;
			this.EsAdicionar = false;
			this.EsVer = false;
			this.EsEliminar = false;
			StateHasChanged();
		}

		protected void HaCambiadoElCliente()
		{
			ElProyecto.ClienteId = ElCliente.ClienteId;
		}
		protected async Task Eliminar(int ProyectoId)
		{
			ElProyecto = (await ProyectoDataService.TraerUnoPorIdAsync(ProyectoId));
			ActualProyectoId = ProyectoId;
			this.EsVer = true;
			this.EsEliminar = true;
			this.ModalTitle = "Delete Proyecto";
		}

		protected bool IsLoggedOn()
		{
			//            return (localStorage.GetItem<string>("JWT-Token") != String.Empty && DateTime.Now.Ticks < Convert.ToInt64(localStorage.GetItem<string>("JWT-Time-Expire")));
			return true;
		}

		protected async void MiPaginadorPageChanged(int page)
		{
			LosProyectos = await ProyectoDataService.TraerPáginaAsync(SearchTerm, page);
			StateHasChanged();
		}

		protected async Task ModificarEnLaBaseDeDatos()
		{
			if (ActualProyectoId == 0)
			{
				ElProyecto.ClienteId = ElCliente.ClienteId;
				ElProyecto.EstadoId = EstadoEnum.Activo;
				ElProyecto = await ProyectoDataService.AdicionarAsync(ElProyecto);
			}
			else
			{
				if (!EsEliminar)
					ElProyecto = await ProyectoDataService.ActualizarAsync(ElProyecto);
				else
					await ProyectoDataService.EliminarAsync(ElProyecto);
			}
			CerrarFormaModal();

			ElMensaje = "Proyecto actualizado con éxito";
			ElTítulo = "Proyecto Actualizado";
			ToastService.ShowSuccess(ElMensaje);

			await OnParametersSetAsync();
			LosProyectos = (await ProyectoDataService.TraerPáginaAsync(SearchTerm, int.Parse(Page)));

			StateHasChanged();

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

		protected async Task NavegarAPágina(int Página)
		{
			if (string.IsNullOrEmpty(SearchTerm))
				LosProyectos = (await ProyectoDataService.TraerPáginaAsync("", Página));
			else
				LosProyectos = (await ProyectoDataService.TraerPáginaAsync(SearchTerm, Página));
			StateHasChanged();
		}

		protected override async Task OnInitializedAsync()
		{
			try
			{
				LosProyectos = (await ProyectoDataService.TraerPáginaAsync("", 1));
			}
			catch (Exception e)
			{
				ElMensaje = "Something went wrong. " + e.ToString();
				ToastService.ShowError(ElMensaje);
			}

		}


		protected async Task Ver(int ProyectoId)
		{
			ElProyecto = (await ProyectoDataService.TraerUnoPorIdAsync(ProyectoId));
			ActualProyectoId = ProyectoId;
			this.EsVer = true;
			this.ModalTitle = "View Proyecto";
		}
	}
}
