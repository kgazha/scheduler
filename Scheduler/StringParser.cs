using Scheduler.Interfaces;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Scheduler
{
    public class StringParser : IParser
    {
 

        public string Parse(string input, string pattern, int groupIndex)
        {
            string result = "";
            var match = Regex.Match(input, pattern);
            if (match.Success)
            {
                if (match.Groups.Count >= groupIndex)
                {
                    result = match.Groups[groupIndex].Value;
                }
            }
            return result;
        }

    }
}
