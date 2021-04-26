using Integra.Shared.Base;
using System;

namespace Integra.Shared.Domain
{
	public class CotizaciónLínea : DatosBase
	{
		public uint CotizaciónLíneaId { get; set; }

		public uint CotizaciónId { get; set; }
		public ushort NúmeroDeLinea { get; set; }


		public int ArtículoId { get; set; }
		public Artículo Artículo { get; set; }

		//public int ProveedorId { get; set; }
		//public Proveedor Proveedor { get; set; }

		public decimal Cantidad { get; set; }
		public decimal PrecioUnitario { get; set; }
		public decimal TotalDeLaLínea { get; set; }

	}

}
