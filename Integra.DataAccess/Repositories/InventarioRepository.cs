using Integra.Shared;
using Integra.Shared.Domain;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Integra.DataAccess.Repositories
{
	public class InventarioRepository : Gen√©ricoRepository<Inventario>, IInventarioRepository
	{
		public InventarioRepository(IntegraDbContext context) : base(context)
		{
		}
		public override Inventario TraerUnoPorId (ulong id)
		{
			throw new NotImplementedException();
		}

	}
}
