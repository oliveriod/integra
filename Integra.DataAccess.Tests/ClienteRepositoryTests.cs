using Integra.Shared.Base;
using Integra.Shared.Domain;
using Integra.DataAccess.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Integra.DataAccess.Tests
{
	public class ClienteRepositoryTests
	{
		private readonly ITestOutputHelper _output;

		public ClienteRepositoryTests(ITestOutputHelper output)
		{
			_output = output;
		}


		//[Fact]
		//public void Insertar_NadaRaro_RetornaId33()
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
		//		var repositorio = new ClienteRepository(context);

		//		//	Act
		//		repositorio.Adicionar(new Cliente() { Nombre = "Katy", PrimerApellido= "Perry", EstadoId = EstadoEnum.Activo });
		//		repositorio.Adicionar(new Cliente { ClienteId = 2, Nombre = "María", PrimerApellido = "Shrapova", TipoEnteId = TipoEnteEnum.Persona, EstadoId = EstadoEnum.Activo });
		//		repositorio.Adicionar(new Cliente { ClienteId = 3, Nombre = "Jennifer", PrimerApellido = "Brady", TipoEnteId = TipoEnteEnum.Persona, EstadoId = EstadoEnum.Activo });

		//		repositorio.SaveChanges();

		//		var objeto = repositorio.BuscarPrimero(o=> o.Nombre == "Katy");

		//		//	Assert
		//		Assert.Equal((uint) 2, objeto.ClienteId);
		//	}
		//}



		//[Fact]
		//public void Buscar_TamañoDePáginaEsDiez_RetornaDiezItems()
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

		//		context.Clientes.Add(new Cliente() { Nombre = "Katy", PrimerApellido = "Perry" });
		//		context.Clientes.Add(new Cliente() { Nombre = "Juana", PrimerApellido = "Mayo" });
		//		context.Clientes.Add(new Cliente() { Nombre = "Anacaona", PrimerApellido = "Perry" });
		//		context.Clientes.Add(new Cliente() { Nombre = "Cenicienta", PrimerApellido = "Perry" });
		//		context.Clientes.Add(new Cliente() { Nombre = "Katy", PrimerApellido = "Fernández" });
		//		context.Clientes.Add(new Cliente() { Nombre = "Katy", PrimerApellido = "Pérez" });
		//		context.Clientes.Add(new Cliente() { Nombre = "Katy", PrimerApellido = "González" });

		//		context.SaveChanges();
		//	}

		//	using (var context = new IntegraDbContext(options))
		//	{
		//		var repositorio = new ClienteRepository(context);

		//		//	Act
		//		var objetos = repositorio.TraerVariosAsync();

		//		//	Assert
		//		Assert.Equal(10, objetos.Result.LosRegistros.Count);
		//	}
		//}

		//[Theory]
		//[InlineData("Katy", 4)]
		//[InlineData("Perry", 3)]
		//public void Listar_EmpiezanCon_RetornaItems(string empiezanCon, int cantidad)
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

		//		context.Clientes.Add(new Cliente() { Nombre = "Katy", PrimerApellido = "Perry" });
		//		context.Clientes.Add(new Cliente() { Nombre = "Juana", PrimerApellido = "Mayo" });
		//		context.Clientes.Add(new Cliente() { Nombre = "Anacaona", PrimerApellido = "Perry" });
		//		context.Clientes.Add(new Cliente() { Nombre = "Cenicienta", PrimerApellido = "Perry" });
		//		context.Clientes.Add(new Cliente() { Nombre = "Katy", PrimerApellido = "Fernández" });
		//		context.Clientes.Add(new Cliente() { Nombre = "Katy", PrimerApellido = "Pérez" });
		//		context.Clientes.Add(new Cliente() { Nombre = "Katy", PrimerApellido = "González" });

		//		context.SaveChanges();
		//	}

		//	using (var context = new IntegraDbContext(options))
		//	{
		//		var repositorio = new ClienteRepository(context);

		//		//	Act
		//		var objetos = repositorio.Buscar(o=> o.Nombre.StartsWith(empiezanCon) || o.PrimerApellido.StartsWith(empiezanCon));

		//		//	Assert
		//		Assert.Equal(cantidad, objetos.Count());
		//	}
		//}


	}
}
