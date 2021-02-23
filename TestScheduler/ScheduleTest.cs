using Scheduler;
using System;
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

        [Fact]
        public void ClosestDateTimeTest()
        {
            var schedule = new Schedule("2021.1,2,04-12.20,24-27 2-6 3-10:01-55:23.10-945");
            var result = schedule.NearestEvent(new DateTime(year: 2021, month: 2, day: 20,
                hour: 3, minute: 4, second: 23, millisecond: 944));
            var expected = new DateTime(year: 2021, month: 2, day: 20,
                hour: 3, minute: 4, second: 23, millisecond: 944);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void T1()
        {
            var schedule = new Schedule("2019-2021.1,2,06-12.20,24-27 2-6 3-10:01-55:23.10-945");
            var result = schedule.NearestEvent(new DateTime(year: 2021, month: 2, day: 20,
                hour: 3, minute: 4, second: 23, millisecond: 934));
            var expected = new DateTime(year: 2021, month: 2, day: 20,
                hour: 3, minute: 4, second: 23, millisecond: 934);
            Assert.Equal(expected, result);
        }
        [Fact]
        public void T2()
        {
            var schedule = new Schedule("2021.1,2,04-12.20,24-27 2-6 3-10:01-55:23.10-945");
            var result = schedule.NearestEvent(new DateTime(year: 2021, month: 2, day: 20,
                hour: 3, minute: 4, second: 23, millisecond: 933));
            var expected = new DateTime(year: 2021, month: 2, day: 20,
                hour: 3, minute: 4, second: 23, millisecond: 933);
            Assert.Equal(expected, result);
        }
        [Fact]
        public void T3()
        {
            var schedule = new Schedule("2021.1,2,04-12.20,24-27 2-6 3-10:01-55:23.10-945");
            var result = schedule.NearestEvent(new DateTime(year: 2021, month: 2, day: 20,
                hour: 3, minute: 4, second: 23, millisecond: 932));
            var expected = new DateTime(year: 2021, month: 2, day: 20,
                hour: 3, minute: 4, second: 23, millisecond: 932);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void DayOfWeekTest()
        {
            var schedule = new Schedule("2021.1,2,07-12.21,24-27 2-4 ");
            var result = schedule.NearestEvent(new DateTime(year: 2021, month: 4, day: 20));
            var expected = new DateTime(year: 2021, month: 2, day: 25);
            Assert.Equal(expected, result);
        }
    }
}
