using Scheduler;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace TestScheduler
{
    public class ScheduleTest
    {
        [Fact]
        public void NearestEventTest()
        {
            Schedule schedule = new Schedule();
            var result = schedule.NearestEvent(new DateTime(year: 2021, month: 2, day: 20));
            var expected = new DateTime(year: 2021, month: 2, day: 20);
            Assert.Equal(expected, result);

            schedule = new Schedule("2019-2020.10-12.20");
            result = schedule.NearestEvent(new DateTime(year: 2021, month: 2, day: 20));
            expected = new DateTime(year: 2020, month: 12, day: 20);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ClosestNextDateTest()
        {
            var schedule = new Schedule("2019-2021.10-12.20,24-27");
            var result = schedule.NearestEvent(new DateTime(year: 2021, month: 4, day: 20));
            var expected = new DateTime(year: 2021, month: 10, day: 20);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ClosestPreviousDateTest()
        {
            var schedule = new Schedule("2019-2020.1,2,09-12.20,24-27");
            var result = schedule.NearestEvent(new DateTime(year: 2021, month: 4, day: 20));
            var expected = new DateTime(year: 2020, month: 12, day: 27);
            Assert.Equal(expected, result);
        }
    }
}
