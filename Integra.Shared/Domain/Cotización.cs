using Integra.Shared.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Integra.Shared.Domain
{
	public class Cotización : DatosBase
	{
		public uint CotizaciónId { get; set; }
		public DateTime FechaCotización { get; set; }

		[StringLength (20) ]
		public string Código { get; set; }

		public int ClienteId { get; set; }
		public Cliente Cliente { get; set; }

		public int CantidadDeLíneas { get; set; }
		public decimal Total { get; set; }

		public int ProyectoId { get; set; }
		public Proyecto Proyecto { get; set; }

		public int VendedorId { get; set; }
		public Vendedor Vendedor { get; set; }


		[StringLength(200)]
		public string Comentario { get; set; }

		public ICollection<CotizaciónLínea> CotizaciónLíneas { get; set; }
	}
}
