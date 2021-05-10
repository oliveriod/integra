using Integra.Shared.Base;
using Integra.Shared.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;



namespace Integra.DataAccess
{
	public class IntegraDbContext : DbContext
	{
		public DbSet<AcciónDeInventario> AccionesDeInventario { get; set; }
		public DbSet<AcciónDeInventarioDetalle> AccionesDeInventarioDetalle { get; set; }

		public DbSet<Artículo> Artículos { get; set; }
		public DbSet<ArtículoTipo> ArtículoTipos { get; set; }
		public DbSet<ArtículoSubTipo> ArtículoSubTipos { get; set; }
		public DbSet<Bodega> Bodegas { get; set; }
		public DbSet<Inventario> Inventarios { get; set; }
		public DbSet<Receta> Recetas { get; set; }
		public DbSet<Unidad> Unidades { get; set; }
		public DbSet<UnidadConversión> UnidadConversiones { get; set; }


		public DbSet<Proveedor> Proveedores { get; set; }
		public DbSet<Cliente> Clientes { get; set; }
		public DbSet<Cotización> Cotizaciones { get; set; }
		public DbSet<Proyecto> Proyectos { get; set; }

		public DbSet<Secuencia> Secuencias { get; set; }
		public DbSet<Vendedor> Vendedores { get; set; }


		//protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		//{
		//	var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = ":memory:" };
		//	var connection = new SqliteConnection(connectionStringBuilder.ToString());

		//	optionsBuilder
		//		.UseSqlite(connection)
		//		;

		//}



		public IntegraDbContext(DbContextOptions<IntegraDbContext> options) : base(options)
		{
		}



		public override int SaveChanges()
		{
			var entries = ChangeTracker
				.Entries()
				.Where(e => e.Entity is DatosBase && (
						e.State == EntityState.Added
						|| e.State == EntityState.Modified));

			foreach (var entityEntry in entries)
			{
				((DatosBase)entityEntry.Entity).FechaDeModificación = DateTime.Now;

				if (entityEntry.State == EntityState.Added)
				{
					((DatosBase)entityEntry.Entity).FechaDeCreación = DateTime.Now;
				}
			}

			return base.SaveChanges();
		}


		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{

			// AcciónDeInventarioDetalle
			modelBuilder.Entity<AcciónDeInventarioDetalle>()
				.HasKey(o => new { o.AcciónDeInventarioId, o.NúmeroDeLinea })
				;

			// Artículo
			modelBuilder.Entity<Artículo>()
				.HasIndex(d => d.Nombre)
				;
			modelBuilder.Entity<Artículo>()
				.HasIndex(d => d.Código)
				.IsUnique()
				;

			// ArtículoProveedor
			modelBuilder.Entity<ArtículoProveedor>()
				.HasKey(o => new { o.ArtículoId, o.ProveedorId })
				;

			// Cliente
			modelBuilder.Entity<Cliente>()
				.HasIndex(i => i.Nombre)
				;

			modelBuilder.Entity<Cliente>()
				.HasIndex(i => i.Identificación)
				.IsUnique()
				;

			//	Cotización
			modelBuilder.Entity<Cotización>()
			.HasIndex(i => i.FechaCotización)
			;
			modelBuilder.Entity<Cotización>()
			.HasIndex(i => i.ClienteId)
			;
			modelBuilder.Entity<Cotización>()
			.HasIndex(i => i.ProyectoId)
			;


			//	CotizaciónLínea
			modelBuilder.Entity<CotizaciónLínea>()
			.HasKey(o => new { o.CotizaciónId, o.ArtículoId })
			;


			// Inventario
			modelBuilder.Entity<Inventario>()
				.HasKey(o => new { o.BodegaId, o.UbicaciónId, o.ArtículoId })
				;

			//	RecetaDetalle
			modelBuilder.Entity<RecetaDetalle>()
			.HasKey(o => new { o.RecetaId, o.ArtículoId })
			;

			//	Secuencia
			modelBuilder.Entity<Secuencia>()
			.HasKey(o => new { o.Columna })
			;

			// Ubicación
			modelBuilder.Entity<Ubicación>()
				.HasKey(o => new { o.BodegaId, o.Código })
				;


			//	UnidadConversión
			modelBuilder.Entity<UnidadConversión>()
			.HasKey(o => new { o.UnidadOrigenId, o.UnidadDestinoId })
			;




			// Seed data

			SeedConfiguraciónData(modelBuilder);
			SeedConfiguraciónArtículosData(modelBuilder);

			//SeedArtículosData(modelBuilder);
			//SeedInventarioData(modelBuilder);
			//SeedRecetasData(modelBuilder);

			//SeedProveedoresData(modelBuilder);
			//SeedClientesData(modelBuilder);

			//SeedData(modelBuilder);

		}

