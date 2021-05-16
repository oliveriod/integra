using Integra.Shared.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Integra.Shared.Domain
{
	public class Receta : DatosBase
	{
		public ushort RecetaId { get; set; }

		[StringLength(20)]
		public string Código { get; set; }

		[Required]
		[StringLength(100)]
		public string Nombre { get; set; }

		[StringLength(500)]
		public string Descripción { get; set; }

		/// <summary>
		/// Artículo procesado
		/// </summary>
		public uint ArtículoId { get; set; }
		public Artículo Artículo { get; set; }

		[Required]
		public decimal CantidadProducida { get; set; }

		/// <summary>
		/// Insumos
		/// </summary>
		public ICollection<RecetaDetalle> RecetaDetalles { get; set; }
	}

}
