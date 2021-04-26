using Integra.Shared;
using Integra.Shared.Domain;
using System.Linq;

namespace Integra.DataAccess.Repositories
{
	public class RecetaRepository : GenéricoRepository<Receta>, IRecetaRepository
	{
		public RecetaRepository(IntegraDbContext context) : base(context)
		{
		}
	}
}