		private static void SeedConfiguraciónData(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Estado>().HasData(
			  new Estado { EstadoId = EstadoEnum.Activo, Nombre = "Activo" },
			  new Estado { EstadoId = EstadoEnum.Inactivo, Nombre = "Inactivo" },
			  new Estado { EstadoId = EstadoEnum.PorDefecto, Nombre = "Por defecto" }
			);
			modelBuilder.Entity<TipoEnte>().HasData(
			  new TipoEnte { TipoEnteId = TipoEnteEnum.PorDefecto, Nombre = "Por defecto" },
			  new TipoEnte { TipoEnteId = TipoEnteEnum.Persona, Nombre = "Persona" },
			  new TipoEnte { TipoEnteId = TipoEnteEnum.Empresa, Nombre = "Empresa" }
			);


			modelBuilder.Entity<Secuencia>().HasData(
				new Secuencia { Columna = "ArtículoId", ValorActual = 0 },
				new Secuencia { Columna = "BodegaId", ValorActual = 2 },
				new Secuencia { Columna = "ClienteId", ValorActual = 0 },
				new Secuencia { Columna = "EmpresaId", ValorActual = 2 },
				new Secuencia { Columna = "RecetaId", ValorActual = 0 },
				new Secuencia { Columna = "SucursalId", ValorActual = 4 },
				new Secuencia { Columna = "UbicaciónId", ValorActual = 1 }
				);



			modelBuilder.Entity<Empresa>().HasData(
			  new Empresa { EmpresaId = 1, Nombre = "MyShake", Código = "MSH", EstadoId = EstadoEnum.PorDefecto },
			  new Empresa { EmpresaId = 2, Nombre = "YourShake", Código = "YSH", EstadoId = EstadoEnum.Activo }
			);

			modelBuilder.Entity<Sucursal>().HasData(
			  new Sucursal { SucursalId = 1, EmpresaId = 1, NúmeroDeSucursal = 1, Nombre = "MyShake #1", Código = "MS01", EstadoId = EstadoEnum.Activo },
			  new Sucursal { SucursalId = 2, EmpresaId = 1, NúmeroDeSucursal = 2, Nombre = "MyShake #2", Código = "MS02", EstadoId = EstadoEnum.Activo },
			  new Sucursal { SucursalId = 3, EmpresaId = 2, NúmeroDeSucursal = 1, Nombre = "YourShake #1", Código = "YS01", EstadoId = EstadoEnum.Activo },
			  new Sucursal { SucursalId = 4, EmpresaId = 2, NúmeroDeSucursal = 2, Nombre = "YourShake #2", Código = "YS02", EstadoId = EstadoEnum.Activo }
			);

			modelBuilder.Entity<Bodega>().HasData(
				new Bodega { BodegaId = 1, SucursalId = 1, Código = "SB", Descripción = "Sin Bodega"		, ParaLaVenta = false },
				new Bodega { BodegaId = 2, SucursalId = 1, Código = "MP", Descripción = "Materia Prima"		, ParaLaVenta = false },
				new Bodega { BodegaId = 3, SucursalId = 1, Código = "PT", Descripción = "Producto Terminado", ParaLaVenta = true }
				);

			modelBuilder.Entity<Ubicación>().HasData(
				new Ubicación { UbicaciónId = 1, BodegaId = 1, Código = "S/U", Descripción = "Sin ubicación", EstadoId = EstadoEnum.Activo },
				new Ubicación { UbicaciónId = 2, BodegaId = 2, Código = "S/U", Descripción = "Sin ubicación", EstadoId = EstadoEnum.Activo },
				new Ubicación { UbicaciónId = 3, BodegaId = 3, Código = "S/U", Descripción = "Sin ubicación", EstadoId = EstadoEnum.Activo }
				);

			modelBuilder.Entity<Unidad>().HasData(
				new Unidad { UnidadId = 1, Código = "uni", Nombre = "Unidad", EstadoId = EstadoEnum.Activo },

				new Unidad { UnidadId = 11, Código = "ml", Nombre = "Mililitro", EstadoId = EstadoEnum.Activo },
				new Unidad { UnidadId = 12, Código = "l", Nombre = "Litro", EstadoId = EstadoEnum.Activo },
				new Unidad { UnidadId = 13, Código = "g", Nombre = "Gramo", EstadoId = EstadoEnum.Activo },
				new Unidad { UnidadId = 14, Código = "Kg", Nombre = "Kilogramo", EstadoId = EstadoEnum.Activo },

				new Unidad { UnidadId = 50, Código = "oz", Nombre = "Onza", EstadoId = EstadoEnum.Activo },
				new Unidad { UnidadId = 51, Código = "gal", Nombre = "Galón", EstadoId = EstadoEnum.Activo }
				);

			modelBuilder.Entity<UnidadConversión>().HasData(
				new UnidadConversión { UnidadOrigenId = 12, UnidadDestinoId = 11, FactorDeConversión = 1000, Nombre = "litro a mililitro" },
				new UnidadConversión { UnidadOrigenId = 14, UnidadDestinoId = 13, FactorDeConversión = 1000, Nombre = "kilogramo a gramo" },
				new UnidadConversión { UnidadOrigenId = 51, UnidadDestinoId = 50, FactorDeConversión = 128, Nombre = "galón a onza" }
				);

			// Valores por defecto
			modelBuilder.Entity<Ubicación>().HasData(new Ubicación { UbicaciónId = 1, Descripción = "Sin Ubicación", Código = "SU000000" });
			modelBuilder.Entity<Vendedor>().HasData(new Vendedor { VendedorId = 1, Nombre = "Sin Vendedor", TipoEnteId = TipoEnteEnum.PorDefecto, EstadoId = EstadoEnum.Activo });
			modelBuilder.Entity<Cliente>().HasData(new Cliente { ClienteId = 1, Nombre = "Contado", TipoEnteId = TipoEnteEnum.PorDefecto, EstadoId = EstadoEnum.Activo });

		}


