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

		[Theory]
		[InlineData(1, 4, 4)]
		[InlineData(2, 3, 0)]
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


                var artículoRepository = new ArtículoRepository(context);
                var inventarioRepository = new InventarioRepository(context);
                var recetaRepository = new RecetaRepository(context);


                // Crear artículos
                artículoRepository.Adicionar(new Artículo { ArtículoId = 1, ArtículoSubTipoId = 1, UnidadId = 11, Código = "LEC001", Nombre = "Leche Entera", EstadoId = EstadoEnum.Activo });
				artículoRepository.Adicionar(new Artículo { ArtículoId = 2, ArtículoSubTipoId = 3, UnidadId = 11, Código = "NAT001", Nombre = "Nata líquida", EstadoId = EstadoEnum.Activo } );
				artículoRepository.Adicionar(new Artículo { ArtículoId = 3, ArtículoSubTipoId = 4, UnidadId = 13, Código = "FSE001", Nombre = "Avellanas", EstadoId = EstadoEnum.Activo } );
				artículoRepository.Adicionar(new Artículo { ArtículoId = 4, ArtículoSubTipoId = 4, UnidadId = 13, Código = "FSE002", Nombre = "Mani", EstadoId = EstadoEnum.Activo } );
				artículoRepository.Adicionar(new Artículo { ArtículoId = 5, ArtículoSubTipoId = 4, UnidadId = 13, Código = "FSE003", Nombre = "Nueces", EstadoId = EstadoEnum.Activo } );
				artículoRepository.Adicionar(new Artículo { ArtículoId = 6, ArtículoSubTipoId = 6, UnidadId = 11, Código = "ESP001", Nombre = "Vainilla", EstadoId = EstadoEnum.Activo } );
				artículoRepository.Adicionar(new Artículo { ArtículoId = 7, ArtículoSubTipoId = 6, UnidadId = 11, Código = "VAR001", Nombre = "Azucar", EstadoId = EstadoEnum.Activo } );
				artículoRepository.Adicionar(new Artículo { ArtículoId = 8, ArtículoSubTipoId = 6, UnidadId = 13, Código = "VAR002", Nombre = "canela", EstadoId = EstadoEnum.Activo } );
				artículoRepository.Adicionar(new Artículo { ArtículoId = 9, ArtículoSubTipoId = 6, UnidadId = 13, Código = "VAR003", Nombre = "Chocolate", EstadoId = EstadoEnum.Activo } );
				artículoRepository.Adicionar(new Artículo { ArtículoId = 10, ArtículoSubTipoId = 100, UnidadId = 12, Código = "HEL001", Nombre = "Helado de Vainilla", EstadoId = EstadoEnum.Activo } );
				artículoRepository.Adicionar(new Artículo { ArtículoId = 11, ArtículoSubTipoId = 100, UnidadId = 12, Código = "HEL002", Nombre = "Helado de Chocolate", EstadoId = EstadoEnum.Activo} );

                //Crear las recetas
				recetaRepository.Adicionar(new Receta { RecetaId = 1, Nombre = "Helado de nueces y chocolate", ArtículoId = 101,
                RecetaDetalles = new List<RecetaDetalle>()
                    {
                        new RecetaDetalle { RecetaId = 1, ArtículoId = 3, Cantidad = 100},
                        new RecetaDetalle { RecetaId = 1, ArtículoId = 5,  Cantidad = 200},
                        new RecetaDetalle { RecetaId = 1, ArtículoId = 9, Cantidad = 300}

                    }
                });
				recetaRepository.Adicionar(new Receta { RecetaId = 2, Nombre = "Helado de Avellanas", ArtículoId = 102,
                RecetaDetalles =    new List<RecetaDetalle>()
                    {
                        new RecetaDetalle { RecetaId = 2, ArtículoId = 1, Cantidad = 200},
                        new RecetaDetalle { RecetaId = 2, ArtículoId = 4,  Cantidad = 300},
                        new RecetaDetalle { RecetaId = 2, ArtículoId = 7, Cantidad = 200}
                    }
                 });

                //  Llenar el inventario para las pruebas
				inventarioRepository.Adicionar(new Inventario { BodegaId=1, UbicaciónId=1, ArtículoId = 3, Cantidad=400, UnidadId = 1 });
				inventarioRepository.Adicionar(new Inventario { BodegaId=1, UbicaciónId=1, ArtículoId = 5, Cantidad=800, UnidadId = 1 });
				inventarioRepository.Adicionar(new Inventario { BodegaId=1, UbicaciónId=1, ArtículoId = 9, Cantidad=1200, UnidadId = 1 });

				inventarioRepository.Adicionar(new Inventario { BodegaId=1, UbicaciónId=1, ArtículoId = 1, Cantidad=600, UnidadId = 1 });
				inventarioRepository.Adicionar(new Inventario { BodegaId=1, UbicaciónId=1, ArtículoId = 4, Cantidad=900, UnidadId = 1 });
				inventarioRepository.Adicionar(new Inventario { BodegaId=1, UbicaciónId=1, ArtículoId = 7, Cantidad=400, UnidadId = 1 });



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
				var cuantos = inventarioRepository.TraerUnoAsync

				
			}
		}


		

	}
}

