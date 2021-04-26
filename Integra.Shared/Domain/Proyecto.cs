using Integra.Shared.Base;
using System.ComponentModel.DataAnnotations;

namespace Integra.Shared.Domain
{
	public class Proyecto : DatosBase
	{
		public int ProyectoId { get; set; }

		[Required]
		[StringLength(20)]
		public string Código { get; set; }

		[StringLength(100)]
		public string Nombre { get; set; }

		[StringLength(500)]
		public string Descripción { get; set; }

		public int ClienteId { get; set; }
		public Cliente Cliente { get; set; }

	}
}
