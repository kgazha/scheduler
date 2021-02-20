using Scheduler.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scheduler
{
	public class Schedule
	{
		public IDateTimeParser parser = new DateTimeParser(new StringParser());
		public ISequence sequence = new SequenceCreator();
		string defaultSchedule = "*.*.* * *:*:*.*";

		public Schedule()
        {
			parser.Parse(defaultSchedule);
        }

		public Schedule(string scheduleString)
		{
			parser.Parse(scheduleString);
		}

		public DateTime NearestEvent(DateTime t1)
		{
			DateTime result = new DateTime();
			var years = sequence.GenerateSequence(parser.Year, 2000, 2100);
			int year = years.Aggregate((x, y) => Math.Abs(x - t1.Year) <= Math.Abs(y - t1.Year) ? x : y);
			var months = sequence.GenerateSequence(parser.Month, 1, 12);
			int month = 0;
            if (year < t1.Year)
            {
				month = months.Aggregate((x, y) => Math.Abs(x - t1.Month) >= Math.Abs(y - t1.Month) ? x : y);
			}
			else
            {
				month = months.Aggregate((x, y) => Math.Abs(x - t1.Month) <= Math.Abs(y - t1.Month) ? x : y);
			}
			int day = 0;
            if (parser.Day == "32")
            {
				day = DateTime.DaysInMonth(year, month);
			}
			else
            {
				var days = sequence.GenerateSequence(parser.Day, 1, DateTime.DaysInMonth(year, month));
				day = days.Aggregate((x, y) => Math.Abs(x - t1.Day) <= Math.Abs(y - t1.Day) ? x : y);
            }
			result = new DateTime(year: year, month: month, day: day);
			return result;
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
