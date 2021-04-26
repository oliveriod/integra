using Integra.Shared.Base;
using Integra.Shared.Domain;
using System.ComponentModel.DataAnnotations;

namespace Integra.Shared.DTO
{
	public class ClienteDto : EnteBase
	{
		public int ClienteId { get; set; }

		[StringLength(500)]
		public string Descripción { get; set; }




	}
}
