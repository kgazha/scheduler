using Scheduler;
using Scheduler.Interfaces;
using Xunit;

namespace TestScheduler
{
    public class StringParserTest
    {
        IParser parser = new StringParser();

        [Fact]
        public void ParseDateTest()
        {
            var pattern = @"(((\d{4})?\*?,?-?)*)\.(((\d{1,2})?\*?,?-?)*)\.(((\d{1,2})?\*?,?-?)*)";
            var date = "2000-2010,2014,2015.02,3-9.16";
            var year = parser.Parse(date, pattern, 1);
            var month = parser.Parse(date, pattern, 4);
            var day = parser.Parse(date, pattern, 7);
            Assert.Equal("2000-2010,2014,2015", year);
            Assert.Equal("02,3-9", month);
            Assert.Equal("16", day);
        }

        [Fact]
        public void ParseDayOfWeekTest()
        {
            var pattern = @"\s((\d{1},?-?)+)\s";
            var date = "2021.02-5.17 3 12:00:00";
            var dayOfWeek = parser.Parse(date, pattern, 1);
            Assert.Equal("3", dayOfWeek);
        }

        [Fact]
        public void ParseTimeTest()
        {
            var pattern = @"(\S+):(\S+):(((\d{1,2})?\*?,?-?\/?)*)\.?(\S+)?";
            var date = "2021.02-5.17 3 12:30/3:40.334";
            var hour = parser.Parse(date, pattern, 1);
            var minute = parser.Parse(date, pattern, 2);
            var second = parser.Parse(date, pattern, 3);
            var milisecond = parser.Parse(date, pattern, 6);
            Assert.Equal("12", hour);
            Assert.Equal("30/3", minute);
            Assert.Equal("40", second);
            Assert.Equal("334", milisecond);
        }
    }
}
