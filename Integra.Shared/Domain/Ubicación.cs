using Integra.Shared.Base;
using System.ComponentModel.DataAnnotations;

namespace Integra.Shared.Domain
{
	public class Ubicación : DatosBase
	{
		public ushort UbicaciónId { get; set; }

		public ushort BodegaId { get; set; }
		public Bodega Bodega { get; set; }

		[Required]
		[StringLength(20)]
		public string Código { get; set; }

		[StringLength(100)]
		public string Nombre { get; set; }

		[StringLength(500)]
		public string Descripción { get; set; }


	}
}
