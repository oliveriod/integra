using Integra.Shared.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Integra.Shared.Domain
{
	public class Proveedor : EnteBase
	{
		public uint ProveedorId { get; set; }

		[StringLength(500)]
		public string Descripción { get; set; }

		[StringLength(100)]
		public string Contacto { get; set; }

		public List<ArtículoProveedor> ArtículoProveedors { get; set; }

	}
}
