using System;
using System.ComponentModel.DataAnnotations;

namespace Integra.Shared.Base
{
	public class Secuencia
	{
		[Required]
		[StringLength(100)]
		public string Columna { get; set; }
		public ulong ValorActual { get; set; }
	}

}
