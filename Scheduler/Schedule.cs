using Scheduler.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scheduler
{
	public class Schedule
	{
		public IDateTimeParser parser = new DateTimeParser(new StringParser());
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
			result.AddMilliseconds(1);
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
