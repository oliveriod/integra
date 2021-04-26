using Integra.DataAccess;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog.Web;
using System;

namespace Integra.API
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var logger = NLogBuilder
					.ConfigureNLog("nlog.config")
					.GetCurrentClassLogger();

			try
			{

				logger.Info("Inicializando la aplicación...");

				var host = CreateHostBuilder(args).Build();

				using (var scope = host.Services.CreateScope())
				{
					var context = scope.ServiceProvider.GetService<IntegraDbContext>();
					context.Database.EnsureCreated();
				}

				host.Run();
			}
			catch (Exception ex)
			{
				logger.Error(ex, "Ocurrió una excepción y la aplicación se detuvo.");
				throw;
			}
			finally
			{
				NLog.LogManager.Shutdown();
			}
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>();
					webBuilder.UseNLog();
				});
	}
}
