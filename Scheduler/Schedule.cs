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
            DateTime result = FindDate(t1, true, true);
            return result;
        }

        private DateTime FindDate(DateTime t1, bool findEqual, bool findNext)
        {
            DateTime result = new DateTime();
            int year = Years.Aggregate((x, y) => Math.Abs(x - t1.Year) <= Math.Abs(y - t1.Year) ? x : y);

            int month = Months.Aggregate((x, y) => Math.Abs(x - t1.Month) <= Math.Abs(y - t1.Month) ? x : y);
            Days = sequence.GenerateSequence(parser.Day, 1, DateTime.DaysInMonth(year, month));
            int day = Days.Aggregate((x, y) => Math.Abs(x - t1.Day) <= Math.Abs(y - t1.Day) ? x : y);
            int hour = 0;
            int minute = 0;
            int second = 0;
            int millisecond = 0;

            if (Hours.Count != 0)
            {
                hour = Hours.Aggregate((x, y) => Math.Abs(x - t1.Hour) <= Math.Abs(y - t1.Hour) ? x : y);
                minute = Minutes.Aggregate((x, y) => Math.Abs(x - t1.Minute) <= Math.Abs(y - t1.Minute) ? x : y);
                second = Seconds.Aggregate((x, y) => Math.Abs(x - t1.Second) <= Math.Abs(y - t1.Second) ? x : y);
                millisecond = Milliseconds.Aggregate((x, y) => Math.Abs(x - t1.Millisecond) <= Math.Abs(y - t1.Millisecond) ? x : y);
            }

            var datetime = new DateTime(year: year, month: month, day: day,
                                hour: hour, minute: minute, second: second,
                                millisecond: millisecond);
            var closestDiff = Math.Abs((t1 - datetime).TotalMilliseconds);
            //if (findEqual &&)
            if (findNext && !findEqual)
            {
                bool added = false;
                var diff = Math.Abs((t1 - datetime).TotalMilliseconds);
                while (diff < closestDiff || !added)
                {
                    AddNext(Milliseconds, ref millisecond, ref added);
                    AddNext(Seconds, ref second, ref added);
                    AddNext(Minutes, ref minute, ref added);
                    AddNext(Hours, ref hour, ref added);
                    AddNext(Days, ref day, ref added);
                    AddNext(Months, ref month, ref added);
                    AddNext(Years, ref year, ref added);
                    if (added)
                    {
                        Days = sequence.GenerateSequence(parser.Day, 1, DateTime.DaysInMonth(year, month));
                    }

                    datetime = new DateTime(year: year, month: month, day: day,
                                    hour: hour, minute: minute, second: second,
                                    millisecond: millisecond);
                    diff = Math.Abs((t1 - datetime).TotalMilliseconds);
                    if (diff < closestDiff)
                    {
                        closestDiff = diff;
                    }
                }
            }

            if (findNext && findEqual)
            {
                var diff = Math.Abs((t1 - datetime).TotalMilliseconds);
                bool added = false;
                bool closestFound = false;
                DateTime closestTime = datetime;
                while (!closestFound || !added)
                {
                    AddNext(Milliseconds, ref millisecond, ref added);
                    AddNext(Seconds, ref second, ref added);
                    AddNext(Minutes, ref minute, ref added);
                    AddNext(Hours, ref hour, ref added);
                    AddNext(Days, ref day, ref added);
                    AddNext(Months, ref month, ref added);
                    AddNext(Years, ref year, ref added);
                    if (added)
                    {
                        Days = sequence.GenerateSequence(parser.Day, 1, DateTime.DaysInMonth(year, month));
                    }

                    datetime = new DateTime(year: year, month: month, day: day,
                                    hour: hour, minute: minute, second: second,
                                    millisecond: millisecond);
                    diff = Math.Abs((t1 - datetime).TotalMilliseconds);
                    if (diff < closestDiff)
                    {
                        closestDiff = diff;
                        closestTime = datetime;
                        added = false;
                    }
                    else
                    {
                        closestFound = true;
                        datetime = closestTime;
                    }
                }
            }

            if (findEqual && !findNext)
            {
                var diff = Math.Abs((t1 - datetime).TotalMilliseconds);
                bool subtracted = false;
                bool closestFound = false;
                DateTime closestTime = datetime;
                while (!closestFound || !subtracted)
                {
                    SubtractNext(Milliseconds, ref millisecond, ref subtracted);
                    SubtractNext(Seconds, ref second, ref subtracted);
                    SubtractNext(Minutes, ref minute, ref subtracted);
                    SubtractNext(Hours, ref hour, ref subtracted);
                    SubtractNext(Days, ref day, ref subtracted);
                    SubtractNext(Months, ref month, ref subtracted);
                    SubtractNext(Years, ref year, ref subtracted);
                    if (subtracted)
                    {
                        Days = sequence.GenerateSequence(parser.Day, 1, DateTime.DaysInMonth(year, month));
                    }

                    datetime = new DateTime(year: year, month: month, day: day,
                                    hour: hour, minute: minute, second: second,
                                    millisecond: millisecond);
                    diff = Math.Abs((t1 - datetime).TotalMilliseconds);
                    if (diff < closestDiff)
                    {
                        closestDiff = diff;
                        closestTime = datetime;
                        subtracted = false;
                    }
                    else
                    {
                        closestFound = true;
                        datetime = closestTime;
                    }
                }
            }

            if (!findEqual && !findNext)
            {
                closestDiff = Math.Abs((t1 - datetime).TotalMilliseconds);
                var diff = Math.Abs((t1 - datetime).TotalMilliseconds);
                bool subtracted = false;
                var changeDay = true;
                bool closestFound = false;
                DateTime closestTime = datetime;
                while (/*!closestFound || */!subtracted || !changeDay)
                {
                    SubtractNext(Milliseconds, ref millisecond, ref subtracted);
                    SubtractNext(Seconds, ref second, ref subtracted);
                    SubtractNext(Minutes, ref minute, ref subtracted);
                    SubtractNext(Hours, ref hour, ref subtracted);
                    SubtractNext(Days, ref day, ref subtracted);
                    if (!changeDay & subtracted)
                    {
                        SubtractNext(Days, ref day, ref changeDay);
                        subtracted = changeDay;
                    }
                    SubtractNext(Months, ref month, ref subtracted);
                    SubtractNext(Years, ref year, ref subtracted);
                    if (subtracted)
                    {
                        Days = sequence.GenerateSequence(parser.Day, 1, DateTime.DaysInMonth(year, month));
                    }

                    datetime = new DateTime(year: year, month: month, day: day,
                                    hour: hour, minute: minute, second: second,
                                    millisecond: millisecond);
                    diff = Math.Abs((t1 - datetime).TotalMilliseconds);
                    //if (diff < closestDiff)
                    //{
                    //    closestDiff = diff;
                    //    closestTime = datetime;
                    //    subtracted = false;
                    //}
                    //else
                    //{
                    //    closestFound = true;
                    //    datetime = closestTime;
                    //}
                    //if (diff < closestDiff)
                    //{
                    //    closestDiff = diff;
                    //}
                    if (DaysOfWeek.Count != 0)
                    {
                        if (DaysOfWeek.Contains((int)datetime.DayOfWeek))
                        {
                            changeDay = true;
                        }
                        else
                        {
                            changeDay = false;
                        }
                    }
                }
            }

            result = datetime;
            

            return result;
        }

        private void AddNext(List<int> sourceValues, ref int value, ref bool added)
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
            }
        }

        private void SubtractNext(List<int> sourceValues, ref int value, ref bool subtracted)
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
            }
        }

        private DateTime GetNearestDateTime(DateTime t1, DateTime currentResult, bool findEqual, bool findNext)
        {
            double closestDateTime = double.MaxValue;
            foreach (var hour in Hours)
            {
                foreach (var minute in Minutes)
                {
                    foreach (var second in Seconds)
                    {
                        if (Milliseconds.Count == 0)
                        {
                            Milliseconds.Add(0);
                        }
                        foreach (var millisecond in Milliseconds)
                        {
                            var datetime = new DateTime(year: currentResult.Year, month: currentResult.Month, day: currentResult.Day,
                                hour: hour, minute: minute, second: second, millisecond: millisecond);
                            var diff = Math.Abs((datetime - t1).TotalMilliseconds);
                            if ((diff < closestDateTime && (diff != 0 || findEqual)) || (findNext && !findEqual))
                            {
                                closestDateTime = diff;
                                currentResult = datetime;
                                if (currentResult > t1)
                                {
                                    findNext = false;
                                }
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

        public DateTime NearestPrevEvent(DateTime t1)
        {
            DateTime result = FindDate(t1, true, false);
            return result;
        }

        public DateTime NextEvent(DateTime t1)
        {
            DateTime result = FindDate(t1, false, true);
            return result;
        }

        public DateTime PrevEvent(DateTime t1)
        {
            DateTime result = FindDate(t1, false, false);
            return result;
        }

    }

}
