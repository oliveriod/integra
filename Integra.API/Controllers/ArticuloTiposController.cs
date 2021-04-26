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
	public class ArticuloTiposController : ControllerBase
	{
		private readonly IMapper _mapper;
		private readonly ILogger<ArtículoTipo> _logger;
		private readonly IArtículoTipoRepository _artículoTipoRepository;

		public ArticuloTiposController(
			IMapper mapper,
			ILogger<ArtículoTipo> logger,
			IArtículoTipoRepository artículoTipoRepository)
		{
			_mapper = mapper;
			_logger = logger;
			_artículoTipoRepository = artículoTipoRepository;
		}

		[HttpPut]
		[ActionName("Actualizar")]
		public IActionResult Actualizar([FromBody] ArtículoTipo algoParaActualizar)
		{
			var resultado = _artículoTipoRepository.Actualizar(algoParaActualizar);

			if (resultado == null)
				return NotFound();
			_artículoTipoRepository.SaveChanges();

			return Ok(_mapper.Map<ArtículoTipoDto>(resultado));
		}

		[HttpPost]
		[ActionName("Adicionar")]
		public IActionResult Adicionar([FromBody] ArtículoTipo algoParaAdicionar)
		{
			var resultado = _artículoTipoRepository.Actualizar(algoParaAdicionar);

			if (resultado == null)
				return NotFound();
			_artículoTipoRepository.SaveChanges();

			return Ok(_mapper.Map<ArtículoTipoDto>(resultado));
		}

		[HttpDelete]
		[ActionName("Eliminar")]
		public IActionResult Eliminar([FromBody] ArtículoTipo algoParaBorrar)
		{
			try
			{
				_artículoTipoRepository.Eliminar(algoParaBorrar);
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
			Expression<Func<ArtículoTipo, bool>> elWhere;

			if (string.IsNullOrEmpty(loquebusco))
				elWhere = null;
			else
				elWhere = n => n.Nombre.ToLower().Contains(loquebusco.ToLower()) || n.Código.ToLower().Contains(loquebusco.ToLower());

			var resultado = _artículoTipoRepository.TraerVariosAsync(elWhere, o => o.Nombre, cuantospp);

			if (resultado == null)
				return NotFound();

			return Ok(_mapper.Map<IEnumerable<ArtículoTipoDto>>(resultado));
		}

		[HttpGet]
		[ActionName("TraerPagina")]
		public IActionResult TraerPagina([FromQuery] string loquebusco, int pagina, int cuantospp = 10)
		{
			Expression<Func<ArtículoTipo, bool>> elWhere;

			if (string.IsNullOrEmpty(loquebusco))
				elWhere = null;
			else
				elWhere = n => n.Nombre.ToLower().Contains(loquebusco.ToLower()) || n.Código.ToLower().Contains(loquebusco.ToLower());

			var LaRespuesta = _artículoTipoRepository.TraerVariosAsync(elWhere
				, o => o.Nombre
				, pagina, cuantospp);

			var resultado = LaRespuesta.Result;

			if (resultado == null)
				return NotFound();

			return Ok(_mapper.Map<PaginatedList<ArtículoTipoDto>>(resultado));
		}

		[HttpGet]
		[ActionName("TraerTodos")]
		public IActionResult TraerTodos()
		{
			var LaRespuesta = _artículoTipoRepository.TraerTodosAsync( o => o.Nombre);

			var resultado = LaRespuesta.Result;

			if (resultado == null)
				return NotFound();

			return Ok(_mapper.Map<List<ArtículoTipoDto>>(resultado));
		}

		[HttpGet("{artículoTipoId:ushort}")]
		[ActionName("TraerUnoPorId")]
		public IActionResult TraerUnoPorId(ushort artículoTipoId)
		{
			var resultado = _artículoTipoRepository.TraerUnoPorId(artículoTipoId);

			if (resultado == null)
				return NotFound();

			return Ok(_mapper.Map<ArtículoTipoDto>(resultado));
		}

	}


}
