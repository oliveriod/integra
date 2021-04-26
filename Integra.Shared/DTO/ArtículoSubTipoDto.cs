using Integra.Shared.Base;
using Integra.Shared.Domain;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Integra.Shared.DTO
{
	public class ArtículoSubTipoDto
	{
		public int ArtículoSubTipoId { get; set; }

		[Required]
		public int ArtículoTipoId { get; set; }
		public ArtículoTipo ArtículoTipo { get; set; }

		[Required]
		[StringLength(20)]
		public string Código { get; set; }

		[Required]
		[StringLength(100)]
		public string Nombre { get; set; }

		[StringLength(500)]
		public string Descripción { get; set; }

		[Required]
		public EstadoEnum EstadoId { get; set; }

	}


}
