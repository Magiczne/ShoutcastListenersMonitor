using System;
using Caliburn.Micro;
using ShoutcastMonitorGUI.Models;
using ShoutcastMonitorLib.Abstraction;

namespace ShoutcastMonitorGUI.Loggers
{
    public class EventBasedLogger : IDataLogger
    {
        #region Fields

        /// <summary>
        ///     Event aggregator instance
        /// </summary>
        private readonly IEventAggregator _eventAggregator;

        #endregion

        public EventBasedLogger(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        /// <inheritdoc cref="IDataLogger"/>
        public void Log(int listeners)
        {
            _eventAggregator.PublishOnUIThread(new ListenerData
            {
                Time = DateTime.Now,
                Listeners = listeners
            });
        }

        /// <inheritdoc cref="IDataLogger"/>
        public void Error(string message)
        {
            _eventAggregator.PublishOnUIThread(new ListenerData
            {
                Time = DateTime.Now,
                Listeners = -1,
                Message = message
            });
        }
    }
}