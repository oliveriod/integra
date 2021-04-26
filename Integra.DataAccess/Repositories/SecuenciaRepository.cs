using Integra.Shared;
using Integra.Shared.Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Integra.DataAccess.Repositories
{
	public class SecuenciaRepository 
	{
		private IntegraDbContext _context;

		public SecuenciaRepository(IntegraDbContext context) 
		{
			_context = context;
		}

		public ulong Siguiente(string Columna)
		{
			_context.Database.BeginTransaction();
			var secuencia = _context.Secuencias
				.Single(c => c.Columna == Columna);
			secuencia.ValorActual += 1;

			return secuencia.ValorActual;
		}


 	}
}
