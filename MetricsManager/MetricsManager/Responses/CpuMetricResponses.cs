﻿using System;
using System.Collections.Generic;

namespace MetricsManager.Responses
{
	/// <summary>
	/// Контейнер для передачи списка метрик в ответе от сервера
	/// </summary>
	public class AllCpuMetricsResponse
	{
		public List<CpuMetricDto> Metrics { get; set; }

		public AllCpuMetricsResponse()
		{
			Metrics = new List<CpuMetricDto>();
		}

	}

	/// <summary>
	/// Контейнер для передачи метрики в ответе от сервера
	/// </summary>
	public class CpuMetricDto
	{
		public int AgentId { get; set; }
		public DateTimeOffset Time { get; set; }
		public int Value { get; set; }
	}

	/// <summary>
	/// Контейнер для передачи списка метрик в ответе от сервера
	/// </summary>
	public class AllAgentCpuMetricsResponse
	{
		public List<CpuMetricFromAgentDto> Metrics { get; set; }
	}

	/// <summary>
	/// Контейнер для передачи метрики в ответе от сервера
	/// </summary>
	public class CpuMetricFromAgentDto
	{
		public DateTimeOffset Time { get; set; }
		public int Value { get; set; }
	}

}