using Integra.Shared.Base;
using Integra.Shared.Domain;
using System;
using System.ComponentModel.DataAnnotations;


namespace Integra.Shared.DTO
{
	public class CotizaciónLíneaDto : DatosBase
	{
		public int CotizaciónLíneaId { get; set; }

		public int CotizaciónId { get; set; }
		public int NúmeroDeLinea { get; set; }


		public int ArtículoId { get; set; }
		public string ArtículoNombre { get; set; }


		//public int ProveedorId { get; set; }
		//public Proveedor Proveedor { get; set; }

		public decimal Cantidad { get; set; }
		public decimal PrecioUnitario { get; set; }
		public decimal TotalDeLaLínea { get; set; }

	}

}
