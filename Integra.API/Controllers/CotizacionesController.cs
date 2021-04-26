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
	public class CotizacionesController : ControllerBase
	{
		private readonly IMapper _mapper;
		private readonly ILogger<Cotización> _logger;
		private readonly ICotizaciónRepository _cotizaciónRepository;

		public CotizacionesController(
			IMapper mapper,
			ILogger<Cotización> logger,
			ICotizaciónRepository cotizaciónRepository)
		{
			_mapper = mapper;
			_logger = logger;
			_cotizaciónRepository = cotizaciónRepository;
		}

		[HttpPut]
		[ActionName("Actualizar")]
		public IActionResult Actualizar([FromBody] Cotización algoParaActualizar)
		{
			var resultado = _cotizaciónRepository.Actualizar(algoParaActualizar);

			if (resultado == null)
				return NotFound();
			_cotizaciónRepository.SaveChanges();

			return Ok(_mapper.Map<CotizaciónDto>(resultado));
		}

		[HttpPost]
		[ActionName("Adicionar")]
		public IActionResult Adicionar([FromBody] Cotización algoParaAdicionar)
		{
			try
			{
				var resultado = _cotizaciónRepository.Adicionar(algoParaAdicionar);

				if (resultado == null)
					return NotFound();
				_cotizaciónRepository.SaveChanges();

				return Ok(_mapper.Map<CotizaciónDto>(resultado));
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
		public IActionResult Eliminar([FromBody] Cotización algoParaEliminar)
		{
			try
			{
				_cotizaciónRepository.Eliminar(algoParaEliminar);
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
			Expression<Func<Cotización, bool>> elWhere;
			if (string.IsNullOrEmpty(loquebusco))
				elWhere = null;
			else
				elWhere = n => n.Cliente.NombreCompleto.ToLower().Contains(loquebusco.ToLower()) || n.Proyecto.Código.ToLower().Contains(loquebusco.ToLower());

			var LaRespuesta = _cotizaciónRepository.TraerVariosPTAAsync(elWhere, o => o.Código, cuantospp);

			var resultado = LaRespuesta.Result;

			if (resultado == null)
				return NotFound();

			return Ok(_mapper.Map<List<CotizaciónDto>>(resultado));
		}

		[HttpGet]
		[ActionName("TraerPagina")]
		public IActionResult TraerPagina([FromQuery] string loquebusco, int pagina, int cuantospp = 10)
		{
			Expression<Func<Cotización, bool>> elWhere;


			if (string.IsNullOrEmpty(loquebusco))
				elWhere = null;
			else
				elWhere = n => n.Cliente.Nombre.ToLower().Contains(loquebusco.ToLower()) || n.Cliente.PrimerApellido.ToLower().Contains(loquebusco.ToLower()) || n.Proyecto.Código.ToLower().Contains(loquebusco.ToLower());

			var LaRespuesta = _cotizaciónRepository.TraerVariosAsync(elWhere
						, o => o.Código
						, new List<string> { "Proyecto", "Vendedor", "Cliente", "CotizaciónLíneas", "CotizaciónLíneas", "CotizaciónLíneas.Artículo"  }
						, pagina, cuantospp);

			var resultado = LaRespuesta.Result;

			if (resultado == null)
				return NotFound();

			return Ok(_mapper.Map<PaginatedList<CotizaciónDto>>(resultado));
		}

		[HttpGet("{cotizaciónId:uint}")]
		[ActionName("TraerUnoPorId")]
		public IActionResult TraerUnoPorId(uint cotizaciónId)
		{
			//			var resultado = _cotizaciónRepository.TraerUnoPorId(cotizaciónId);
			var resultado = _cotizaciónRepository.TraerUnoAsync(
				c=> c.CotizaciónId == cotizaciónId,
				new List<string> { "Proyecto", "Vendedor", "Cliente", "CotizaciónLíneas", "CotizaciónLíneas", "CotizaciónLíneas.Artículo" }
				);
			if (resultado == null)
				return NotFound();

			return Ok(_mapper.Map<CotizaciónDto>(resultado.Result));
		}
	}


}
