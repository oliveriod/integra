using System;
using System.ComponentModel.DataAnnotations;
using Integra.Shared.Domain;

namespace Integra.Shared.Base
{
	public class EnteBase : DatosBase
	{
		[Required]
		[StringLength(100)]
		public string Nombre { get; set; }

		[StringLength(50)]
		public string PrimerApellido { get; set; }

		[StringLength(50)]
		public string SegundoApellido { get; set; }

		[StringLength(100)]
		[EmailAddress]
		public string Email { get; set; }

		[StringLength(50)]
		public string Teléfono { get; set; }

		[Required]
		public TipoEnteEnum TipoEnteId { get; set; }
		public TipoEnte TipoEnte { get; set; }


		[StringLength(50)]
		public string Identificación { get; set; }

		public string NombreCompleto
		{
			get { return ($"{Nombre} {PrimerApellido} {SegundoApellido}").Trim(); }
		}


	}
}
