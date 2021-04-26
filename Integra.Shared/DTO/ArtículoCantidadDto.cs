using Integra.Shared.Domain;
using System.ComponentModel.DataAnnotations;

namespace Integra.Shared.DTO
{
	public class ArtículoCantidadDto 
	{
		public uint ArtículoId { get; set; }
		public decimal Cantidad { get; set; }

	}

}
