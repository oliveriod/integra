using Integra.Shared.Base;
using Integra.Shared.Domain;
using System.ComponentModel.DataAnnotations;

namespace Integra.Shared.DTO
{
	public class InventarioDto : DatosBase
	{
		public ushort BodegaId { get; set; }
		public ushort UbicaciónId { get; set; }
		public uint ArtículoId { get; set; }
		public string ArtículoNombre { get; set; }
		public Artículo Artículo { get; set; }

		public ushort UnidadId { get; set; }
		public Unidad Unidad { get; set; }

		public decimal Cantidad { get; set; }
	}
}
