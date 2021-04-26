using Integra.Shared.Domain;

namespace Integra.DataAccess.Repositories
{
	public class CotizaciónLíneaRepository : GenéricoRepository<CotizaciónLínea>, ICotizaciónLíneaRepository
	{
		public CotizaciónLíneaRepository(IntegraDbContext context) : base(context)
		{
		}


	}
}
