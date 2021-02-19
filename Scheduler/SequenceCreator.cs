using Scheduler.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scheduler
{
    public class SequenceCreator
    {
        string expression;
                
        public SequenceCreator(string expression)
        {
            this.expression = expression;
        }

        public List<int> GenerateSequence(int minValue, int maxValue)
        {
            List<int> values = new List<int>();
            return values;
        }
    }
}
