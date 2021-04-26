using Integra.Shared.Base;

namespace Integra.Shared.DTO
{
	public class ArtículoProveedorDto : DatosBase
	{
		public int ArtículoId { get; set; }
		public ArtículoDto Artículo { get; set; }
		public int ProveedorId { get; set; }
		public ProveedorDto Proveedor { get; set; }

		public decimal PrecioUnitario { get; set; }
	}

}
