using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using NLog.Web;
using System;

namespace Integra.Web
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
				CreateHostBuilder(args).Build().Run();
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
					webBuilder.UseStaticWebAssets();	// ODI20210328 Esto parece que es necesario para Blazored
					webBuilder.UseStartup<Startup>();
					webBuilder.UseNLog();
				});
	}
}
