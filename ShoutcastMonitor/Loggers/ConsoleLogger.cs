using System;
using ShoutcastMonitor.Abstraction;

namespace ShoutcastMonitor.Loggers
{
    internal class ConsoleLogger : IDataLogger
    {
        /// <inheritdoc cref="IDataLogger"/>
        public void Log(int listeners)
        {
            Console.WriteLine($"{DateTime.Now:HH:mm:ss}\t{listeners}");
        }

        /// <inheritdoc cref="IDataLogger"/>
        public void Error(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{DateTime.Now:HH:mm:ss}\t{message}");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}