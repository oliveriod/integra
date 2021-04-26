using Integra.Shared.Base;
using System.ComponentModel.DataAnnotations;

namespace Integra.Shared.Domain
{
	public class Inventario : DatosBase
	{
		public byte BodegaId { get; set; }
		public ushort UbicaciónId { get; set; }
		public uint ArtículoId { get; set; }
		public Artículo Artículo { get; set; }

		public ushort UnidadId { get; set; }
		public Unidad Unidad { get; set; }

		public decimal Cantidad { get; set; }
	}
}