		private static void SeedConfiguraciónArtículosData(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<ArtículoTipo>().HasData(
				new ArtículoTipo { ArtículoTipoId = 1, Nombre = "Lácteos", Código = "LAC", EstadoId = EstadoEnum.Activo },
				new ArtículoTipo { ArtículoTipoId = 2, Nombre = "Frutos Secos", Código = "FSE", EstadoId = EstadoEnum.Activo },
				new ArtículoTipo { ArtículoTipoId = 3, Nombre = "Sabores", Código = "SAB", EstadoId = EstadoEnum.Activo },
				new ArtículoTipo { ArtículoTipoId = 4, Nombre = "Varios", Código = "VAR", EstadoId = EstadoEnum.Activo },
				new ArtículoTipo { ArtículoTipoId = 100, Nombre = "Helados", Código = "HEL", EstadoId = EstadoEnum.Activo }
			);

			modelBuilder.Entity<ArtículoSubTipo>().HasData(
				new ArtículoSubTipo { ArtículoSubTipoId = 1, Nombre = "Leche", Código = "LEC", ArtículoTipoId = 1, EstadoId = EstadoEnum.Activo },
				new ArtículoSubTipo { ArtículoSubTipoId = 2, Nombre = "Leche condensada", Código = "LCO", ArtículoTipoId = 1, EstadoId = EstadoEnum.Activo },
				new ArtículoSubTipo { ArtículoSubTipoId = 3, Nombre = "Nata", Código = "NAT", ArtículoTipoId = 1, EstadoId = EstadoEnum.Activo },
				new ArtículoSubTipo { ArtículoSubTipoId = 4, Nombre = "Frutos Secos", Código = "FSE", ArtículoTipoId = 2, EstadoId = EstadoEnum.Activo },
				new ArtículoSubTipo { ArtículoSubTipoId = 6, Nombre = "Licores", Código = "LIC", ArtículoTipoId = 3, EstadoId = EstadoEnum.Activo },
				new ArtículoSubTipo { ArtículoSubTipoId = 7, Nombre = "Especies", Código = "ESP", ArtículoTipoId = 3, EstadoId = EstadoEnum.Activo },
				new ArtículoSubTipo { ArtículoSubTipoId = 8, Nombre = "Varios", Código = "VAR", ArtículoTipoId = 4, EstadoId = EstadoEnum.Activo },
				new ArtículoSubTipo { ArtículoSubTipoId = 100, Nombre = "Helados", Código = "HEL", ArtículoTipoId = 100, EstadoId = EstadoEnum.Activo }
			  );

		}

