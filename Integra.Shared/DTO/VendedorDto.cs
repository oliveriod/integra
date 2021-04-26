using Integra.Shared.Base;
using Integra.Shared.Domain;
using System.ComponentModel.DataAnnotations;

namespace Integra.Shared.DTO
{
	public class VendedorDto : EnteBase
	{
		public int VendedorId { get; set; }

		[StringLength(500)]
		public string Descripción { get; set; }


	}
}
