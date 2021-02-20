using System;
using System.Collections.Generic;
using System.Text;

namespace Scheduler.Interfaces
{
    public interface ISequence
    {
        List<int> GenerateSequence(string expression, int minValue, int maxValue);
    }
}
