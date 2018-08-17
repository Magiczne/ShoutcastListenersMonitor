using System;
using System.IO;
using ShoutcastMonitorLib.Abstraction;

namespace ShoutcastMonitorLib.Loggers
{
    public class TextFileLogger : IDataLogger
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