		private static void SeedArtículosData(ModelBuilder modelBuilder)
		{

			modelBuilder.Entity<Artículo>().HasData(
				new Artículo { ArtículoId = 1, ArtículoSubTipoId = 1, UnidadId = 11, Código = "LEC001", Nombre = "Leche Entera", EstadoId = EstadoEnum.Activo },
				new Artículo { ArtículoId = 2, ArtículoSubTipoId = 2, UnidadId = 11, Código = "LCO001", Nombre = "Leche condensada", EstadoId = EstadoEnum.Activo },
				new Artículo { ArtículoId = 3, ArtículoSubTipoId = 3, UnidadId = 11, Código = "NAT001", Nombre = "Nata líquida", EstadoId = EstadoEnum.Activo },
				new Artículo { ArtículoId = 4, ArtículoSubTipoId = 4, UnidadId = 13, Código = "FSE001", Nombre = "Avellanas", EstadoId = EstadoEnum.Activo },
				new Artículo { ArtículoId = 5, ArtículoSubTipoId = 4, UnidadId = 13, Código = "FSE002", Nombre = "Mani", EstadoId = EstadoEnum.Activo },
				new Artículo { ArtículoId = 6, ArtículoSubTipoId = 4, UnidadId = 13, Código = "FSE003", Nombre = "Nueces", EstadoId = EstadoEnum.Activo },
				new Artículo { ArtículoId = 7, ArtículoSubTipoId = 5, UnidadId = 11, Código = "LIC001", Nombre = "Licor de menta", EstadoId = EstadoEnum.Activo },
				new Artículo { ArtículoId = 8, ArtículoSubTipoId = 5, UnidadId = 11, Código = "LIC002", Nombre = "Licor de café", EstadoId = EstadoEnum.Activo },
				new Artículo { ArtículoId = 9, ArtículoSubTipoId = 6, UnidadId = 11, Código = "ESP001", Nombre = "Vainilla", EstadoId = EstadoEnum.Activo },
				new Artículo { ArtículoId = 10, ArtículoSubTipoId = 6, UnidadId = 11, Código = "ESP002", Nombre = "Canela", EstadoId = EstadoEnum.Activo },
				new Artículo { ArtículoId = 11, ArtículoSubTipoId = 6, UnidadId = 11, Código = "VAR001", Nombre = "Azucar", EstadoId = EstadoEnum.Activo },
				new Artículo { ArtículoId = 12, ArtículoSubTipoId = 6, UnidadId = 11, Código = "VAR002", Nombre = "Miel", EstadoId = EstadoEnum.Activo },
				new Artículo { ArtículoId = 13, ArtículoSubTipoId = 6, UnidadId = 13, Código = "VAR003", Nombre = "Chocolate", EstadoId = EstadoEnum.Activo },
				new Artículo { ArtículoId = 101, ArtículoSubTipoId = 100, UnidadId = 12, Código = "HEL001", Nombre = "Helado de Vainilla", EstadoId = EstadoEnum.Activo },
				new Artículo { ArtículoId = 102, ArtículoSubTipoId = 100, UnidadId = 12, Código = "HEL002", Nombre = "Helado de Chocolate", EstadoId = EstadoEnum.Activo }
			);
		}

