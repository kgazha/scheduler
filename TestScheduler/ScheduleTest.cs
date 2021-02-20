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
            var result = schedule.NearestEvent(DateTime.Now);
            var expected = new DateTime(year: 2021, month: 2, day: 20);
            Assert.Equal(expected, result);

            schedule = new Schedule("2019-2020.10-12.20");
            result = schedule.NearestEvent(DateTime.Now);
            expected = new DateTime(year: 2020, month: 12, day: 20);
            Assert.Equal(expected, result);
        }
    }
}
