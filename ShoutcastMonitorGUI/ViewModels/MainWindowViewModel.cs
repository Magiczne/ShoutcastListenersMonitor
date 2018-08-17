using Caliburn.Micro;
using ShoutcastMonitorGUI.Loggers;
using ShoutcastMonitorGUI.Models;
using ShoutcastMonitorGUI.Properties;
using ShoutcastMonitorGUI.Util.Collections;
using ShoutcastMonitorLib.Abstraction;
using ShoutcastMonitorLib.Loggers;
using ShoutcastMonitorLib.Receivers;

namespace ShoutcastMonitorGUI.ViewModels
{
    public class MainWindowViewModel : Screen, IHandle<ListenerData>
    {
        #region Fields

        /// <summary>
        ///     Data receiver
        /// </summary>
        private readonly IReceiver _dataReceiver;

        #endregion

        public MainWindowViewModel(IEventAggregator eventAggregator)
        {
            eventAggregator.Subscribe(this);

            var logger = new CompositeLogger();
            logger.Add(new TextFileLogger());
            logger.Add(new EventBasedLogger(eventAggregator));

            _dataReceiver = new SimpleReceiver(StatsUrl, TimeInterval, logger);
        }

        #region IHandle

        /// <inheritdoc />
        public void Handle(ListenerData data)
        {
            ListenersData.Push(data);
        }

        #endregion

        public void Monitor()
        {
            IsMonitoring = !IsMonitoring;

            if (IsMonitoring)
            {
                ListenersData.Clear();

                // To have last 30 minutes of data visible in the data grid
                ListenersData.MaxSize = 1800 / TimeInterval;

                _dataReceiver.TimeInterval = TimeInterval;
                _dataReceiver.StatsUrl = StatsUrl;

                _dataReceiver.Start();
            }
            else
            {
                _dataReceiver.Stop();
            }
        }

        #region Properties

        /// <summary>
        ///     Stats URL to be getting data from
        /// </summary>
        public string StatsUrl { get; set; }

        /// <summary>
        ///     Data acquistition time interval
        /// </summary>
        public int TimeInterval { get; set; } = 120;

        /// <summary>
        ///     Is monitoring currently active
        /// </summary>
        public bool IsMonitoring { get; private set; }

        /// <summary>
        ///     Text on button
        /// </summary>
        public string ActionButtonText => IsMonitoring ? Strings.StopMonitoring : Strings.Monitor;

        /// <summary>
        ///     Listeners data
        /// </summary>
        public LimitedStack<ListenerData> ListenersData { get; } = new LimitedStack<ListenerData>(1);

        #endregion
    }
}