		private static void SeedInventarioData(ModelBuilder modelBuilder)
		{


			modelBuilder.Entity<Inventario>().HasData(
				new Inventario { BodegaId = 1, UbicaciónId = 1, ArtículoId = 11, UnidadId = 13, Cantidad = 10000, EstadoId = EstadoEnum.Activo },
				new Inventario { BodegaId = 1, UbicaciónId = 1, ArtículoId = 3, UnidadId = 11, Cantidad = 10000, EstadoId = EstadoEnum.Activo },
				new Inventario { BodegaId = 1, UbicaciónId = 1, ArtículoId = 13, UnidadId = 13, Cantidad = 10000, EstadoId = EstadoEnum.Activo },
				new Inventario { BodegaId = 1, UbicaciónId = 1, ArtículoId = 6, UnidadId = 13, Cantidad = 10000, EstadoId = EstadoEnum.Activo }
				);
		}




		private static void SeedRecetasData(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Receta>().HasData(
				new Receta { RecetaId = 1, Nombre = "Helado de nueces y chocolate", ArtículoId = 101, },
				new Receta { RecetaId = 2, Nombre = "Helado de Avellanas", ArtículoId = 102 }
				);

			modelBuilder.Entity<RecetaDetalle>().HasData(
				new RecetaDetalle { RecetaId = 1, ArtículoId = 11, Cantidad = 110 },
				new RecetaDetalle { RecetaId = 1, ArtículoId = 3, Cantidad = 500 },
				new RecetaDetalle { RecetaId = 1, ArtículoId = 13, Cantidad = 100 },
				new RecetaDetalle { RecetaId = 1, ArtículoId = 6, Cantidad = 70 }
				);
		}



