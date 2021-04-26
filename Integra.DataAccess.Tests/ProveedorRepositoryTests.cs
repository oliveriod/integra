using Integra.Shared.Domain;
using Integra.DataAccess.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Integra.DataAccess.Tests
{
	public class ProveedorRepositoryTests
	{
		private readonly ITestOutputHelper _output;

		public ProveedorRepositoryTests(ITestOutputHelper output)
		{
			_output = output;
		}


		//[Fact]
		//public void InsertarProveedor_NadaRaro_RetornaId4()
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
		//	}

		//	using (var context = new IntegraDbContext(options))
		//	{
		//		var repositorio = new ProveedorRepository(context);

		//		//	Act
		//		repositorio.Adicionar(new Proveedor() { Nombre = "Mi nuevo proveedor" });
		//		repositorio.SaveChanges();
		//		var objeto = repositorio.BuscarPrimero(o => o.Nombre == "Mi nuevo proveedor");

		//		//	Assert
		//		Assert.Equal((uint) 4, objeto.ProveedorId);
		//	}
		//}

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
