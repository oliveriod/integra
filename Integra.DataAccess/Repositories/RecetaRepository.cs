using Integra.Shared;
using Integra.Shared.Domain;
using System.Linq;

namespace Integra.DataAccess.Repositories
{
	public class RecetaRepository : Gen√©ricoRepository<Receta>, IRecetaRepository
	{
		public RecetaRepository(IntegraDbContext context) : base(context)
		{
		}
	}
}
