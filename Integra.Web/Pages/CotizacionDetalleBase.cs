using Integra.Shared.DTO;
using Integra.Web.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System;
using System.Linq;

namespace Integra.Web.Pages
{
	public class CotizacionDetalleBase : ComponentBase
	{
		[Inject]
		public ICotizaciónDataService CotizaciónDataService { get; set; }

		public CotizaciónDto Cotización { get; set; }
        public string Message { get; set; }

        [Parameter]
        public int CotizaciónId { get; set; }




        protected override async Task OnInitializedAsync()
        {
            try
            {
                Cotización = await CotizaciónDataService.TraerUnoPorIdAsync(CotizaciónId);
            }
            catch (Exception e)
            {
                Message = "Something went wrong. " + e.ToString();
            }

        }
    }
}
