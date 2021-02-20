﻿using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Scheduler;
using Scheduler.Interfaces;

namespace TestScheduler
{
    public class DateTimeParserTest
    {
        [Fact]
        public void ParseTest()
        {
            IDateTimeParser dateTimeParser = new DateTimeParser(new StringParser());
            string scheduleString = "*.*.* * *:*:*.*";
            dateTimeParser.Parse(scheduleString);
            Assert.Equal("*", dateTimeParser.Year);
            Assert.Equal("*", dateTimeParser.Month);
            Assert.Equal("*", dateTimeParser.Day);
            Assert.Equal("*", dateTimeParser.DayOfWeek);
            Assert.Equal("*", dateTimeParser.Hour);
            Assert.Equal("*", dateTimeParser.Minute);
            Assert.Equal("*", dateTimeParser.Second);
            Assert.Equal("*", dateTimeParser.Milisecond);
        }
    }
}