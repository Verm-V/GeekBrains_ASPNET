﻿using System;

namespace MetricsAgent.DAL
{
	public class RamMetric
	{
		public int Value { get; set; }

		public DateTimeOffset Time { get; set; }
	}
}
