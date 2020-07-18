using System;
using System.Threading.Tasks;

namespace Map.Modules.Teltonika
{
    public interface ILogger
    {
        public Task LogAsync(string str, ConsoleColor foreColor = ConsoleColor.Gray, ConsoleColor backColor = ConsoleColor.Gray);
    }
}