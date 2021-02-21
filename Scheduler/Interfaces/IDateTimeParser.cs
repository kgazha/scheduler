using System;
using System.Collections.Generic;
using System.Text;

namespace Scheduler.Interfaces
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
        string Millisecond { get; set; }
        void Parse(string input);
    }
}
