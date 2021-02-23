using Scheduler.Interfaces;
using System;
using System.Collections.Generic;

namespace Scheduler
{
    public class SequenceCreator : ISequence
    {
        public List<int> GenerateSequence(string expression, int minValue, int maxValue)
        {
            List<int> sequence = new List<int>();

            foreach (var item in expression.Split(','))
            {
                int step = 1;
                if (item.Contains('/'))
                {
                    step = Convert.ToInt32(item.Split('/')[1]);
                }
                if (item.Contains('-'))
                {
                    sequence.AddRange(GetSequenceFromRange(item, step));
                }
                else if (item.Contains('*'))
                {
                    int startValue = minValue;
                    while (startValue <= maxValue)
                    {
                        sequence.Add(startValue);
                        startValue += step;
                    }
                }
                else if (item.Contains("32"))
                {
                    sequence.Add(maxValue);
                }
                else if (item != "")
                {
                    sequence.Add(Convert.ToInt32(item));
                }
            }
            return sequence;
        }

        private List<int> GetSequenceFromRange(string range, int step)
        {
            List<int> sequence = new List<int>();
            int x = Convert.ToInt32(range.Split('-')[0]);
            int y = Convert.ToInt32(range.Split('-')[1].Split('/')[0]);

            while (x <= y)
            {
                sequence.Add(x);
                x += step;
            }
            return sequence;
        }
    }
}
