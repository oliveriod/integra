using System;
using System.ComponentModel.DataAnnotations;

namespace Integra.Shared.DTO
{
	public class CotizaciónSinLíneasDto
	{
		public int CotizaciónId { get; set; }

		[StringLength(20)]
		public string Codigo { get; set; }

		public int ClienteId { get; set; }
		public string ClienteNombreCompleto { get; set; }

		public decimal Total { get; set; }

		public string ProyectoNombre { get; set; }
		public DateTime FechaCotización { get; set; }
		public string VendedorNombreCompleto { get; set; }
	}
}
