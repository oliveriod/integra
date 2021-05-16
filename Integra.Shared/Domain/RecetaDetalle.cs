using Integra.Shared.Base;
using System;

namespace Integra.Shared.Domain
{
	public class RecetaDetalle : DatosBase
	{
		public ushort RecetaDetalleId { get; set; }
		public ushort RecetaId { get; set; }
		public Receta Receta { get; set; }
		public uint ArtículoId { get; set; }
		public Artículo Artículo { get; set; }

		public decimal Cantidad { get; set; }

	}

}
