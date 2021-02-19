using System;
using System.Collections.Generic;
using System.Text;

namespace Scheduler.Interfaces
{
    public interface IParser
    {
        string Parse(string input, string pattern, int groupIndex);
    }
}
