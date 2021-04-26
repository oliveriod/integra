using Integra.Shared.Base;

namespace Integra.Shared.Domain
{
	public class AcciónDeInventarioDetalle : DatosBase
	{
		public ulong AcciónDeInventarioId { get; set; }
		public ushort NúmeroDeLinea { get; set; }


		public uint ArtículoId { get; set; }
		public Artículo Artículo { get; set; }

		public decimal Cantidad { get; set; }
		public decimal PrecioUnitario { get; set; }
		public decimal TotalDeLaLínea { get; set; }

	}

}
