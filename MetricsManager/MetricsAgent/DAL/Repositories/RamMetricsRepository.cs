﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SQLite;
using Dapper;

namespace MetricsAgent.DAL
{
	/// <summary>
	/// Маркировочный интерфейс. Необходим, чтобы проверить работу репозитория на тесте-заглушке 
	/// </summary>
	public interface IRamMetricsRepository : IRepository<RamMetric>
	{
	}

	/// <summary>
	/// Репозиторий для обработки Ram метрик
	/// </summary>
	public class RamMetricsRepository : IRamMetricsRepository
	{
		/// <summary>
		/// Имя таблицы с которой работаем
		/// </summary>
		private const string TableName = "rammetrics";

		/// <summary>
		/// Строка подключения
		/// </summary>
		private const string ConnectionString = @"Data Source=metrics.db; Version=3;Pooling=True;Max Pool Size=100;";

		public RamMetricsRepository()
		{
			// Добавляем парсилку типа TimeSpan в качестве подсказки для SQLite
			SqlMapper.AddTypeHandler(new TimeSpanHandler());
		}

		/// <summary>
		/// Возвращает список с метриками за заданный интервал времени
		/// </summary>
		/// <param name="fromTime">Начало временного интервала</param>
		/// <param name="toTime">Конец временного интервала</param>
		/// <returns>Список с метриками за заданный интервал времени</returns>
		public IList<RamMetric> GetByTimeInterval(DateTimeOffset fromTime, DateTimeOffset toTime)
		{
			var returnList = new List<RamMetric>();
			using (var connection = new SQLiteConnection(ConnectionString))
			{
				return connection.Query<RamMetric>(
				"SELECT * " +
				$"FROM {TableName} " +
				"WHERE (time >= @fromTime AND time <= @toTime)",
				new
				{
					fromTime = fromTime.ToUnixTimeSeconds(),
					toTime = toTime.ToUnixTimeSeconds(),
				}).ToList();
			}
		}

		/// <summary>
		/// Извлекает последнюю по дате метрику из базы данных
		/// </summary>
		/// <returns>Последняя по времени метрика из базы данных</returns>
		public RamMetric GetLast()
		{
			using (var connection = new SQLiteConnection(ConnectionString))
			{
				return connection.QuerySingle<RamMetric>(
				"SELECT * " +
				$"FROM {TableName} " +
				$"WHERE time = (SELECT MAX (time) FROM {TableName})");
			}
		}
	}
}
