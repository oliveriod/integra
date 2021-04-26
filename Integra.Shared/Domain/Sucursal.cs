using Integra.Shared.Base;
using System.ComponentModel.DataAnnotations;

namespace Integra.Shared.Domain
{
	public class Sucursal : DatosBase
	{
		public ushort SucursalId { get; set; }

		[Required]
		public byte EmpresaId { get; set; }
		public Empresa Empresa { get; set; }

		[Required]
		public byte  NúmeroDeSucursal { get; set; }

		[Required]
		[StringLength(20)]
		public string Código { get; set; }

		[StringLength(100)]
		public string Nombre { get; set; }

		[StringLength(500)]
		public string Descripción { get; set; }


	}
}
