using System;
using System.Collections.Generic;
using System.Text;

namespace Scheduler
{
    public class SequenceCreator
    {
        List<int> Years;
        List<int> Months;
        List<int> Days;
        List<int> DaysOfWeek;
        List<int> Hours;
        List<int> Minutes;
        List<int> Seconds;
        List<int> Miliseconds;

        public SequenceCreator()
        {
            Years = new List<int>();
            Months = new List<int>();
            Days = new List<int>();
            DaysOfWeek = new List<int>();
            Hours = new List<int>();
            Minutes = new List<int>();
            Seconds = new List<int>();
            Miliseconds = new List<int>();
        }
        
        public SequenceCreator(IDateTimeParser parser) : base()
        {
            
        }
    }
}
