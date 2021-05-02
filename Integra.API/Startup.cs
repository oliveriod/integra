using Integra.API.Services;
using Integra.DataAccess;
using Integra.DataAccess.Repositories;
using Integra.Shared.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;

namespace Integra.API
{
	public class Startup
	{
		public IConfiguration Configuration { get; }

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			var connectionStringBuilder =
				new SqliteConnectionStringBuilder { DataSource = "Integra_de_oliverio.db" };
			var connection = new SqliteConnection(connectionStringBuilder.ToString());

			var opcionesParaDB = new DbContextOptionsBuilder<IntegraDbContext>()
				.UseSqlite(connection)
				.Options;

			CreateInitialDatabase(opcionesParaDB);


			// DbContext
			services.AddDbContext<IntegraDbContext>(options => options.UseSqlite(connection));


			// Repositorios
			services.AddScoped<IAcciónDeInventarioRepository, AcciónDeInventarioRepository>();
			services.AddScoped<IArtículoRepository, ArtículoRepository>();
			services.AddScoped<IArtículoSubTipoRepository, ArtículoSubTipoRepository>();
			services.AddScoped<IArtículoTipoRepository, ArtículoTipoRepository>();
			services.AddScoped<IInventarioRepository, InventarioRepository>();
			services.AddScoped<IRecetaRepository, RecetaRepository>();
			services.AddScoped<IClienteRepository, ClienteRepository>();
			services.AddScoped<ICotizaciónRepository, CotizaciónRepository>();
			services.AddScoped<IProveedorRepository, ProveedorRepository>();
			services.AddScoped<IProyectoRepository, ProyectoRepository>();
			services.AddScoped<IGenéricoRepository<CotizaciónLínea>, CotizaciónLíneaRepository>();

			// Servicios (BLL)
			services.AddScoped<IArtículoService, ArtículoService>();

			// Automapper (por si acaso)
			services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


			services.AddControllers();
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "Integra.API", Version = "v1" });
			});

			services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseSwagger();
				app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Integra.API v1"));
			}

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}

		public void CreateInitialDatabase(DbContextOptions<IntegraDbContext> options)
		{

			using var context = new IntegraDbContext(options);
			context.Database.EnsureDeleted();
			bool bdLista = context.Database.EnsureCreated();

		}

	}
}
