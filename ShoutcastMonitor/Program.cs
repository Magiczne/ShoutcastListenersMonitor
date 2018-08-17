using System;
using ShoutcastMonitorLib.Loggers;
using ShoutcastMonitorLib.Receivers;

namespace ShoutcastMonitor
{
    internal class Program
    {
        /// <summary>
        ///     Interval in seconds
        /// </summary>
        private const int Interval = 120;

        private static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("First parameter should be url address of the stream stats");
                return;
            }

            var url = args[0];

            var logger = new CompositeLogger();
            logger.Add(new ConsoleLogger());
            logger.Add(new TextFileLogger());

            new SimpleReceiver(url, Interval, logger).Start();

            Console.ReadKey();
        }
    }
}