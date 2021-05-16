using Integra.API.Services;
using Integra.DataAccess;
using Integra.DataAccess.Repositories;
using Integra.Shared.Base;
using Integra.Shared.Domain;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Integra.API.Tests
{
	public class ArtículoServiceTests
	{
		private readonly ITestOutputHelper _output;

		public ArtículoServiceTests(ITestOutputHelper output)
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

		// Hacer una prueba del servicio que crea artículos (revisar que los meta en la bodega.ubicación)



		// Hacer una prueba del servicio que crea ubicaciones (revisar que meta los artículos en la bodega.ubicación)

		[Theory]
		[InlineData(1, 4, 4)]
//		[InlineData(1, 7, 0)]
		public void ProcesarRecetas(ushort Receta, int CantidadAProducir, int CantidadProducida)
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

				context.SaveChanges();
			}

			using (var context = new IntegraDbContext(options))
			{
                var artículoRepository = new ArtículoRepository(context);

                // Crear artículos
                artículoRepository.Adicionar(new Artículo { ArtículoSubTipoId = 1, UnidadId = 11, Código = "LEC001", Nombre = "Leche Entera", EstadoId = EstadoEnum.Activo });
				artículoRepository.Adicionar(new Artículo { ArtículoSubTipoId = 3, UnidadId = 11, Código = "NAT001", Nombre = "Nata líquida", EstadoId = EstadoEnum.Activo } );
				artículoRepository.Adicionar(new Artículo { ArtículoSubTipoId = 4, UnidadId = 13, Código = "FSE001", Nombre = "Avellanas", EstadoId = EstadoEnum.Activo } );
				artículoRepository.Adicionar(new Artículo { ArtículoSubTipoId = 4, UnidadId = 13, Código = "FSE003", Nombre = "Nueces", EstadoId = EstadoEnum.Activo } );
				artículoRepository.Adicionar(new Artículo { ArtículoSubTipoId = 6, UnidadId = 13, Código = "VAR003", Nombre = "Chocolate", EstadoId = EstadoEnum.Activo } );
				artículoRepository.Adicionar(new Artículo { ArtículoSubTipoId = 100, UnidadId = 12, Código = "HEL001", Nombre = "Helado de Vainilla", EstadoId = EstadoEnum.Activo } );

				context.SaveChanges();
			}


			using (var context = new IntegraDbContext(options))
			{
				var recetaRepository = new RecetaRepository(context);

				//Crear las recetas
				recetaRepository.Adicionar(new Receta
				{
					RecetaId = 1,
					Nombre = "Helado de nueces y chocolate",
					ArtículoId = 6,
					CantidadProducida = 1,
					RecetaDetalles = new List<RecetaDetalle>()
					{
						new RecetaDetalle { RecetaDetalleId=1, RecetaId = 1, ArtículoId = 1, Cantidad = 100},
						new RecetaDetalle { RecetaDetalleId=2, RecetaId = 1, ArtículoId = 4, Cantidad = 200},
						new RecetaDetalle { RecetaDetalleId=3, RecetaId = 1, ArtículoId = 5, Cantidad = 300}

					}
				});
				context.SaveChanges();
			}


			using (var context = new IntegraDbContext(options))
			{
				var inventarioRepository = new InventarioRepository(context);

				//  Llenar el inventario para las pruebas
				inventarioRepository.Adicionar(new Inventario { BodegaId = 1, UbicaciónId = 1, ArtículoId = 1, Cantidad = 400, UnidadId = 1 });
				inventarioRepository.Adicionar(new Inventario { BodegaId = 1, UbicaciónId = 1, ArtículoId = 4, Cantidad = 800, UnidadId = 1 });
				inventarioRepository.Adicionar(new Inventario { BodegaId = 1, UbicaciónId = 1, ArtículoId = 5, Cantidad = 1200, UnidadId = 1 });

				context.SaveChanges();
			}


			using (var context = new IntegraDbContext(options))
			{
                var loggerMock = new Mock<ILogger<Artículo>>();
                var artículoRepository = new ArtículoRepository(context);
                var inventarioRepository = new InventarioRepository(context);
                var recetaRepository = new RecetaRepository(context);
                var acciónDeInventarioRepository = new AcciónDeInventarioRepository(context);


				//	Act
                var servicio = new ArtículoService(context, loggerMock.Object, artículoRepository, inventarioRepository, recetaRepository, acciónDeInventarioRepository);

				servicio.Producir(Receta, CantidadAProducir, 1);

				//	Assert
				var inventarioTerminado = inventarioRepository.TraerUnoAsync(i => i.ArtículoId == 6 && i.BodegaId == 1 && i.UbicaciónId == 1);
				var resultado = inventarioTerminado.Result;

				Assert.Equal(CantidadProducida, resultado.Cantidad);

			}
		}


		

	}
}

