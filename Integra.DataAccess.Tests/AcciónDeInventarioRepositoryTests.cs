#define TEST

using Integra.DataAccess.Repositories;
using Integra.Shared.Base;
using Integra.Shared.Domain;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Integra.DataAccess.Tests
{
	public class AcciónDeInventarioRepositoryTests
	{
		private readonly ITestOutputHelper _output;

		public AcciónDeInventarioRepositoryTests(ITestOutputHelper output)
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

			using (var context = new IntegraDbContext(options))
			{
				var repositorio = new AcciónDeInventarioRepository(context);

				var repositorioArtículos = new ArtículoRepository(context);

				repositorioArtículos.Adicionar(new Artículo { ArtículoSubTipoId = 1, UnidadId = 11, Código = "LEC001", Nombre = "Leche Entera", EstadoId = EstadoEnum.Activo });
				repositorioArtículos.Adicionar(new Artículo { ArtículoSubTipoId = 1, UnidadId = 11, Código = "LEC002", Nombre = "Leche Descremada", EstadoId = EstadoEnum.Activo });
				repositorioArtículos.SaveChanges();



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

		//[Fact]
		//public void ObtenerProveedores_TamañoDePáginaEsDos_RetornaDosProveedores()
		//{
		//	// Arrange

		//	var connectionStringBuilder =
		//		new SqliteConnectionStringBuilder { DataSource = ":memory:" };
		//	var connection = new SqliteConnection(connectionStringBuilder.ToString());

		//	var options = new DbContextOptionsBuilder<IntegraDbContext>()
		//		.UseSqlite(connection)
		//		.Options;

		//	using (var context = new IntegraDbContext(options))
		//	{
		//		context.Database.OpenConnection();
		//		context.Database.EnsureCreated();

		//		context.Proveedores.Add(new Proveedor() { Nombre = "Do It" });
		//		context.Proveedores.Add(new Proveedor() { Nombre = "Novey" });
		//		context.Proveedores.Add(new Proveedor() { Nombre = "Implosa" });
		//		context.Proveedores.Add(new Proveedor() { Nombre = "Discovery" });

		//		context.SaveChanges();
		//	}

		//	using (var context = new IntegraDbContext(options))
		//	{
		//		var repositorio = new ProveedorRepository(context);

		//		//	Act
		//		var objetos = repositorio.TraerVariosAsync(2, 2);

		//		//	Assert
		//		Assert.Equal(2, objetos.Result.LosRegistros.Count);
		//	}
		//}

		//[Fact]
		//public void ObtenerProveedores_EmpiezanConD_RetornaDosProveedores()
		//{
		//	// Arrange
		//	var connectionStringBuilder =
		//		new SqliteConnectionStringBuilder { DataSource = ":memory:" };
		//	var connection = new SqliteConnection(connectionStringBuilder.ToString());

		//	var options = new DbContextOptionsBuilder<IntegraDbContext>()
		//		.UseSqlite(connection)
		//		.Options;

		//	using (var context = new IntegraDbContext(options))
		//	{
		//		context.Database.OpenConnection();
		//		context.Database.EnsureCreated();

		//		context.Proveedores.Add(new Proveedor() { Nombre = "Do It" });
		//		context.Proveedores.Add(new Proveedor() { Nombre = "Novey" });
		//		context.Proveedores.Add(new Proveedor() { Nombre = "Implosa" });
		//		context.Proveedores.Add(new Proveedor() { Nombre = "Discovery" });

		//		context.SaveChanges();
		//	}

		//	using (var context = new IntegraDbContext(options))
		//	{
		//		var repositorio = new ProveedorRepository(context);

		//		//	Act
		//		var objetos = repositorio.Buscar(o => o.Nombre.StartsWith("D"));

		//		//	Assert
		//		Assert.Equal(2, objetos.Count());
		//	}
		//}


	}
}
