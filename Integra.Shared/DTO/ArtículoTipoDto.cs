using Integra.Shared.Base;
using System.ComponentModel.DataAnnotations;

namespace Integra.Shared.DTO
{

	public class ArtículoTipoDto
	{
		public int ArtículoTipoId { get; set; }

		[Required]
		[StringLength(20)]
		public string Código { get; set; }

		[Required]
		[StringLength(100)]
		public string Nombre { get; set; }

		[StringLength(500)]
		public string Descripción { get; set; }

		[Required]
		public EstadoEnum Estado { get; set; }
	}

}

