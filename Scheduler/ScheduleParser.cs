using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Scheduler
{
    public class ScheduleParser : IDateTimeParser
    {
        //List<string> DatePatterns { get; set; }
        public string Year { get; set; }
        public string Month { get; set; }
        public string Day { get; set; }
        public string DayOfWeek { get; set; }
        public string Hour { get; set; }
        public string Minute { get; set; }
        public string Second { get; set; }
        public string Milisecond { get; set; }

        // Шаблон для даты со списками и диапазонами. Пример: 2000-2010,2014,2015.02-3.16
        string datePattern = @"(((\d{4})?\*?,?-?)*)\.(((\d{1,2})?\*?,?-?)*)\.(((\d{1,2})?\*?,?-?)*)";
        string dayOfWeekPattern = @"\s((\d{1},?-?)+)\s";
        string timePattern = @"(\S+):(\S+):(((\d{1,2})?\*?,?-?\/?)*)\.?(\S+)?";

        public ScheduleParser()
        {
        }

        private void ParseDate(string scheduleString)
        {
            var result = Regex.Match(scheduleString, datePattern);
            if (result.Success)
            {
                Year = result.Groups[1].Value;
                Month = result.Groups[4].Value;
                Day = result.Groups[7].Value;
            }
        }

        private void ParseDayOfWeek(string scheduleString)
        {
            var result = Regex.Match(scheduleString, dayOfWeekPattern);
            if (result.Success)
            {
                DayOfWeek = result.Groups[1].Value;
            }
        }

        private void ParseTime(string scheduleString)
        {
            var result = Regex.Match(scheduleString, timePattern);
            if (result.Success)
            {
                Hour = result.Groups[1].Value;
                Minute = result.Groups[2].Value;
                Second = result.Groups[3].Value;
                if (result.Groups.Count == 7)
                {
                    Milisecond = result.Groups[6].Value;
                }
            }
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
            Milisecond = "";
            ParseDate(scheduleString);
            ParseDayOfWeek(scheduleString);
            ParseTime(scheduleString);
        }

    }
}
