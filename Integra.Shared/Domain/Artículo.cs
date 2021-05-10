using Integra.Shared.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Integra.Shared.Domain
{
	public class Artículo : DatosBase
	{
		public uint ArtículoId { get; set; }

		[Required]
		public ushort EmpresaId { get; set; }
		public Empresa Empresa { get; set; }

		[Required]
		[StringLength(20)]
		public string Código { get; set; }

		[Required]
		[StringLength(100)]
		public string Nombre { get; set; }

		[StringLength(500)]
		public string Descripción { get; set; }

		[Required]
		[Column(TypeName = "money")]
		public decimal Precio { get; set; }

		public int ArtículoSubTipoId { get; set; }
		public ArtículoSubTipo ArtículoSubTipo { get; set; }

		public ushort UnidadId { get; set; }
		public Unidad Unidad { get; set; }

		public List<ArtículoProveedor> ArtículoProveedores { get; set; }
	}

}
