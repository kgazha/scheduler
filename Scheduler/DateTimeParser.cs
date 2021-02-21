using Scheduler.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scheduler
{
    public class DateTimeParser : IDateTimeParser
    {
        public string Year { get; set; }
        public string Month { get; set; }
        public string Day { get; set; }
        public string DayOfWeek { get; set; }
        public string Hour { get; set; }
        public string Minute { get; set; }
        public string Second { get; set; }
        public string Millisecond { get; set; }
        IParser parser;

        public DateTimeParser(IParser parser)
        {
            this.parser = parser;
        }

        // Шаблон для даты со списками и диапазонами. Пример: 2000-2010,2014,2015.02-3.16
        string datePattern = @"(((\d{4})?\*?,?-?)*)\.(((\d{1,2})?\*?,?-?)*)\.(((\d{1,2})?\*?,?-?)*)";
        string dayOfWeekPattern = @"\s(((\d{1})?\*?,?-?)+)\s";
        string timePattern = @"(\S+):(\S+):(((\d{1,2})?\*?,?-?\/?)*)\.?(\S+)?\*?";

        private void ParseDate(string scheduleString)
        {
            Year = parser.Parse(scheduleString, datePattern, 1);
            Month = parser.Parse(scheduleString, datePattern, 4);
            Day = parser.Parse(scheduleString, datePattern, 7);
        }

        private void ParseDayOfWeek(string scheduleString)
        {
            DayOfWeek = parser.Parse(scheduleString, dayOfWeekPattern, 1);
        }

        private void ParseTime(string scheduleString)
        {
            Hour = parser.Parse(scheduleString, timePattern, 1);
            Minute = parser.Parse(scheduleString, timePattern, 2);
            Second = parser.Parse(scheduleString, timePattern, 3);
            Millisecond = parser.Parse(scheduleString, timePattern, 6);
        }

        public void Parse(string scheduleString)
        {
            Year = "";
            Month = "";
            Day = "";
            DayOfWeek = "";
            Hour = "";
            Minute = "";
            Second = "";
            Millisecond = "";
            ParseDate(scheduleString);
            ParseDayOfWeek(scheduleString);
            ParseTime(scheduleString);
        }
    }
}
