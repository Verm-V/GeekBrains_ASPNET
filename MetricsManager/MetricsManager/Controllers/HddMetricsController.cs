﻿using AutoMapper;
using Metrics.Tools;
using MetricsManager.DAL;
using MetricsManager.Requests;
using MetricsManager.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.Controllers
{
	[Route("api/metrics/hdd")]
	[ApiController]
	public class HddMetricsController : ControllerBase
	{
		private readonly ILogger<HddMetricsController> _logger;
		private readonly IHddMetricsRepository _repository;
		private readonly IAgentsRepository _agentRepository;
		private readonly IMapper _mapper;

		public HddMetricsController(
			ILogger<HddMetricsController> logger,
			IHddMetricsRepository repository,
			IAgentsRepository agentRepository,
			IMapper mapper)
		{
			_logger = logger;
			_logger.LogDebug("Вызов конструктора");
			_repository = repository;
			_mapper = mapper;
			_agentRepository = agentRepository;
		}

		[HttpGet("agent/{request.agentId}/from/{request.fromTime}/to/{request.toTime}")]
		public IActionResult GetMetricsFromAgent([FromRoute] HddMetricGetByIntervalForAgentRequest request)
		{
			_logger.LogDebug("Вызов метода. Параметры:" +
				$" {nameof(request.agentId)} = {request.agentId}" +
				$" {nameof(request.fromTime)} = {request.fromTime}" +
				$" {nameof(request.toTime)} = {request.toTime}");

			var metrics = _repository.GetByTimeInterval(request.agentId, request.fromTime, request.toTime);

			var response = new AllHddMetricsResponse();

			foreach (var metric in metrics.Metrics)
			{
				response.Metrics.Add(_mapper.Map<HddMetricDto>(metric));
			}

			return Ok(response);
		}

		[HttpGet("agent/{request.agentId}/from/{request.fromTime}/to/{request.toTime}/percentiles/{percentile}")]
		public IActionResult GetMetricsByPercentileFromAgent(
			[FromRoute] HddMetricGetByIntervalForAgentRequest request,
			[FromRoute] Percentile percentile)
		{
			_logger.LogDebug("Вызов метода. Параметры:" +
				$" {nameof(request.agentId)} = {request.agentId}" +
				$" {nameof(request.fromTime)} = {request.fromTime}" +
				$" {nameof(request.toTime)} = {request.toTime}" +
				$" {nameof(percentile)} = {percentile}");

			var metrics = _repository.GetByTimeIntervalPercentile(request.agentId, request.fromTime, request.toTime, percentile);

			var response = new AllHddMetricsResponse();

			foreach (var metric in metrics.Metrics)
			{
				response.Metrics.Add(_mapper.Map<HddMetricDto>(metric));
			}

			return Ok(response);
		}

		[HttpGet("cluster/from/{request.fromTime}/to/{request.toTime}")]
		public IActionResult GetMetricsFromAllCluster([FromRoute] HddMetricGetByIntervalForClusterRequest request)
		{
			_logger.LogDebug("Вызов метода. Параметры:" +
				$" {nameof(request.fromTime)} = {request.fromTime}" +
				$" {nameof(request.toTime)} = {request.toTime}");

			var agents = _agentRepository.GetAllAgentsInfo();

			var response = new AllHddMetricsResponse();

			foreach (var agent in agents.Agents)
			{
				var currentAgentMetrics = _repository.GetByTimeInterval(agent.AgentId, request.fromTime, request.toTime);

				foreach (var metric in currentAgentMetrics.Metrics)
				{
					response.Metrics.Add(_mapper.Map<HddMetricDto>(metric));
				}
			}

			return Ok(response);
		}

		[HttpGet("cluster/from/{request.fromTime}/to/{request.toTime}/percentiles/{percentile}")]
		public IActionResult GetMetricsByPercentileFromAllCluster(
			[FromRoute] HddMetricGetByIntervalForClusterRequest request,
			[FromRoute] Percentile percentile)
		{
			_logger.LogDebug("Вызов метода. Параметры:" +
				$" {nameof(request.fromTime)} = {request.fromTime}" +
				$" {nameof(request.toTime)} = {request.toTime}" +
				$" {nameof(percentile)} = {percentile}");

			var agents = _agentRepository.GetAllAgentsInfo();

			var response = new AllHddMetricsResponse();

			foreach (var agent in agents.Agents)
			{
				var currentAgentMetrics = _repository.GetByTimeIntervalPercentile(agent.AgentId, request.fromTime, request.toTime, percentile);

				foreach (var metric in currentAgentMetrics.Metrics)
				{
					response.Metrics.Add(_mapper.Map<HddMetricDto>(metric));
				}

			}

			return Ok(response);
		}
	}
}
