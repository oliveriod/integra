using Integra.DataAccess;
using Integra.DataAccess.Repositories;
using Integra.Shared.Base;
using Integra.Shared.Domain;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Integra.API.Tests
{
	public class AcciónDeInventarioServiceTests
	{
		private readonly ITestOutputHelper _output;

		public AcciónDeInventarioServiceTests(ITestOutputHelper output)
		{
			_output = output;
		}





		[Fact]
		public void InsertarAcciónDeInventario_NadaRaro_RetornaId_1()
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
			// Incluir artículos
			using (var context = new IntegraDbContext(options))
			{
				var repositorioArtículos = new ArtículoRepository(context);

				repositorioArtículos.Adicionar(new Artículo { ArtículoSubTipoId = 1, UnidadId = 11, Código = "LEC001", Nombre = "Leche Entera", EstadoId = EstadoEnum.Activo });
				repositorioArtículos.Adicionar(new Artículo { ArtículoSubTipoId = 1, UnidadId = 11, Código = "LEC002", Nombre = "Leche Descremada", EstadoId = EstadoEnum.Activo });
				repositorioArtículos.SaveChanges();
			}

			// Incluir inventario
			using (var context = new IntegraDbContext(options))
			{
				var repositorio = new InventarioRepository(context);

				repositorio.Adicionar(new Inventario { BodegaId = 1, ArtículoId = 1, UbicaciónId = 1, UnidadId = 1, Cantidad = 100 });
				repositorio.Adicionar(new Inventario { BodegaId = 1, ArtículoId = 2, UbicaciónId = 1, UnidadId = 1, Cantidad = 100 });
				repositorio.SaveChanges();
			}

			// Incluir acción
			using (var context = new IntegraDbContext(options))
			{
				var repositorio = new AcciónDeInventarioRepository(context);



				AcciónDeInventario acción = new AcciónDeInventario();
				ulong id = 1;

				acción.AcciónDeInventarioId = id;
				acción.ClienteId = 1;
				acción.BodegaId = 1;
				acción.VendedorId = 1;
				acción.Signo = -1;
				acción.Fecha = System.DateTime.Now;

				List<AcciónDeInventarioDetalle> detalles = new List<AcciónDeInventarioDetalle>();
				detalles.Add(new AcciónDeInventarioDetalle { AcciónDeInventarioId = id, NúmeroDeLinea = 1, ArtículoId = 1, Cantidad = 1, PrecioUnitario = 5 });
				detalles.Add(new AcciónDeInventarioDetalle { AcciónDeInventarioId = id, NúmeroDeLinea = 2, ArtículoId = 2, Cantidad = 1, PrecioUnitario = 10 });

				acción.CantidadDeLíneas = (ushort)detalles.Count();
				acción.Total = 15;
				acción.AcciónDeInventarioDetalles = detalles;


				//	Act
				repositorio.Adicionar(acción);
				repositorio.SaveChanges();
				var objeto = repositorio.TraerUnoAsync(o => o.AcciónDeInventarioId == id, new List<string> { "AcciónDeInventarioDetalles" });

				//	Assert
				Assert.Equal(id, objeto.Result.AcciónDeInventarioId);
				Assert.Equal(2, objeto.Result.AcciónDeInventarioDetalles.Count());

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

