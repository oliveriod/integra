using System;
using System.ComponentModel.DataAnnotations;
using Integra.Shared.Domain;

namespace Integra.Shared.Base
{
	public class DatosBase
	{
		[Required]
		public EstadoEnum EstadoId { get; set; }
		public Estado Estado { get; set; }

		public DateTime FechaDeCreación { get; set; }
		public DateTime FechaDeModificación { get; set; }
	}

}
