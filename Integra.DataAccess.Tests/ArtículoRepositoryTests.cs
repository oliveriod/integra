using Integra.Shared.Domain;
using Integra.DataAccess.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using Integra.Shared.Base;

namespace Integra.DataAccess.Tests
{
	public class ArtículoRepositoryTests
	{
		private readonly ITestOutputHelper _output;

		public ArtículoRepositoryTests(ITestOutputHelper output)
		{
			_output = output;
		}


		[Fact]
		public void InsertarArtículo_NadaRaro_Retorna_Id1()
		{
			// Arrange
			var connectionStringBuilder =
				new SqliteConnectionStringBuilder { DataSource = ":memory:" };
			var connection = new SqliteConnection(connectionStringBuilder.ToString());

			var options = new DbContextOptionsBuilder<IntegraDbContext>()
				.UseSqlite(connection)
				.Options;

			using (var context = new IntegraDbContext(options))
			{
				context.Database.OpenConnection();
				context.Database.EnsureCreated();
			}

			using (var context = new IntegraDbContext(options))
			{
				var repositorio = new ArtículoRepository(context);

				//	Act
				repositorio.Adicionar(new Artículo { ArtículoSubTipoId = 1, UnidadId = 11, Código = "LEC001", Nombre = "Leche Entera", EstadoId = EstadoEnum.Activo });
				repositorio.SaveChanges();

				var objeto = repositorio.BuscarPrimero(artículo => artículo.Nombre == "Leche Entera");

				//	Assert
				Assert.Equal((uint) 1, objeto.ArtículoId);
			}
		}

		[Fact]
		public void ObtenerArtículos_TamañoDePáginaEsDiez_RetornaDiezArtículos()
		{
			// Arrange

			var connectionStringBuilder =
				new SqliteConnectionStringBuilder { DataSource = ":memory:" };
			var connection = new SqliteConnection(connectionStringBuilder.ToString());

			var options = new DbContextOptionsBuilder<IntegraDbContext>()
				.UseSqlite(connection)
				.Options;

			using (var context = new IntegraDbContext(options))
			{
				context.Database.OpenConnection();
				context.Database.EnsureCreated();

				var repositorio = new ArtículoRepository(context);

				repositorio.Adicionar(new Artículo() { Código = "TPVC003", UnidadId = 1, Nombre = "Tubo de PVC de 3 pulgadas", ArtículoSubTipoId = 1 });
				repositorio.Adicionar(new Artículo() { Código = "TPVC004", UnidadId = 1, Nombre = "Tubo de PVC de 4 pulgadas", ArtículoSubTipoId = 1 });
				repositorio.Adicionar(new Artículo() { Código = "TJL004", UnidadId = 1, Nombre = "Tejalit de 4 pies", ArtículoSubTipoId = 1 });
				repositorio.Adicionar(new Artículo() { Código = "TJL006", UnidadId = 1, Nombre = "Tejalit de 6 pies", ArtículoSubTipoId = 1 });
				repositorio.Adicionar(new Artículo() { Código = "TJL008", UnidadId = 1, Nombre = "Tejalit de 8 pies", ArtículoSubTipoId = 1 });
				repositorio.Adicionar(new Artículo() { Código = "TJL010", UnidadId = 1, Nombre = "Tejalit de 10 pies", ArtículoSubTipoId = 1 });
				repositorio.Adicionar(new Artículo() { Código = "TJL012", UnidadId = 1, Nombre = "Tejalit de 12 pies", ArtículoSubTipoId = 1 });
				repositorio.Adicionar(new Artículo() { Código = "TJL014", UnidadId = 1, Nombre = "Tejalit de 14 pies", ArtículoSubTipoId = 1 });
				repositorio.Adicionar(new Artículo() { Código = "TJL015", UnidadId = 1, Nombre = "Tejalit de 16 pies", ArtículoSubTipoId = 1 });
				repositorio.Adicionar(new Artículo() { Código = "TJL016", UnidadId = 1, Nombre = "Tejalit de 18 pies", ArtículoSubTipoId = 1 });
				repositorio.Adicionar(new Artículo() { Código = "TJL020", UnidadId = 1, Nombre = "Tejalit de 20 pies", ArtículoSubTipoId = 1 });

				context.SaveChanges();

				context.SaveChanges();
			}

			using (var context = new IntegraDbContext(options))
			{
				var repositorio = new ArtículoRepository(context);

				//	Act
				var objetos = repositorio.TraerVariosAsync(1, 10);

				//	Assert
				Assert.Equal(10, objetos.Result.LosRegistros.Count);
			}
		}

		[Fact]
		public void ObtenerArtículos_EmpiezanConTejalit_RetornaCincoArtículos()
		{
			// Arrange

			var connectionStringBuilder =
				new SqliteConnectionStringBuilder { DataSource = ":memory:" };
			var connection = new SqliteConnection(connectionStringBuilder.ToString());

			var options = new DbContextOptionsBuilder<IntegraDbContext>()
				.UseSqlite(connection)
				.Options;

			using (var context = new IntegraDbContext(options))
			{
				context.Database.OpenConnection();
				context.Database.EnsureCreated();

				var repositorio = new ArtículoRepository(context);

				repositorio.Adicionar(new Artículo() {Código = "TPVC003", UnidadId=1, Nombre = "Tubo de PVC de 3 pulgadas", ArtículoSubTipoId=1 });
				repositorio.Adicionar(new Artículo() {Código = "TPVC004", UnidadId = 1, Nombre = "Tubo de PVC de 4 pulgadas", ArtículoSubTipoId = 1 });
				repositorio.Adicionar(new Artículo() {Código = "TJL004", UnidadId=1, Nombre = "Tejalit de 4 pies", ArtículoSubTipoId = 1 });
				repositorio.Adicionar(new Artículo() {Código = "TJL006", UnidadId=1, Nombre = "Tejalit de 6 pies", ArtículoSubTipoId = 1 });
				repositorio.Adicionar(new Artículo() {Código = "TJL008", UnidadId=1, Nombre = "Tejalit de 8 pies", ArtículoSubTipoId = 1 });
				repositorio.Adicionar(new Artículo() {Código = "TJL010", UnidadId=1, Nombre = "Tejalit de 10 pies", ArtículoSubTipoId = 1 });
				repositorio.Adicionar(new Artículo() {Código = "TJL012", UnidadId = 1, Nombre = "Tejalit de 12 pies", ArtículoSubTipoId = 1 });

				context.SaveChanges();
			}

			using (var context = new IntegraDbContext(options))
			{
				var repositorio = new ArtículoRepository(context);

				//	Act
				var objetos = repositorio.Buscar(o=> o.Nombre.StartsWith("Tejalit"));

				//	Assert
				Assert.Equal(5, objetos.Count());
			}
		}
	}
}
