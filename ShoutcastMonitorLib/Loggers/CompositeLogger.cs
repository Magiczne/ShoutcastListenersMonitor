using System.Collections.Generic;
using ShoutcastMonitorLib.Abstraction;

namespace ShoutcastMonitorLib.Loggers
{
    public class CompositeLogger : IDataLogger
    {
        /// <summary>
        ///     Data loggers
        /// </summary>
        private readonly List<IDataLogger> _innerLoggers;

        /// <summary>
        ///     Create new instance of CompositeLogger
        /// </summary>
        public CompositeLogger()
        {
            _innerLoggers = new List<IDataLogger>();
        }

        /// <summary>
        ///     Add logger
        /// </summary>
        /// <param name="logger"></param>
        public void Add(IDataLogger logger)
        {
            _innerLoggers.Add(logger);
        }

        /// <summary>
        ///     Remove logger
        /// </summary>
        /// <param name="logger"></param>
        public void Remove(IDataLogger logger)
        {
            _innerLoggers.Remove(logger);
        }

        /// <inheritdoc cref="IDataLogger"/>
        public void Log(int listeners)
        {
            foreach (var logger in _innerLoggers)
            {
                logger.Log(listeners);
            }
        }

        /// <inheritdoc cref="IDataLogger"/>
        public void Error(string message)
        {
            foreach (var logger in _innerLoggers)
            {
                logger.Error(message);
            }
        }
    }
}