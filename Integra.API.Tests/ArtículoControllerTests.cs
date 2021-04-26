using Integra.Shared.Base;

using Integra.API.Controllers;
using Integra.DataAccess;
using Integra.DataAccess.Repositories;
using Integra.Shared;
using Integra.Shared.Domain;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Integra.API.Tests
{
	public class ArtículoControllerTests
	{
		[Fact]
		public void InsertarArtículo_RetornaId_1()
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
				Assert.Equal((uint)1, objeto.ArtículoId);
			}
		}


		[Fact]
		public void InsertarArtículo_RetornaId_3()
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

			//ACT
			using (var context = new IntegraDbContext(options))
			{
				var repositorio = new ArtículoRepository(context);

				//	Act
				context.Database.BeginTransaction();
				repositorio.Adicionar(new Artículo { ArtículoSubTipoId = 1, UnidadId = 11, Código = "LEC001", Nombre = "Leche Entera", EstadoId = EstadoEnum.Activo });
				repositorio.Adicionar(new Artículo { ArtículoSubTipoId = 1, UnidadId = 11, Código = "LEC002", Nombre = "Leche semi descremada", EstadoId = EstadoEnum.Activo });
				repositorio.Adicionar(new Artículo { ArtículoSubTipoId = 1, UnidadId = 11, Código = "LEC003", Nombre = "Leche descremada", EstadoId = EstadoEnum.Activo });

				repositorio.SaveChanges();
				context.Database.CommitTransaction();

				var objeto = repositorio.BuscarPrimero(artículo => artículo.Nombre == "Leche descremada");

				//	Assert
				Assert.Equal((uint)3, objeto.ArtículoId);
			}
		}


		[Fact]
		public async Task Should_Return_Falso_Porque_No_Hay_Insumos()
		{
			//// Arrange
			var repositoryMock = new Mock<IArtículoRepository>();
			//var title = new GtlTitle();
			//repositoryMock.Setup(r => r.Get("978-0-10074-5")).Returns(Task.FromResult(title));

			//var controller = new ArticulosController(repositoryMock.Object);

			////	Act
			//var result = await controller.TraerUnoPorId(1);


			////	Assert
			//Assert.Equal(title, result.Value);
		}




		[Fact]
		public async Task Should_Return_Cierto_Porque_Hay_Insumos()
		{
			//// Arrange
			var repositoryMock = new Mock<IArtículoRepository>();
			//var title = new GtlTitle();
			//repositoryMock.Setup(r => r.Get("978-0-10074-5")).Returns(Task.FromResult(title));

			//var controller = new ArticulosController(repositoryMock.Object);

			////	Act
			//var result = await controller.TraerUnoPorId(1);


			////	Assert
			//Assert.Equal(title, result.Value);
		}

	}
}

