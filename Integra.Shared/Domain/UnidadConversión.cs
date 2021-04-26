using Integra.Shared.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Integra.Shared.Domain
{

	public class UnidadConversión : DatosBase
	{
		public ushort UnidadOrigenId { get; set; }
		public ushort UnidadDestinoId { get; set; }

		public double FactorDeConversión { get; set; }

		[StringLength(20)]
		public string Código { get; set; }

		[Required]
		[StringLength(100)]
		public string Nombre { get; set; }

	}

}
