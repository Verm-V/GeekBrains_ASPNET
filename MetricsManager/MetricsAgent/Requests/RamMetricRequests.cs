﻿using Microsoft.AspNetCore.Mvc;
using System;

namespace MetricsAgent.Requests
{
	/// <summary>
	/// Контейнер для запроса метрик из базы
	/// </summary>
	public class RamMetricGetByIntervalRequest
	{
		[FromRoute]
		public DateTimeOffset fromTime { get; set; }
		[FromRoute]
		public DateTimeOffset toTime { get; set; }
	}
}
