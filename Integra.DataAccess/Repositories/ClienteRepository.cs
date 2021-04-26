using Integra.Shared;
using Integra.Shared.Domain;
using System.Linq;

namespace Integra.DataAccess.Repositories
{
	public class ClienteRepository : GenéricoRepository<Cliente>, IClienteRepository
	{
		public ClienteRepository(IntegraDbContext context) : base(context)
		{
		}

		public override Cliente Adicionar(Cliente entity)
		{
			var secuencia = _context.Secuencias
				.Single(c => c.Columna == "ClienteId");
			secuencia.ValorActual += 1;
			entity.ClienteId = (uint)secuencia.ValorActual;

			return base.Adicionar(entity);
		}
	}
}
