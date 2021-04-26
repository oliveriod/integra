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
	public class ProveedoresController : ControllerBase
	{
		private readonly IMapper _mapper;
		private readonly ILogger<Proveedor> _logger;
		private readonly IProveedorRepository _proveedorRepository;

		public ProveedoresController(
			IMapper mapper,
			ILogger<Proveedor> logger,
			IProveedorRepository proveedorRepository)
		{
			_mapper = mapper;
			_logger = logger;
			_proveedorRepository = proveedorRepository;
		}

		[HttpPut]
		[ActionName("Actualizar")]
		public IActionResult Actualizar([FromBody] Proveedor algoParaActualizar)
		{
			var resultado = _proveedorRepository.Actualizar(algoParaActualizar);

			if (resultado == null)
				return NotFound();
			_proveedorRepository.SaveChanges();

			return Ok(_mapper.Map<ProveedorDto>(resultado));
		}

		[HttpPost]
		[ActionName("Adicionar")]
		public IActionResult Adicionar([FromBody] Proveedor algoParaAdicionar)
		{
			var resultado = _proveedorRepository.Adicionar(algoParaAdicionar);

			if (resultado == null)
				return NotFound();
			_proveedorRepository.SaveChanges();

			return Ok(_mapper.Map<ProveedorDto>(resultado));
		}

		[HttpDelete]
		[ActionName("Eliminar")]
		public IActionResult Eliminar([FromBody] Proveedor algoParaEliminar)
		{
			try
			{
				_proveedorRepository.Eliminar(algoParaEliminar);
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
			Expression<Func<Proveedor, bool>> elWhere;
			if (string.IsNullOrEmpty(loquebusco))
				elWhere = null;
			else
				elWhere = n => n.Nombre.ToLower().Contains(loquebusco.ToLower());

			var LaRespuesta = _proveedorRepository.TraerVariosPTAAsync(elWhere, o => o.Nombre, cuantospp);

			var resultado = LaRespuesta.Result;

			if (resultado == null)
				return NotFound();

			return Ok(_mapper.Map<List<ProveedorDto>>(resultado));
		}

		[HttpGet]
		[ActionName("TraerPagina")]
		public IActionResult TraerPagina([FromQuery] string loquebusco, int pagina, int cuantospp = 10)
		{
			Expression<Func<Proveedor, bool>> elWhere;
			if (string.IsNullOrEmpty(loquebusco))
				elWhere = n => n.EstadoId == EstadoEnum.Activo;
			else
				elWhere = n => n.EstadoId == EstadoEnum.Activo && n.Nombre.ToLower().Contains(loquebusco.ToLower());

			var LaRespuesta = _proveedorRepository.TraerVariosAsync(elWhere, o => o.Nombre, pagina, cuantospp);

			var resultado = LaRespuesta.Result;

			if (resultado == null)
				return NotFound();

			return Ok(_mapper.Map<PaginatedList<ProveedorDto>>(resultado));
		}

		[HttpGet("{proveedorId:uint}")]
		[ActionName("TraerUnoPorId")]
		public IActionResult TraerUnoPorId(uint proveedorId)
		{
			var resultado = _proveedorRepository.TraerUnoPorId(proveedorId);

			if (resultado == null)
				return NotFound();

			return Ok(_mapper.Map<ProveedorDto>(resultado));
		}
	}


}
