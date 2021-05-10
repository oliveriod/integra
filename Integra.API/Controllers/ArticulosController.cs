using AutoMapper;
using Integra.API.Services;
using Integra.Shared.Base;
using Integra.Shared.Domain;
using Integra.Shared.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Integra.API
{

	public class ArticulosController : ControllerBase
	{
		private readonly IMapper _mapper;
		private readonly ILogger<Artículo> _logger;
		private readonly IArtículoService _artículoService;

		public ArticulosController(
			IMapper mapper,
			ILogger<Artículo> logger,
			IArtículoService artículoService)
		{
			_mapper = mapper;
			_logger = logger;
			_artículoService = artículoService;
		}

		[HttpPut]
		[ActionName("Actualizar")]
		public IActionResult Actualizar([FromBody] Artículo algoParaActualizar)
		{
			var resultado = _artículoService.Actualizar(algoParaActualizar);

			if (resultado == null)
				return NotFound();

			return Ok(_mapper.Map<ArtículoDto>(resultado));
		}

		[HttpPost]
		[ActionName("Adicionar")]
		public IActionResult Adicionar([FromBody] Artículo algoParaAdicionar)
		{
			try
			{
				var resultado = _artículoService.AdicionarArtículo(algoParaAdicionar);

				if (resultado == null)
					return NotFound();

				return Ok(_mapper.Map<ArtículoDto>(resultado));
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				_logger.LogError(ex.StackTrace);
				return Problem(ex.Message);
			}
		}

		[HttpDelete]
		[ActionName("Eliminar")]
		public IActionResult Eliminar([FromBody] Artículo algoParaEliminar)
		{
			try
			{
				_artículoService.Eliminar(algoParaEliminar);
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
			Expression<Func<Artículo, bool>> elWhere;
			if (string.IsNullOrEmpty(loquebusco))
				elWhere = null;
			else
				elWhere = n => n.Nombre.ToLower().Contains(loquebusco.ToLower()) || n.Código.ToLower().Contains(loquebusco.ToLower());

			var resultado = _artículoService.TraerAyuda(loquebusco, cuantospp);

			if (resultado == null)
				return NotFound();

			return Ok(_mapper.Map<List<ArtículoDto>>(resultado));
		}

		[HttpGet]
		[ActionName("TraerPagina")]
		public IActionResult TraerPagina([FromQuery] string loquebusco, int pagina, int cuantospp = 10)
		{

			var resultado = _artículoService.TraerPagina(loquebusco	, pagina, cuantospp);

			if (resultado == null)
				return NotFound();

			return Ok(_mapper.Map<PaginatedList<ArtículoDto>>(resultado));
		}

		[HttpGet("{artículoId:int}")]
		[ActionName("TraerUnoPorId")]
		public IActionResult TraerUnoPorId(int artículoId)
		{
			var resultado = _artículoService.TraerUnoPorId(artículoId);
			if (resultado == null)
				return NotFound();

			return Ok(_mapper.Map<ArtículoDto>(resultado));
		}


	}


}
