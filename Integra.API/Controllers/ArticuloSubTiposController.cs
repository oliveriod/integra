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
	public class ArticuloSubTiposController : ControllerBase
	{
		private readonly IMapper _mapper;
		private readonly ILogger<ArtículoTipo> _logger;
		private readonly IArtículoSubTipoRepository _artículoSubTipoRepository;

		public ArticuloSubTiposController(
			IMapper mapper,
			ILogger<ArtículoTipo> logger,
			IArtículoSubTipoRepository artículoSubTipoRepository)
		{
			_mapper = mapper;
			_logger = logger;
			_artículoSubTipoRepository = artículoSubTipoRepository;
		}

		[HttpPut]
		[ActionName("Actualizar")]
		public IActionResult Actualizar([FromBody] ArtículoSubTipo algoParaActualizar)
		{
			var resultado = _artículoSubTipoRepository.Actualizar(algoParaActualizar);

			if (resultado == null)
				return NotFound();
			_artículoSubTipoRepository.SaveChanges();

			return Ok(_mapper.Map<ArtículoTipoDto>(resultado));
		}

		[HttpPost]
		[ActionName("Adicionar")]
		public IActionResult Adicionar([FromBody] ArtículoSubTipo algoParaAdicionar)
		{
			var resultado = _artículoSubTipoRepository.Actualizar(algoParaAdicionar);

			if (resultado == null)
				return NotFound();
			_artículoSubTipoRepository.SaveChanges();

			return Ok(_mapper.Map<ArtículoTipoDto>(resultado));
		}

		[HttpDelete]
		[ActionName("Eliminar")]
		public IActionResult Eliminar([FromBody] ArtículoSubTipo algoParaEliminar)
		{
			try
			{
				_artículoSubTipoRepository.Eliminar(algoParaEliminar);
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
		public IActionResult TraerAyuda([FromQuery] int articuloTipoId, string loquebusco, int cuantospp = 50)
		{
			Expression<Func<ArtículoSubTipo, bool>> elWhere;
			if (string.IsNullOrEmpty(loquebusco))
				elWhere = n => n.ArtículoTipoId == articuloTipoId;
			else
				elWhere = n => n.ArtículoTipoId == articuloTipoId && (n.Nombre.ToLower().Contains(loquebusco.ToLower()) || n.Código.ToLower().Contains(loquebusco.ToLower()));

			var LaRespuesta = _artículoSubTipoRepository.TraerVariosPTAAsync(elWhere, o => o.Nombre, cuantospp);

			var resultado = LaRespuesta.Result;

			if (resultado == null)
				return NotFound();

			return Ok(_mapper.Map<IEnumerable<ArtículoSubTipoDto>>(resultado));
		}

		[HttpGet]
		[ActionName("TraerPagina")]
		public IActionResult TraerPagina([FromQuery] string loquebusco, int pagina, int cuantospp = 50)
		{
			Expression<Func<ArtículoSubTipo, bool>> elWhere;
			if (string.IsNullOrEmpty(loquebusco))
				elWhere = null;
			else
				elWhere = n => n.Nombre.ToLower().Contains(loquebusco.ToLower()) || n.Código.ToLower().Contains(loquebusco.ToLower());


			var LaRespuesta = _artículoSubTipoRepository.TraerVariosAsync(elWhere
						, o => o.Nombre
						, pagina, cuantospp);

			var resultado = LaRespuesta.Result;

			if (resultado == null)
				return NotFound();

			return Ok(_mapper.Map<PaginatedList<ArtículoSubTipoDto>>(resultado));
		}

		[HttpGet("{artículoSubTipoId:ushort}")]
		[ActionName("TraerUnoPorId")]
		public IActionResult TraerUnoPorId(ushort artículoSubTipoId)
		{
			var resultado = _artículoSubTipoRepository.TraerUnoPorId(artículoSubTipoId);

			if (resultado == null)
				return NotFound();

			return Ok(_mapper.Map<ArtículoTipoDto>(resultado));
		}


	}


}
