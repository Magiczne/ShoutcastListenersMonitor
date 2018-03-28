using System;
using System.IO;
using ShoutcastMonitor.Abstraction;

namespace ShoutcastMonitor.Loggers
{
    internal class TextFileLogger : IDataLogger
    {
        /// <summary>
        ///     Filename
        /// </summary>
        private static string Filename => $"text-{DateTime.Now:yyyy-MM-dd}.txt";

        /// <inheritdoc cref="IDataLogger"/>
        public void Log(int listeners)
        {
            File.AppendAllLines(Filename, new []
            {
                $"{DateTime.Now:HH:mm:ss}\t{listeners}"
            });
        }

        /// <inheritdoc cref="IDataLogger"/>
        public void Error(string message)
        {
            File.AppendAllLines(Filename, new []
            {
                $"{DateTime.Now:HH:mm:ss}\t{message}"
            });
        }
    }
}