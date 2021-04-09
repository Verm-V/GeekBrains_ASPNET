﻿using System;
using System.Collections.Generic;

namespace MetricsAgent.Responses
{
	/// <summary>
	/// Контейнер для передачи списка метрик в ответе от сервера
	/// </summary>
	public class AllHddMetricsResponse
	{
		public List<HddMetricDto> Metrics { get; set; }
	}

	/// <summary>
	/// Контейнер для передачи метрики в ответе от сервера
	/// </summary>
	public class HddMetricDto
	{
		public TimeSpan Time { get; set; }
		public int Value { get; set; }
	}
}
