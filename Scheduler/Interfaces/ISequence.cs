using System.Collections.Generic;

namespace Scheduler.Interfaces
{
    public interface ISequence
    {
        List<int> GenerateSequence(string expression, int minValue, int maxValue);
    }
}
