using AutoMapper;
using Integra.DataAccess.Repositories;
using Integra.Shared.Base;
using Integra.Shared.Domain;
using Integra.Shared.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Integra.API.Controllers
{

	[Route("api/[controller]/[action]")]
	[ApiController]
	public class ProyectosController : ControllerBase
	{
		private readonly IMapper _mapper;
		private readonly ILogger<Proyecto> _logger;
		private readonly IProyectoRepository _proyectoRepository;

		public ProyectosController(
			IMapper mapper,
			ILogger<Proyecto> logger,
			IProyectoRepository proyectoRepository)
		{
			_mapper = mapper;
			_logger = logger;
			_proyectoRepository = proyectoRepository;
		}

		[HttpPut]
		[ActionName("Actualizar")]
		public IActionResult Actualizar([FromBody] Proyecto algoParaActualizar)
		{
			var resultado = _proyectoRepository.Actualizar(algoParaActualizar);

			if (resultado == null)
				return NotFound();
			_proyectoRepository.SaveChanges();

			return Ok(_mapper.Map<ProyectoDto>(resultado));
		}

		[HttpPost]
		[ActionName("Adicionar")]
		public IActionResult Adicionar([FromBody] Proyecto algoParaAdicionar)
		{
			var resultado = _proyectoRepository.Adicionar(algoParaAdicionar);

			if (resultado == null)
				return NotFound();
			_proyectoRepository.SaveChanges();

			return Ok(_mapper.Map<ProyectoDto>(resultado));
		}

		[HttpDelete]
		[ActionName("Eliminar")]
		public IActionResult Eliminar([FromBody] Proyecto algoParaEliminar)
		{
			try
			{
				_proyectoRepository.Eliminar(algoParaEliminar);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return NotFound();
			}

			return Ok();
		}

		[HttpGet]
		[ActionName("TraerAyuda")]
		public IActionResult TraerAyuda([FromQuery] string loquebusco, int cuantospp = 50)
		{
			Expression<Func<Proyecto, bool>> elWhere;
			if (string.IsNullOrEmpty(loquebusco))
				elWhere = null;
			else
				elWhere = n => n.Nombre.ToLower().Contains(loquebusco.ToLower())
					|| n.Código.ToLower().Contains(loquebusco.ToLower());

			var LaRespuesta = _proyectoRepository.TraerVariosPTAAsync(elWhere, o => o.Nombre, cuantospp);

			var resultado = LaRespuesta.Result;

			if (resultado == null)
				return NotFound();

			return Ok(_mapper.Map<List<ProyectoDto>>(resultado));
		}

		[HttpGet]
		[ActionName("TraerPagina")]
		public IActionResult TraerPagina([FromQuery] string loquebusco, int pagina, int cuantospp = 10)
		{
			Expression<Func<Proyecto, bool>> elWhere;
			if (string.IsNullOrEmpty(loquebusco))
				elWhere = n => n.EstadoId == EstadoEnum.Activo;
			else
				elWhere = n => n.EstadoId == EstadoEnum.Activo && ( n.Nombre.ToLower().Contains(loquebusco.ToLower())
					|| n.Código.ToLower().Contains(loquebusco.ToLower()));

			var losIncludes = new List<string> { "Cliente" };

			var LaRespuesta = _proyectoRepository.TraerVariosAsync(elWhere, o => o.Nombre, losIncludes, pagina, cuantospp);

			var resultado = LaRespuesta.Result;

			if (resultado == null)
				return NotFound();

			return Ok(_mapper.Map<PaginatedList<ProyectoDto>>(resultado));
		}

		[HttpGet("{proyectoId:int}")]
		[ActionName("TraerUnoPorId")]
		public IActionResult TraerUnoPorId(int proyectoId)
		{
			Expression<Func<Proyecto, bool>> elWhere;
			elWhere = n => n.ProyectoId == proyectoId;

			var LaRespuesta = _proyectoRepository.TraerUnoAsync(elWhere, new List<string> { "Cliente" });

			var resultado = LaRespuesta.Result;

			if (resultado == null)
				return NotFound();

			return Ok(_mapper.Map<ProyectoDto>(resultado));
		}


	}

}
