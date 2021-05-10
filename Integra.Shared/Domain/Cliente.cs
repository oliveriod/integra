using System.ComponentModel.DataAnnotations;
using Integra.Shared.Base;

namespace Integra.Shared.Domain
{
	public class Cliente : EnteBase
	{
		public uint ClienteId { get; set; }

		[StringLength(500)]
		public string Descripción { get; set; }

		[StringLength(500)]
		public string Dirección { get; set; }

		[StringLength(50)]
		public string Ciudad { get; set; }


	}
}
