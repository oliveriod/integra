using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Integra.DataAccess
{
	public class IntegraDbContextFactory : IDesignTimeDbContextFactory<IntegraDbContext>
	{
		public IntegraDbContext CreateDbContext(string[] args)
		{
			var connectionStringBuilder =
						  new SqliteConnectionStringBuilder { DataSource = ":memory:" };
			var connection = new SqliteConnection(connectionStringBuilder.ToString());



			var optionsBuilder = new DbContextOptionsBuilder<IntegraDbContext>()
				.UseSqlite(connection)
				.Options;

			return new IntegraDbContext(optionsBuilder);
		}
	}
}
