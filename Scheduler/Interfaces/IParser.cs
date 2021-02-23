namespace Scheduler.Interfaces
{
    public interface IParser
    {
        string Parse(string input, string pattern, int groupIndex);
    }
}
