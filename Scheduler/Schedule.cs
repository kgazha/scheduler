﻿using Scheduler.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scheduler
{
	public class Schedule
	{
		const int MinYear = 2000;
		const int MaxYear = 2100;
		const int FirstMonth = 1;
		const int LastMonth = 12;
		const int FirstDayOfWeek = 0;
		const int LastDayOfWeek = 6;
		const int MinHour = 0;
		const int MaxHour = 23;
		const int MinMinute = 0;
		const int MaxMinute = 59;
		const int MinSecond = 0;
		const int MaxSecond = 59;
		const int MinMillisecond = 0;
		const int MaxMillisecond = 999;
		public IDateTimeParser parser = new DateTimeParser(new StringParser());
		public ISequence sequence = new SequenceCreator();
		string defaultSchedule = "*.*.* * *:*:*.*";
		List<int> Years { get; set; }
		List<int> Months { get; set; }
		List<int> Days { get; set; }
		List<int> DaysOfWeek { get; set; }
		List<int> Hours { get; set; }
		List<int> Minutes { get; set; }
		List<int> Seconds { get; set; }
		List<int> Milliseconds { get; set; }

		public List<DateTime> PossibleDates { get; set; }

		public Schedule()
        {
			Initialize(defaultSchedule);
		}

		public Schedule(string scheduleString)
		{
			Initialize(scheduleString);
		}

		private void Initialize(string scheduleString)
        {
			parser.Parse(scheduleString);
			Years = sequence.GenerateSequence(parser.Year, MinYear, MaxYear);
			Months = sequence.GenerateSequence(parser.Month, FirstMonth, LastMonth);
			DaysOfWeek = sequence.GenerateSequence(parser.DayOfWeek, FirstDayOfWeek, LastDayOfWeek);
			Hours = sequence.GenerateSequence(parser.Hour, MinHour, MaxHour);
			Minutes = sequence.GenerateSequence(parser.Minute, MinMinute, MaxMinute);
			Seconds = sequence.GenerateSequence(parser.Second, MinSecond, MaxSecond);
			Milliseconds = sequence.GenerateSequence(parser.Millisecond, MinMillisecond, MaxMillisecond);
			GeneratePossibleDates();
		}

		public void GeneratePossibleDates()
        {
			PossibleDates = new List<DateTime>();
			foreach (var year in Years)
            {
                foreach (var month in Months)
                {
					Days = sequence.GenerateSequence(parser.Day, 1, DateTime.DaysInMonth(year, month));
                    foreach (var day in Days)
                    {
						PossibleDates.Add(new DateTime(year: year, month: month, day: day));
                    }
                }
            }
        }

		/// <summary>
		/// Возвращает следующий ближайший к заданному времени момент в расписании или
		/// само заданное время, если оно есть в расписании.
		/// </summary>
		/// <param name="t1">Заданное время</param>
		/// <returns>Ближайший момент времени в расписании</returns>
		public DateTime NearestEvent(DateTime t1)
		{
			DateTime result = GetNearestDate(t1);
			return result;
		}

		private DateTime GetNearestDate(DateTime t1)
		{
			DateTime result = new DateTime();
			int startYear = Years.Aggregate((x, y) => Math.Abs(x - t1.Year) <= Math.Abs(y - t1.Year) ? x : y);

			double closestDate = double.MaxValue;
			
			foreach (var year in Years.Where(x => x >= startYear))
			{
				foreach (var month in Months)
				{
					Days = sequence.GenerateSequence(parser.Day, 1, DateTime.DaysInMonth(year, month));
					foreach (var day in Days)
					{

						DateTime date = new DateTime(year: year, month: month, day: day);

						if (DaysOfWeek.Count != 0)
                        {
							if (!DaysOfWeek.Contains((int)date.DayOfWeek))
                            {
								continue;
                            }
						}
						if (Math.Abs((date - t1).TotalDays) < closestDate)
						{
							closestDate = Math.Abs((date - t1).TotalDays);
							result = date;
						}
						else
                        {
							return GetNearestDateTime(t1, result);
                        }
					}
				}
			}
			return result;
		}

		private DateTime GetNearestDateTime(DateTime t1, DateTime currentResult)
        {
			double closestDateTime = double.MaxValue;
			foreach (var hour in Hours)
			{
				foreach (var minute in Minutes)
				{
					foreach (var second in Seconds)
					{
						if (Milliseconds.Count > 0)
						{
							foreach (var millisecond in Milliseconds)
							{
								var datetime = new DateTime(year: currentResult.Year, month: currentResult.Month, day: currentResult.Day,
									hour: hour, minute: minute, second: second, millisecond: millisecond);
								if (Math.Abs((datetime - t1).TotalMilliseconds) < closestDateTime)
								{
									closestDateTime = Math.Abs((datetime - t1).TotalMilliseconds);
									currentResult = datetime;
								}
								else
								{
									return currentResult;
								}
							}
						}
						else
						{
							var datetime = new DateTime(year: currentResult.Year, month: currentResult.Month, day: currentResult.Day,
								hour: hour, minute: minute, second: second);
							if (Math.Abs((datetime - t1).TotalMilliseconds) < closestDateTime)
							{
								closestDateTime = Math.Abs((datetime - t1).TotalMilliseconds);
								currentResult = datetime;
							}
							else
							{
								return currentResult;
							}
						}
					}
				}
			}
			return currentResult;
		}

		//public DateTime NearestPrevEvent(DateTime t1)
		//{

		//}

		//public DateTime NextEvent(DateTime t1)
		//{

		//}

		//public DateTime PrevEvent(DateTime t1)
		//{

		//}

	}

}