		private static void SeedClientesData(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Cliente>().HasData(
			  new Cliente { ClienteId = 1, Nombre = "Oliverio", PrimerApellido = "Diaz", TipoEnteId = TipoEnteEnum.Persona, EstadoId = EstadoEnum.Activo },
			  new Cliente { ClienteId = 2, Nombre = "María", PrimerApellido = "Shrapova", TipoEnteId = TipoEnteEnum.Persona, EstadoId = EstadoEnum.Activo },
			  new Cliente { ClienteId = 3, Nombre = "Jennifer", PrimerApellido = "Brady", TipoEnteId = TipoEnteEnum.Persona, EstadoId = EstadoEnum.Activo },
			  new Cliente { ClienteId = 4, Nombre = "Naomi", PrimerApellido = "Osaka", TipoEnteId = TipoEnteEnum.Persona, EstadoId = EstadoEnum.Activo },
			  new Cliente { ClienteId = 5, Nombre = "Claudia Carolina", PrimerApellido = "Morales", TipoEnteId = TipoEnteEnum.Persona, EstadoId = EstadoEnum.Activo },
			  new Cliente { ClienteId = 6, Nombre = "Adam", PrimerApellido = "West", TipoEnteId = TipoEnteEnum.Persona, EstadoId = EstadoEnum.Activo },
			  new Cliente { ClienteId = 7, Nombre = "Leonard", PrimerApellido = "Hofstadter", TipoEnteId = TipoEnteEnum.Persona, EstadoId = EstadoEnum.Activo },
			  new Cliente { ClienteId = 8, Nombre = "Sheldon", PrimerApellido = "Cooper", TipoEnteId = TipoEnteEnum.Persona, EstadoId = EstadoEnum.Activo },
			  new Cliente { ClienteId = 9, Nombre = "Howard", PrimerApellido = "Wolowitz", TipoEnteId = TipoEnteEnum.Persona, EstadoId = EstadoEnum.Activo },
			  new Cliente { ClienteId = 10, Nombre = "Raj", PrimerApellido = "Koothrappali", TipoEnteId = TipoEnteEnum.Persona, EstadoId = EstadoEnum.Activo },
			  new Cliente { ClienteId = 11, Nombre = "Bernadette", PrimerApellido = "Rostenkowski", TipoEnteId = TipoEnteEnum.Persona, EstadoId = EstadoEnum.Activo },
			  new Cliente { ClienteId = 12, Nombre = "Amy", PrimerApellido = "Farrah Fowler", TipoEnteId = TipoEnteEnum.Persona, EstadoId = EstadoEnum.Activo },
			  new Cliente { ClienteId = 13, Nombre = "Stuart", PrimerApellido = "Bloom", TipoEnteId = TipoEnteEnum.Persona, EstadoId = EstadoEnum.Activo },
			  new Cliente { ClienteId = 14, Nombre = "Barry", PrimerApellido = "Kripke", TipoEnteId = TipoEnteEnum.Persona, EstadoId = EstadoEnum.Activo },
			  new Cliente { ClienteId = 15, Nombre = "Emily", PrimerApellido = "Sweeney", TipoEnteId = TipoEnteEnum.Persona, EstadoId = EstadoEnum.Activo },
			  new Cliente { ClienteId = 16, Nombre = "Wil", PrimerApellido = "Wheaton", TipoEnteId = TipoEnteEnum.Persona, EstadoId = EstadoEnum.Activo },
			  new Cliente { ClienteId = 17, Nombre = "Mary", PrimerApellido = "Cooper", TipoEnteId = TipoEnteEnum.Persona, EstadoId = EstadoEnum.Activo },
			  new Cliente { ClienteId = 18, Nombre = "Priya", PrimerApellido = "Koothrappali", TipoEnteId = TipoEnteEnum.Persona, EstadoId = EstadoEnum.Activo },
			  new Cliente { ClienteId = 19, Nombre = "Leslie", PrimerApellido = "Winkle", TipoEnteId = TipoEnteEnum.Persona, EstadoId = EstadoEnum.Activo },
			  new Cliente { ClienteId = 20, Nombre = "Stephen", PrimerApellido = "Hawking", TipoEnteId = TipoEnteEnum.Persona, EstadoId = EstadoEnum.Activo },
			  new Cliente { ClienteId = 21, Nombre = "Mike", PrimerApellido = "Massimino", TipoEnteId = TipoEnteEnum.Persona, EstadoId = EstadoEnum.Activo },
			  new Cliente { ClienteId = 22, Nombre = "Ramona", PrimerApellido = "Nowitzki", TipoEnteId = TipoEnteEnum.Persona, EstadoId = EstadoEnum.Activo },
			  new Cliente { ClienteId = 23, Nombre = "Neil", PrimerApellido = "deGrasse Tyson", TipoEnteId = TipoEnteEnum.Persona, EstadoId = EstadoEnum.Activo },
			  new Cliente { ClienteId = 24, Nombre = "Alfred", PrimerApellido = "Hofstadter", TipoEnteId = TipoEnteEnum.Persona, EstadoId = EstadoEnum.Activo },
			  new Cliente { ClienteId = 25, Nombre = "Steve", PrimerApellido = "Wozniak", TipoEnteId = TipoEnteEnum.Persona, EstadoId = EstadoEnum.Activo },
			  new Cliente { ClienteId = 26, Nombre = "Stan", PrimerApellido = "Lee", TipoEnteId = TipoEnteEnum.Persona, EstadoId = EstadoEnum.Activo },
			  new Cliente { ClienteId = 27, Nombre = "James Earl", PrimerApellido = "Jones", TipoEnteId = TipoEnteEnum.Persona, EstadoId = EstadoEnum.Activo },
			  new Cliente { ClienteId = 28, Nombre = "Sarah Michelle", PrimerApellido = "Gellar", TipoEnteId = TipoEnteEnum.Persona, EstadoId = EstadoEnum.Activo },
			  new Cliente { ClienteId = 29, Nombre = "OMA, S.A.", TipoEnteId = TipoEnteEnum.Empresa, EstadoId = EstadoEnum.Activo },
			  new Cliente { ClienteId = 30, Nombre = "CODEPTY", TipoEnteId = TipoEnteEnum.Empresa, EstadoId = EstadoEnum.Activo },
			  new Cliente { ClienteId = 31, Nombre = "Pre Party, S.A.", TipoEnteId = TipoEnteEnum.Empresa, EstadoId = EstadoEnum.Activo },
			  new Cliente { ClienteId = 32, Nombre = "Diacor, S.A.", TipoEnteId = TipoEnteEnum.Empresa, EstadoId = EstadoEnum.Activo }
			);
		}

