using Integra.Shared.Base;
using System.ComponentModel.DataAnnotations;

namespace Integra.Shared.Domain
{

	public class TipoEnte 
	{
		public TipoEnteEnum TipoEnteId { get; set; }

		[Required]
		[StringLength(100)]
		public string Nombre { get; set; }

		[StringLength(500)]
		public string Descripción { get; set; }


	}

}
