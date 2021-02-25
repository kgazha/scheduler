using Scheduler.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

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
        int year;
        int month;
        int day;
        int dayOfWeek;
        int hour;
        int minute;
        int second;
        int millisecond;

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
        }

        private int GetClosestYear(DateTime t1)
        {
            return Years.Aggregate((x, y) => Math.Abs(x - t1.Year) <= Math.Abs(y - t1.Year) ? x : y);
        }

        private int GetClosestMonth(DateTime t1, int year)
        {
            int closestMonth = 0;
            var maxDiff = double.MaxValue;
            double dateDiff;
            foreach (var item in Months)
            {
                dateDiff = Math.Abs((new DateTime(year: year, month: item, day: 1)
                    - new DateTime(year: t1.Year, month: t1.Month, day: 1)).Days);
                if (dateDiff < maxDiff)
                {
                    maxDiff = dateDiff;
                    closestMonth = item;
                }
                else
                {
                    break;
                }
            }
            return closestMonth;
        }

        private int GetClosestDay(DateTime t1, int year, int month)
        {
            Days = sequence.GenerateSequence(parser.Day, 1, DateTime.DaysInMonth(year, month));
            int closestDay = 0;
            var maxDiff = double.MaxValue;
            double dateDiff;
            foreach (var item in Days)
            {
                dateDiff = Math.Abs((new DateTime(year, month, item)
                    - new DateTime(t1.Year, t1.Month, t1.Day)).Days);
                if (dateDiff < maxDiff)
                {
                    maxDiff = dateDiff;
                    closestDay = item;
                }
                else
                {
                    break;
                }
            }
            return closestDay;
        }

        private int GetClosestHour(DateTime t1, int year, int month, int day)
        {
            int closestHour = 0;
            var maxDiff = double.MaxValue;
            double dateDiff;
            foreach (var item in Hours)
            {
                dateDiff = Math.Abs((new DateTime(year, month, day, item, minute: 1, second: 1)
                    - new DateTime(t1.Year, t1.Month, t1.Day, t1.Hour, minute: 1, second: 1)).Ticks);
                if (dateDiff < maxDiff)
                {
                    maxDiff = dateDiff;
                    closestHour = item;
                }
                else
                {
                    break;
                }
            }
            return closestHour;
        }

        private int GetClosestMinute(DateTime t1, int year, int month, int day,
            int hour)
        {
            int closestMinute = 0;
            var maxDiff = double.MaxValue;
            double dateDiff;
            foreach (var item in Minutes)
            {
                dateDiff = Math.Abs((new DateTime(year, month, day, hour, item, 1)
                    - new DateTime(t1.Year, t1.Month, t1.Day, t1.Hour, t1.Minute, 1)).Ticks);
                if (dateDiff < maxDiff)
                {
                    maxDiff = dateDiff;
                    closestMinute = item;
                }
                else
                {
                    break;
                }
            }
            return closestMinute;
        }

        private int GetClosestSecond(DateTime t1, int year, int month, int day,
            int hour, int minute)
        {
            int closestSecond = 0;
            var maxDiff = double.MaxValue;
            double dateDiff;
            foreach (var item in Seconds)
            {
                dateDiff = Math.Abs((new DateTime(year, month, day, hour, minute, item)
                    - new DateTime(t1.Year, t1.Month, t1.Day, t1.Hour, t1.Minute, t1.Second)).Ticks);
                if (dateDiff < maxDiff)
                {
                    maxDiff = dateDiff;
                    closestSecond = item;
                }
                else
                {
                    break;
                }
            }
            return closestSecond;
        }

        private int GetClosestMillisecond(DateTime t1, int year, int month, int day,
            int hour, int minute, int second)
        {
            int closestMillisecond = 0;
            var maxDiff = double.MaxValue;
            double dateDiff;
            foreach (var item in Milliseconds)
            {
                dateDiff = Math.Abs((new DateTime(year: year, month: month, day: day,
                    hour: hour, minute: minute, second: second, millisecond: item)
                    - new DateTime(year: t1.Year, month: t1.Month, day: t1.Day,
                    hour: t1.Hour, minute: t1.Minute, second: t1.Second, millisecond: t1.Millisecond)).Ticks);
                if (dateDiff < maxDiff)
                {
                    maxDiff = dateDiff;
                    closestMillisecond = item;
                }
                else
                {
                    break;
                }
            }
            return closestMillisecond;
        }

        private void InitializeClosestDateEntities(DateTime t1)
        {
            year = GetClosestYear(t1);
            month = GetClosestMonth(t1, year);
            day = GetClosestDay(t1, year, month);
        }

        private void InitializeClosestTimeEntities(DateTime t1)
        {
            hour = 0;
            minute = 0;
            second = 0;
            millisecond = 0;

            if (Hours.Count != 0)
            {
                hour = GetClosestHour(t1, year, month, day);
                minute = GetClosestMinute(t1, year, month, day, hour);
                second = GetClosestSecond(t1, year, month, day, hour, minute);
                millisecond = GetClosestMillisecond(t1, year, month, day, hour, minute, second);
            }
        }

        private DateTime FindPrevRelevantDate(DateTime datetime)
        {
            var relevantDateTime = new DateTime();
            day = datetime.Day;
            month = datetime.Month;
            year = datetime.Year;
            var minDay = Days.Min();
            var minMonth = Months.Min();
            var minYear = Years.Min();
            var minDate = new DateTime(minYear, minMonth, minDay).Date;
            bool subtracted = false;
            while (datetime.Date != minDate)
            {
                if (DaysOfWeek.Contains((int)datetime.DayOfWeek))
                {
                    relevantDateTime = datetime;
                    break;
                }
                else
                {
                    subtracted = false;
                }
                SubtractNext(Days, ref day, ref subtracted);
                var monthSubtracted = SubtractNext(Months, ref month, ref subtracted);
                if (monthSubtracted)
                {
                    day = GetClosestDay(datetime, year, month);
                }
                SubtractNext(Years, ref year, ref subtracted);
                datetime = new DateTime(year, month, day,
                    datetime.Hour, datetime.Minute, datetime.Second, datetime.Millisecond);
            }
            return relevantDateTime;
        }

        private DateTime FindNextRelevantDate(DateTime datetime)
        {
            var relevantDateTime = new DateTime();
            day = datetime.Day;
            month = datetime.Month;
            year = datetime.Year;
            var maxDay = Days.Max();
            var maxMonth = Months.Max();
            var maxYear = Years.Max();
            var maxDate = new DateTime(maxYear, maxMonth, maxDay).Date;
            bool added = false;
            while (datetime.Date != maxDate)
            {
                if (DaysOfWeek.Contains((int)datetime.DayOfWeek))
                {
                    relevantDateTime = datetime;
                    break;
                }
                else
                {
                    added = false;
                }
                AddNext(Days, ref day, ref added);
                var monthAdded = AddNext(Months, ref month, ref added);
                if (monthAdded)
                {
                    day = GetClosestDay(datetime, year, month);
                }
                AddNext(Years, ref year, ref added);
                datetime = new DateTime(year, month, day,
                    datetime.Hour, datetime.Minute, datetime.Second, datetime.Millisecond);
            }
            return relevantDateTime;
        }

        private bool AddNext(List<int> sourceValues, ref int value, ref bool added)
        {
            if (sourceValues.Count != 0 && !added)
            {
                int _value = value;
                var next = sourceValues.Where(x => x > _value);
                if (next.Count() > 0)
                {
                    added = true;
                    value = next.First();
                }
                else
                {
                    value = sourceValues.First();
                }
                return true;
            }
            return false;
        }

        private bool SubtractNext(List<int> sourceValues, ref int value, ref bool subtracted)
        {
            if (sourceValues.Count != 0 && !subtracted)
            {
                int _value = value;
                var next = sourceValues.Where(x => x < _value);
                if (next.Count() > 0)
                {
                    subtracted = true;
                    value = next.Last();
                }
                else
                {
                    value = sourceValues.Last();
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Возвращает следующий ближайший к заданному времени момент в расписании или
        /// само заданное время, если оно есть в расписании.
        /// </summary>
        /// <param name="t1">Заданное время</param>
        /// <returns>Ближайший момент времени в расписании</returns>
        public DateTime NearestEvent(DateTime t1)
        {
            var datetime = FindNext(t1, true);
            return datetime;
        }

        /// <summary>
		/// Возвращает предыдущий ближайший к заданному времени момент в расписании или
		/// само заданное время, если оно есть в расписании.
		/// </summary>
		/// <param name="t1">Заданное время</param>
		/// <returns>Ближайший момент времени в расписании</returns>
        public DateTime NearestPrevEvent(DateTime t1)
        {
            var datetime = FindPrev(t1, true);
            return datetime;
        }

        /// <summary>
		/// Возвращает следующий момент времени в расписании.
		/// </summary>
		/// <param name="t1">Время, от которого нужно отступить</param>
		/// <returns>Следующий момент времени в расписании</returns>
        public DateTime NextEvent(DateTime t1)
        {
            var dateTime = FindNext(t1, false);
            return dateTime;
        }

        /// <summary>
		/// Возвращает предыдущий момент времени в расписании.
		/// </summary>
		/// <param name="t1">Время, от которого нужно отступить</param>
		/// <returns>Предыдущий момент времени в расписании</returns>
        public DateTime PrevEvent(DateTime t1)
        {
            var datetime = FindPrev(t1, false);
            return datetime;
        }

        /// <summary>
        /// Возвращает следующий момент времени в расписании или указанное время.
        /// </summary>
        /// <param name="t1">Время, от которого нужно отступить</param>
        /// <param name="equal">Возвращать заданное время, если оно есть в расписании</param>
        /// <returns>Следующий момент времени в расписании</returns>
        private DateTime FindNext(DateTime t1, bool equal)
        {
            InitializeClosestDateEntities(t1);
            InitializeClosestTimeEntities(t1);
            var datetime = new DateTime(year: year, month: month, day: day,
                                hour: hour, minute: minute, second: second,
                                millisecond: millisecond);
            bool added = false;
            bool closestFound = false;
            var closestDiff = Math.Abs((t1 - datetime).Ticks);
            if (closestDiff == 0 && equal)
            {
                return t1;
            }
            var diff = closestDiff;
            var closestTime = datetime;
            while ((diff < closestDiff || !added) && !closestFound)
            {
                AddNext(Milliseconds, ref millisecond, ref added);
                AddNext(Seconds, ref second, ref added);
                AddNext(Minutes, ref minute, ref added);
                AddNext(Hours, ref hour, ref added);
                AddNext(Days, ref day, ref added);
                var monthAdded = AddNext(Months, ref month, ref added);
                if (monthAdded)
                {
                    day = GetClosestDay(datetime, year, month);
                }
                AddNext(Years, ref year, ref added);

                datetime = new DateTime(year: year, month: month, day: day,
                                hour: hour, minute: minute, second: second,
                                millisecond: millisecond);
                diff = Math.Abs((t1 - datetime).Ticks);
                if (closestDiff == 0 && !equal)
                {
                    closestDiff = diff;
                }
                if (diff <= closestDiff && diff != 0)
                {
                    closestDiff = diff;
                    closestTime = datetime;
                    added = false;
                }
                else
                {
                    closestFound = true;
                }
            }
            if (closestTime < t1)
            {
                closestTime = new DateTime();
            }
            else if (DaysOfWeek.Count != 0)
            {
                closestTime = FindNextRelevantDate(closestTime);
            }
            return closestTime;
        }

        /// <summary>
        /// Возвращает предыдущий момент времени в расписании или указанное время.
        /// </summary>
        /// <param name="t1">Время, от которого нужно отступить</param>
        /// <param name="equal">Возвращать заданное время, если оно есть в расписании</param>
        /// <returns>Предыдущий момент времени в расписании</returns>
        private DateTime FindPrev(DateTime t1, bool equal)
        {
            InitializeClosestDateEntities(t1);
            InitializeClosestTimeEntities(t1);
            var datetime = new DateTime(year: year, month: month, day: day,
                                hour: hour, minute: minute, second: second,
                                millisecond: millisecond);
            bool subtracted = false;
            bool closestFound = false;
            var closestDiff = Math.Abs((t1 - datetime).Ticks);
            if (closestDiff == 0 && equal)
            {
                return t1;
            }
            var diff = closestDiff;
            var closestTime = datetime;
            while ((diff < closestDiff || !subtracted) && !closestFound)
            {
                SubtractNext(Milliseconds, ref millisecond, ref subtracted);
                SubtractNext(Seconds, ref second, ref subtracted);
                SubtractNext(Minutes, ref minute, ref subtracted);
                SubtractNext(Hours, ref hour, ref subtracted);
                SubtractNext(Days, ref day, ref subtracted);
                var monthSubtracted = SubtractNext(Months, ref month, ref subtracted);
                if (monthSubtracted)
                {
                    day = GetClosestDay(datetime, year, month);
                }
                SubtractNext(Years, ref year, ref subtracted);

                datetime = new DateTime(year: year, month: month, day: day,
                                hour: hour, minute: minute, second: second,
                                millisecond: millisecond);
                diff = Math.Abs((t1 - datetime).Ticks);
                if (closestDiff == 0 && !equal)
                {
                    closestDiff = diff;
                }
                if (diff <= closestDiff && diff != 0)
                {
                    closestDiff = diff;
                    closestTime = datetime;
                    subtracted = false;
                }
                else
                {
                    closestFound = true;
                }
            }
            if (closestTime > t1)
            {
                closestTime = new DateTime();
            }
            else if (DaysOfWeek.Count != 0)
            {
                closestTime = FindPrevRelevantDate(closestTime);
            }
            return closestTime;
        }
    }
}
