using Integra.Shared.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Integra.Shared.Domain
{
	public class ArtículoSubTipo : DatosBase
	{
		public ushort ArtículoSubTipoId { get; set; }

		[Required]
		public ushort ArtículoTipoId { get; set; }
		public ArtículoTipo ArtículoTipo { get; set; }

		[Required]
		[StringLength(20)]
		public string Código { get; set; }

		[Required]
		[StringLength(100)]
		public string Nombre { get; set; }

		[StringLength(500)]
		public string Descripción { get; set; }

	}


}
