using Integra.Shared.Base;

namespace Integra.Shared.Domain
{
	public class ArtículoProveedor : DatosBase
	{
		public uint ArtículoId { get; set; }
		public Artículo Artículo { get; set; }
		public uint ProveedorId { get; set; }
		public Proveedor Proveedor { get; set; }

		public decimal PrecioUnitario { get; set; }
	}

}
