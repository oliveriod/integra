using Integra.Shared.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Integra.Shared.DTO
{
	public class ProveedorDto : EnteBase
	{
		public int ProveedorId { get; set; }

		[StringLength(500)]
		public string Descripción { get; set; }

		[StringLength(100)]
		public string Contacto { get; set; }

		public List<ArtículoProveedorDto> ArtículoProveedores { get; set; }

	}
}
