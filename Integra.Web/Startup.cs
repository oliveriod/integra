using BlazorStrap;
using Blazored.Toast;
using Blazored.LocalStorage;
using Integra.Web.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;


namespace Integra.Web
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }



		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddRazorPages();
			services.AddServerSideBlazor();

			services.AddBootstrapCss();
			services.AddBlazoredLocalStorage();
			services.AddBlazoredToast();

			// Todo apunta a que esta es la dirección del API
			var InventarioURI = new Uri("https://localhost:44344/");


			void RegisterTypedClient<TClient, TImplementation>(Uri apiBaseUrl)
				where TClient : class where TImplementation : class, TClient
			{
				services.AddHttpClient<TClient, TImplementation>(client =>
				{
					client.BaseAddress = apiBaseUrl;
				});
			}

			// HTTP services
			RegisterTypedClient<IConsumirAPIService, ConsumirAPIService>(InventarioURI);

			RegisterTypedClient<IArtículoDataService, ArtículoDataService>(InventarioURI);
			RegisterTypedClient<IArtículoSubTipoDataService, ArtículoSubTipoDataService>(InventarioURI);
			RegisterTypedClient<IArtículoTipoDataService, ArtículoTipoDataService>(InventarioURI);
			RegisterTypedClient<IClienteDataService, ClienteDataService>(InventarioURI);
			RegisterTypedClient<ICotizaciónDataService, CotizaciónDataService>(InventarioURI);
			RegisterTypedClient<IProyectoDataService, ProyectoDataService>(InventarioURI);




			// Helper services
			//services.AddTransient<IEmailService, EmailService>();
			//services.AddTransient<IExpenseApprovalService, ManagerApprovalService>();


		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapBlazorHub();
				endpoints.MapFallbackToPage("/_Host");
			});
		}
	}
}
