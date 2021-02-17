using System;
using System.Collections.Generic;
using System.Text;

namespace Scheduler
{
    public interface IDateTimeParser
    {
        string Year { get; set; }
        string Month { get; set; }
        string Day { get; set; }
        string DayOfWeek { get; set; }
        string Hour { get; set; }
        string Minute { get; set; }
        string Second { get; set; }
        string Milisecond { get; set; }
        void Parse(string data);
    }
}
