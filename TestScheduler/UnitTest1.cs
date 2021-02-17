using Scheduler;
using System;
using Xunit;

namespace TestScheduler
{
    public class UnitTest1
    {
        IDateTimeParser parser = new ScheduleParser();

        [Fact]
        public void Test1()
        {
            //parser
            parser.Parse("2000-2010,2014,2015.02,3-9.16");
            Assert.Equal("2000-2010,2014,2015", parser.Year);

            parser.Parse("123");
            Assert.Equal("", parser.Year);

            parser.Parse("11-12:10,11-17,21/3:03-34/2.323");
            Assert.Equal("323", parser.Milisecond);

            parser.Parse("12:03:44-45");
            Assert.Equal("", parser.Milisecond);
        }
    }
}
