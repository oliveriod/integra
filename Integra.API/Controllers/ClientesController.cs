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
	public class ClientesController : ControllerBase
	{
		private readonly IMapper _mapper;
		private readonly ILogger<Cliente> _logger;
		private readonly IClienteRepository _clienteRepository;

		public ClientesController(
			IMapper mapper,
			ILogger<Cliente> logger,
			IClienteRepository clienteRepository)
		{
			_mapper = mapper;
			_logger = logger;
			_clienteRepository = clienteRepository;
		}

		[HttpPut]
		[ActionName("Actualizar")]
		public IActionResult Actualizar([FromBody] Cliente algoParaActualizar)
		{
			try
			{

				var resultado = _clienteRepository.Actualizar(algoParaActualizar);

				if (resultado == null)
					return NotFound();
				_clienteRepository.SaveChanges();

				return Ok(_mapper.Map<ClienteDto>(resultado));
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				_logger.LogError(ex.StackTrace);
				return Problem(ex.Message);
			}
		}

		[HttpPost]
		[ActionName("Adicionar")]
		public IActionResult Adicionar([FromBody] Cliente algoParaAdicionar)
		{
			try
			{
				var resultado = _clienteRepository.Adicionar(algoParaAdicionar);

				if (resultado == null)
					return NotFound();
				_clienteRepository.SaveChanges();

				var otroResultado = _clienteRepository.TraerUnoPorId(resultado.ClienteId);

				return Ok(_mapper.Map<ClienteDto>(resultado));
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
		public IActionResult Eliminar([FromBody] Cliente algoParaEliminar)
		{
			try
			{
				_clienteRepository.Eliminar(algoParaEliminar);
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
			Expression<Func<Cliente, bool>> elWhere;
			if (string.IsNullOrEmpty(loquebusco))
				elWhere = null;
			else
				elWhere = n => n.Nombre.ToLower().Contains(loquebusco.ToLower())
					|| n.PrimerApellido.ToLower().Contains(loquebusco.ToLower())
					|| n.SegundoApellido.ToLower().Contains(loquebusco.ToLower());

			var LaRespuesta = _clienteRepository.TraerVariosPTAAsync(elWhere, o => o.Nombre, cuantospp);

			var resultado = LaRespuesta.Result;

			if (resultado == null)
				return NotFound();

			return Ok(_mapper.Map<List<ClienteDto>>(resultado));
		}

		[HttpGet]
		[ActionName("TraerPagina")]
		public IActionResult TraerPagina([FromQuery] string loquebusco, int pagina, int cuantospp = 10)
		{
			Expression<Func<Cliente, bool>> elWhere;
			if (string.IsNullOrEmpty(loquebusco))
				elWhere = n => n.EstadoId == EstadoEnum.Activo;
			else
				elWhere = n => n.EstadoId == EstadoEnum.Activo && ( n.Nombre.ToLower().Contains(loquebusco.ToLower())
					|| n.PrimerApellido.ToLower().Contains(loquebusco.ToLower())
					|| n.SegundoApellido.ToLower().Contains(loquebusco.ToLower()));

			var LaRespuesta = _clienteRepository.TraerVariosAsync(elWhere, o => o.Nombre, pagina, cuantospp);

			var resultado = LaRespuesta.Result;

			if (resultado == null)
				return NotFound();

			return Ok(_mapper.Map<PaginatedList<ClienteDto>>(resultado));
		}

		[HttpGet("{clienteId:int}")]
		[ActionName("TraerUnoPorId")]
		public IActionResult TraerUnoPorId(uint clienteId)
		{
			var resultado = _clienteRepository.TraerUnoPorId(clienteId);

			if (resultado == null)
				return NotFound();

			return Ok(_mapper.Map<ClienteDto>(resultado));
		}


	}

}
