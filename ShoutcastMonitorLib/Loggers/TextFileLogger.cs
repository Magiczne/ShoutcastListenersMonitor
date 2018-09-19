using System;
using System.IO;
using ShoutcastMonitorLib.Abstraction;

namespace ShoutcastMonitorLib.Loggers
{
    public class TextFileLogger : IDataLogger
    {
        /// <summary>
        ///     Data directory
        /// </summary>
        private const string DataDirectory = "logs/";

        /// <summary>
        ///     Filename
        /// </summary>
        private static string Filename => $"{DataDirectory}/log-{DateTime.Now:yyyy-MM-dd}.txt";

        /// <inheritdoc cref="IDataLogger"/>
        public void Log(int listeners)
        {
            if (!Directory.Exists(DataDirectory))
            {
                Directory.CreateDirectory(DataDirectory);
            }

            File.AppendAllLines(Filename, new []
            {
                $"{DateTime.Now:HH:mm:ss}\t{listeners}"
            });
        }

        /// <inheritdoc cref="IDataLogger"/>
        public void Error(string message)
        {
            if (!Directory.Exists(DataDirectory))
            {
                Directory.CreateDirectory(DataDirectory);
            }

            File.AppendAllLines(Filename, new []
            {
                $"{DateTime.Now:HH:mm:ss}\t{message}"
            });
        }
    }
}