using Integra.Shared.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Integra.Shared.Domain
{
	public class AcciónDeInventario : DatosBase
	{
		public ulong AcciónDeInventarioId { get; set; }
		public DateTime Fecha { get; set; }

		public TipoAcciónDeInventarioEnum TipoAcciónDeInventarioId;

		public EstadoAcciónDeInventarioEnum EstadoAcciónDeInventarioId;

		[Required]
		public sbyte Signo;

		[StringLength (20) ]
		public string Código { get; set; }

		public uint ClienteId { get; set; }
		public Cliente Cliente { get; set; }

		public ushort CantidadDeLíneas { get; set; }
		public decimal Total { get; set; }

		public ushort VendedorId { get; set; }
		public Vendedor Vendedor { get; set; }

		public ushort BodegaId { get; set; }
		public Bodega Bodega { get; set; }

		[StringLength(200)]
		public string Comentario { get; set; }

		public ICollection<AcciónDeInventarioDetalle> AcciónDeInventarioDetalles { get; set; }
	}
}