		private static void SeedProveedoresData(ModelBuilder modelBuilder)
		{

			modelBuilder.Entity<Proveedor>().HasData(
			  new Proveedor { ProveedorId = 1, Nombre = "Cochez", TipoEnteId = TipoEnteEnum.Empresa },
			  new Proveedor { ProveedorId = 2, Nombre = "Electrisa", TipoEnteId = TipoEnteEnum.Empresa },
			  new Proveedor { ProveedorId = 3, Nombre = "Panafoto", TipoEnteId = TipoEnteEnum.Empresa }
			);

			modelBuilder.Entity<ArtículoProveedor>().HasData(
				new ArtículoProveedor { ArtículoId = 1, ProveedorId = 1 },
				new ArtículoProveedor { ArtículoId = 2, ProveedorId = 1 },
				new ArtículoProveedor { ArtículoId = 3, ProveedorId = 1 },
				new ArtículoProveedor { ArtículoId = 4, ProveedorId = 1 },
				new ArtículoProveedor { ArtículoId = 4, ProveedorId = 2 },
				new ArtículoProveedor { ArtículoId = 5, ProveedorId = 2 }
				);
		}

		private static void SeedData(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Vendedor>().HasData(
			  new Vendedor { VendedorId = 1, Nombre = "Richard", PrimerApellido = "Díaz", EstadoId = EstadoEnum.Activo },
			  new Vendedor { VendedorId = 2, Nombre = "Katilu", PrimerApellido = "Díaz", EstadoId = EstadoEnum.Activo },
			  new Vendedor { VendedorId = 3, Nombre = "Azul", PrimerApellido = "Díaz", EstadoId = EstadoEnum.Activo },
			  new Vendedor { VendedorId = 4, Nombre = "Yoko Alberto", PrimerApellido = "Díaz", EstadoId = EstadoEnum.Activo },
			  new Vendedor { VendedorId = 5, Nombre = "Wiso del Carmen", PrimerApellido = "Díaz", EstadoId = EstadoEnum.Activo }
			);

			modelBuilder.Entity<Proyecto>().HasData(
			  new Proyecto { ProyectoId = -1, Código = "", ClienteId = -1, EstadoId = EstadoEnum.PorDefecto },
			  new Proyecto { ProyectoId = 1, Código = "PMAN", Nombre = "Proyecto Manhattan", ClienteId = 1, EstadoId = EstadoEnum.Activo },
			  new Proyecto { ProyectoId = 2, Código = "PTER", Nombre = "Terminator", ClienteId = 2, EstadoId = EstadoEnum.Activo },
			  new Proyecto { ProyectoId = 3, Código = "PTRE", Nombre = "Tres", ClienteId = 3, EstadoId = EstadoEnum.Activo },
			  new Proyecto { ProyectoId = 4, Código = "PCUA", Nombre = "Cuatro", ClienteId = 4, EstadoId = EstadoEnum.Activo },
			  new Proyecto { ProyectoId = 5, Código = "PCIN", Nombre = "Cinco", ClienteId = 5, EstadoId = EstadoEnum.Activo },
			  new Proyecto { ProyectoId = 6, Código = "PSEI", Nombre = "Seis", ClienteId = 6, EstadoId = EstadoEnum.Activo },
			  new Proyecto { ProyectoId = 7, Código = "PGBL", Nombre = "Gorros blancos", ClienteId = 1, EstadoId = EstadoEnum.Activo }
			);




		}



	}
}
