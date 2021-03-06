using Integra.Shared.Base;
using AutoMapper;
using Integra.DataAccess;
using Integra.DataAccess.Repositories;
using Integra.Shared.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Integra.API.Services
{

	public class ArtículoService : IArtículoService
	{
		private readonly IntegraDbContext _context;
		private readonly ILogger<Artículo> _logger;
		private readonly IArtículoRepository _artículoRepository;
		private readonly IInventarioRepository _inventarioRepository;
		private readonly IRecetaRepository _recetaRepository;
		private readonly IBodegaRepository _bodegaRepository;
		private readonly IUbicaciónRepository _ubicaciónRepository;
		private readonly IAcciónDeInventarioRepository _acciónDeInventarioRepository;

		public ArtículoService(
			IntegraDbContext context,
			ILogger<Artículo> logger,
			IArtículoRepository artículoRepository,
			IInventarioRepository inventarioRepository,
			IRecetaRepository recetaRepository,
			IAcciónDeInventarioRepository acciónDeInventarioRepository
			)
		{
			_context = context;
			_logger = logger;
			_artículoRepository = artículoRepository;
			_recetaRepository = recetaRepository;
			_inventarioRepository = inventarioRepository;
			_acciónDeInventarioRepository = acciónDeInventarioRepository;
		}

		public Artículo Actualizar(Artículo algoParaActualizar)
		{
			var resultado = _artículoRepository.Actualizar(algoParaActualizar);

			if (resultado == null)
				return null;
			_artículoRepository.SaveChanges();

			return resultado;
		}

		public Artículo AdicionarArtículo(Artículo algoParaAdicionar)
		{
			try
			{
				var resultado = _artículoRepository.Adicionar(algoParaAdicionar);

				if (resultado == null)
					return null;

				// Crear el artículo en todas las bodegas y ubicaciones
				var LasBodegas = _bodegaRepository.TraerTodos(b=> b.Sucursal.EmpresaId == algoParaAdicionar.EmpresaId);
				foreach (Bodega bodega in LasBodegas)
				{
					var LasUbicaciones = _ubicaciónRepository.TraerTodos(u=> u.BodegaId == bodega.BodegaId);
					foreach (Ubicación ubicación in LasUbicaciones)
					{
						Inventario UnInventario = new Inventario { BodegaId = bodega.BodegaId, UbicaciónId=ubicación.UbicaciónId, ArtículoId = algoParaAdicionar.ArtículoId, UnidadId = algoParaAdicionar.UnidadId, Cantidad = 0, EstadoId = EstadoEnum.Activo };
						var resultadoInventario = _inventarioRepository.Adicionar(UnInventario);
					}
				}

				_context.SaveChanges();

				return resultado;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				_logger.LogError(ex.StackTrace);
				return null;
			}
		}
		public Ubicación AdicionarUbicación(Ubicación algoParaAdicionar)
		{
			try
			{
				var resultado = _ubicaciónRepository.Adicionar(algoParaAdicionar);

				if (resultado == null)
					return null;

				// Crear todos los artículos en la nueva ubicación

				Expression<Func<Artículo, bool>> elWhereDeArtículos;
				elWhereDeArtículos = a => a.EmpresaId == algoParaAdicionar.Bodega.Sucursal.EmpresaId;

				var LosArtículos = _artículoRepository.TraerTodos(elWhereDeArtículos);
				foreach (Artículo artículo in LosArtículos)
				{
					{
						Inventario UnInventario = new Inventario { BodegaId = algoParaAdicionar.BodegaId, UbicaciónId = algoParaAdicionar.UbicaciónId, ArtículoId = artículo.ArtículoId, UnidadId = artículo.UnidadId, Cantidad = 0, EstadoId = EstadoEnum.Activo };
						var resultadoInventario = _inventarioRepository.Adicionar(UnInventario);
					}
				}

				_context.SaveChanges();

				return resultado;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				_logger.LogError(ex.StackTrace);
				return null;
			}
		}
		public bool Eliminar( Artículo algoParaEliminar)
		{
			try
			{
				_artículoRepository.Eliminar(algoParaEliminar);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return false;
			}

			return true;
		}

		public IEnumerable<Artículo> TraerAyuda(string loquebusco, int cuantospp = 50)
		{
			Expression<Func<Artículo, bool>> elWhere;
			if (string.IsNullOrEmpty(loquebusco))
				elWhere = null;
			else
				elWhere = n => n.Nombre.ToLower().Contains(loquebusco.ToLower()) || n.Código.ToLower().Contains(loquebusco.ToLower());

			var LaRespuesta = _artículoRepository.TraerVariosPTAAsync(elWhere, o => o.Nombre, cuantospp);

			var resultado = LaRespuesta.Result;

			return resultado;
		}


		public PaginatedList<Artículo> TraerPagina( string loquebusco, int pagina, int cuantospp = 10)
		{
			Expression<Func<Artículo, bool>> elWhere;
			if (string.IsNullOrEmpty(loquebusco))
				elWhere = null;
			else
				elWhere = n => n.Nombre.ToLower().Contains(loquebusco.ToLower()) || n.Código.ToLower().Contains(loquebusco.ToLower());

			var LaRespuesta = _artículoRepository.TraerVariosAsync(elWhere
						, o => o.Nombre
						, new List<string> { "ArtículoSubTipo", "ArtículoSubTipo.ArtículoTipo" }
						, pagina, cuantospp);

			var resultado = LaRespuesta.Result;


			return resultado;
		}

		public Artículo TraerUnoPorId(int artículoId)
		{
			var LaRespuesta = _artículoRepository.TraerUnoAsync(a => a.ArtículoId == artículoId, new List<string> { "ArtículoSubTipo", "ArtículoSubTipo.ArtículoTipo" });

			return LaRespuesta.Result;
		}

		public bool Producir(ushort recetaId, decimal CantidadAProducir, ushort BodegaId)
		{
			bool resultado = false;
			Expression<Func<Receta, bool>> elWhereDeReceta;
			elWhereDeReceta = n => n.RecetaId == recetaId;

			var ResultadoReceta = _recetaRepository.TraerUnoAsync(elWhereDeReceta
						, new List<string> { "RecetaDetalles" } //, "Artículo.ArtículoTipo" }
						);
			var LaReceta = ResultadoReceta.Result;

			List<uint> ListaDeArtículos = new List<uint>();

			foreach (RecetaDetalle unDetalle in LaReceta.RecetaDetalles)
			{
				ListaDeArtículos.Add(unDetalle.ArtículoId);
			}

			var Multiplicador = CantidadAProducir / LaReceta.CantidadProducida;



			Expression<Func<Inventario, bool>> elWhereDeInventario;
			elWhereDeInventario = n => n.BodegaId == BodegaId && ListaDeArtículos.Contains(n.ArtículoId);

			var ResultadoInventario = _inventarioRepository.TraerVariosSinTopeAsync(elWhereDeInventario
						, new List<string> {"Artículo", "Artículo.Unidad" }
						, null
						);
			var ElInventario = ResultadoInventario.Result;

			//	Validar la existencia de los componentes de la receta.
			List<string> NoCumplen = new List<string>();

			var query = from artículoRec in LaReceta.RecetaDetalles
						join artículoInv in ElInventario
						on new { artículoRec.ArtículoId }
						equals new { artículoInv.ArtículoId } into XXX
						from UnaCosa in XXX.DefaultIfEmpty()
						select new
						{
							artículoId = artículoRec.ArtículoId,
							artículoRec.Artículo.Nombre,
							UnidadCódigo = artículoRec.Artículo.Unidad.Código,
							CantidadRequerida = artículoRec.Cantidad * Multiplicador,
							CantidadInventario = UnaCosa?.Cantidad ?? 0m
						};


			// Si superamos la validación de existencia quitamos los insumos y aumentamos el resultado
			ushort CuantosNoCumplen = 0;
			foreach (var registro in query)
			{
				if (registro.CantidadInventario < registro.CantidadRequerida)
				{
					NoCumplen.Add($"No hay suficiente {registro.Nombre}. Se requieren {registro.CantidadRequerida} {registro.UnidadCódigo} y solo hay {registro.CantidadInventario}");
					CuantosNoCumplen++;
				}
			}
			if (CuantosNoCumplen > 0)
			{
				throw new ApplicationException(NoCumplen.ToString());
			}

			// Necesitamos disminuir el inventario de insumos



			// Luego aumentar el inventario de producto terminado


			// Finalmente SaveChanges


			return resultado;
		}




		public bool AplicaAcciónDeInventario(AcciónDeInventario acción)
		{
			var transacción = _context.Database.BeginTransaction();

			try
			{
				foreach (AcciónDeInventarioDetalle Tupla in acción.AcciónDeInventarioDetalles)
				{
					var respuesta = _inventarioRepository.TraerUnoAsync(a => a.BodegaId == acción.BodegaId && a.ArtículoId == Tupla.ArtículoId);
					respuesta.Result.Cantidad += Tupla.Cantidad * acción.Signo;
					_inventarioRepository.Actualizar(respuesta.Result);
				}

				_acciónDeInventarioRepository.ActualizaEstado(acción.AcciónDeInventarioId, EstadoAcciónDeInventarioEnum.Aplicada);

				transacción.Commit();
				_context.SaveChanges();

			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				transacción.Rollback();
				return false;
			}

			return true;

		}



	}


}
