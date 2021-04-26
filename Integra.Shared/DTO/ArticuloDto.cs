using Integra.Shared.Base;
using Integra.Shared.Domain;
using System.ComponentModel.DataAnnotations;

namespace Integra.Shared.DTO
{
	public class ArtículoDto : DatosBase
	{
		public int ArtículoId { get; set; }

		[Required]
		[StringLength(20)]
		public string Código { get; set; }

		[Required]
		[StringLength(100)]
		public string Nombre { get; set; }

		[StringLength(500)]
		public string Descripción { get; set; }

		public decimal Precio { get; set; }

		public int ArtículoSubTipoId { get; set; }
		public ArtículoSubTipoDto ArtículoSubTipo { get; set; }

	}

}
