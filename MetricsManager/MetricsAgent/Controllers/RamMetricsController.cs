﻿using MetricsAgent.DAL;
using MetricsAgent.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AutoMapper;
using MetricsAgent.Requests;
using System.Collections.Generic;

namespace MetricsAgent.Controllers
{
	/// <summary>
	/// Контроллер для обработки Ram метрик
	/// </summary>
	[Route("api/metrics/ram")]
	[ApiController]
	public class RamMetricsController : ControllerBase
	{
		private readonly ILogger<RamMetricsController> _logger;
		private readonly IRamMetricsRepository _repository;
		private readonly IMapper _mapper;

		public RamMetricsController(ILogger<RamMetricsController> logger, IRamMetricsRepository repository, IMapper mapper)
		{
			_logger = logger;
			_logger.LogDebug("Вызов конструктора");
			_repository = repository;
			_mapper = mapper;
		}

		/// <summary>
		/// Получение Ram метрик за заданный промежуток времени
		/// </summary>
		/// <param name="request">Запрос на выдачу метрик с интервалом времени</param>
		/// <returns>Список метрик за заданный интервал времени</returns>
		[HttpGet("from/{fromTime}/to/{toTime}")]
		public IActionResult GetMetrics([FromRoute] RamMetricGetByIntervalRequest request)
		{
			_logger.LogDebug("Вызов метода. Параметры:" +
				$" {nameof(request.fromTime)} = {request.fromTime}" +
				$" {nameof(request.toTime)} = {request.toTime}");

			var metrics = _repository.GetByTimeInterval(request.fromTime, request.toTime);

			var response = new AllRamMetricsResponse()
			{
				Metrics = new List<RamMetricDto>()
			};

			foreach (var metric in metrics)
			{
				response.Metrics.Add(_mapper.Map<RamMetricDto>(metric));
			}

			return Ok(response);
		}

	}
}
