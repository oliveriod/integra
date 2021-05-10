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
using Integra.Shared.DTO;
using System.Threading.Tasks;

namespace Integra.API.Services
{

	public class InventarioService : IInventarioService
	{
		private readonly IntegraDbContext _context;
		private readonly ILogger<Artículo> _logger;
		private readonly IArtículoRepository _artículoRepository;
		private readonly IInventarioRepository _inventarioRepository;
		private readonly IAcciónDeInventarioRepository _acciónDeInventarioRepository;

		public InventarioService(
			IntegraDbContext context,
			ILogger<Artículo> logger,
			IArtículoRepository artículoRepository,
			IInventarioRepository inventarioRepository,
			IAcciónDeInventarioRepository acciónDeInventarioRepository
			)
		{
			_context = context;
			_logger = logger;
			_artículoRepository = artículoRepository;
			_inventarioRepository = inventarioRepository;
			_acciónDeInventarioRepository = acciónDeInventarioRepository;
		}

		public Inventario Actualizar(Inventario algoParaActualizar)
		{
			var resultado = _inventarioRepository.Actualizar(algoParaActualizar);

			if (resultado == null)
				return null;
			_inventarioRepository.SaveChanges();

			return resultado;
		}

		public Inventario Adicionar(Inventario algoParaAdicionar)
		{
			try
			{
				var resultado = _inventarioRepository.Adicionar(algoParaAdicionar);

				if (resultado == null)
					return null;
				_inventarioRepository.SaveChanges();

				return resultado;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				_logger.LogError(ex.StackTrace);
				return null;
			}
		}



		public IEnumerable<Inventario> TraerAyuda(string loquebusco, int cuantospp = 50)
		{
			Expression<Func<Inventario, bool>> elWhere;
			if (string.IsNullOrEmpty(loquebusco))
				elWhere = null;
			else
				elWhere = n => n.Artículo.Nombre.ToLower().Contains(loquebusco.ToLower()) || n.Artículo.Código.ToLower().Contains(loquebusco.ToLower());

			var LaRespuesta = _inventarioRepository.TraerVariosPTAAsync(elWhere, o => o.Artículo.Nombre, cuantospp);

			var resultado = LaRespuesta.Result;

			return resultado;
		}


		public Task<PaginatedList<InventarioDto>> TraerPaginaAsync( ushort bodegaId, string loquebusco, int númeroDePágina, int tamañoDePágina = 10)
		{
			Expression<Func<Artículo, bool>> ElWhereDeArtículos;

			if (string.IsNullOrEmpty(loquebusco))
				ElWhereDeArtículos = null;
			else
				ElWhereDeArtículos = n => n.Nombre.ToLower().Contains(loquebusco.ToLower()) || n.Código.ToLower().Contains(loquebusco.ToLower());

			var LaRespuesta = _artículoRepository.TraerTodosAsync(ElWhereDeArtículos
						, o => o.Nombre
						);

			var LosArtículos = LaRespuesta.Result;

			Expression<Func<Inventario, bool>> ElWhereDeInventario;

			ElWhereDeInventario = i => i.BodegaId == bodegaId ;



			var query = from A in _context.Artículos
						join I in _context.Inventarios
						on A.ArtículoId equals I.ArtículoId into T
						from RT in T.DefaultIfEmpty()
						where A.Nombre.ToLower().Contains(loquebusco.ToLower()) || A.Código.ToLower().Contains(loquebusco.ToLower())
						select new 
						{
							BodegaId = bodegaId,
							ArtículoId = A.ArtículoId,
							ArtículoNombre = A.Nombre,
							Cantidad = RT.Cantidad
						};


			return PaginatedList<InventarioDto>.CreateAsync((IQueryable<InventarioDto>)query, númeroDePágina, tamañoDePágina);
		}

		public Inventario TraerUno(uint artículoId, ushort BodegaId)
		{
			var LaRespuesta = _inventarioRepository.TraerUnoAsync(i => i.ArtículoId == artículoId && i.BodegaId == BodegaId, new List<string> { "Artículo", "ArtículoSubTipo", "ArtículoSubTipo.ArtículoTipo" });

			return LaRespuesta.Result;
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
