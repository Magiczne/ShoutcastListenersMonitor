using Caliburn.Micro;
using ShoutcastMonitorGUI.Loggers;
using ShoutcastMonitorGUI.Models;
using ShoutcastMonitorLib.Abstraction;
using ShoutcastMonitorLib.Loggers;
using ShoutcastMonitorLib.Receivers;

namespace ShoutcastMonitorGUI.ViewModels
{
    public class MainWindowViewModel : Screen, IHandle<ListenerData>
    {
        #region Fields

        /// <summary>
        ///     Data logger
        /// </summary>
        private readonly IDataLogger _dataLogger;

        /// <summary>
        ///     Data receiver
        /// </summary>
        private readonly IReceiver _dataReceiver;

        #endregion

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
        public string ActionButtonText => IsMonitoring ? Properties.Strings.StopMonitoring : Properties.Strings.Monitor;

        /// <summary>
        ///     Listeners data
        /// </summary>
        public BindableCollection<ListenerData> ListenersData { get; } = new BindableCollection<ListenerData>();

        #endregion

        public MainWindowViewModel(IEventAggregator eventAggregator)
        {
            eventAggregator.Subscribe(this);

            var logger = new CompositeLogger();
            logger.Add(new TextFileLogger());
            logger.Add(new EventBasedLogger(eventAggregator));

            _dataLogger = logger;
            _dataReceiver = new SimpleReceiver(StatsUrl, TimeInterval, _dataLogger);
        }

        public void Monitor()
        {
            IsMonitoring = !IsMonitoring;

            if (IsMonitoring)
            {
                _dataReceiver.TimeInterval = TimeInterval;
                _dataReceiver.StatsUrl = StatsUrl;

                _dataReceiver.Start();
            }
            else
            {
                _dataReceiver.Stop();
            }
        }

        #region IHandle

        /// <inheritdoc />
        public void Handle(ListenerData data)
        {
            ListenersData.Add(data);
        }

        #endregion
    }
}