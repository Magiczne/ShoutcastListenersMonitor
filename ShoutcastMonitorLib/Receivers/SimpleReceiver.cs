using System;
using System.Net;
using System.Timers;
using System.Xml;
using ShoutcastMonitorLib.Abstraction;

namespace ShoutcastMonitorLib.Receivers
{
    public class SimpleReceiver : IReceiver
    {
        #region Fields

        /// <summary>
        ///     Timer instance
        /// </summary>
        private readonly Timer _timer;

        #endregion

        #region Properties

        /// <inheritdoc cref="IReceiver"/>
        public IDataLogger Logger { get; set; }

        /// <inheritdoc cref="IReceiver"/>
        public int TimeInterval { get; set; }

        /// <inheritdoc cref="IReceiver"/>
        public string StatsUrl { get; set; }

        #endregion

        /// <summary>
        ///     Create instance of SimpleReceiver
        /// </summary>
        /// <param name="statsUrl">Stats statsUrl to monitor</param>
        /// <param name="interval">Timer interval (in seconds)</param>
        /// <param name="logger">Logger instance</param>
        public SimpleReceiver(string statsUrl, int interval, IDataLogger logger)
        {
            StatsUrl = statsUrl;
            Logger = logger;
            TimeInterval = interval;

            _timer = new Timer(1000 * interval);
            _timer.Elapsed += (sender, args) =>
            {
                ProcessData();
            };
        }

        /// <inheritdoc cref="IReceiver"/>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="FormatException"></exception>
        /// <exception cref="WebException"></exception>
        /// <exception cref="XmlException"></exception>
        public int ReceiveListeners()
        {
            string xmlString;

            using (var client = new WebClient())
            {
                xmlString = client.DownloadString(StatsUrl);
            }

            var document = new XmlDocument();
            document.LoadXml(xmlString);

            var listeners = document.GetElementsByTagName("CURRENTLISTENERS")[0];

            return int.Parse(listeners.InnerText);
        }

        /// <inheritdoc cref="IReceiver"/>
        public void Start()
        {
           ProcessData();

            _timer.Interval = TimeInterval * 1000;
            _timer.Start();
        }

        /// <inheritdoc cref="IReceiver"/>
        public void Stop()
        {
            _timer.Stop();
        }

        /// <summary>
        ///     Process data
        /// </summary>
        private void ProcessData()
        {
            try
            {
                var listeners = ReceiveListeners();
                Logger.Log(listeners);
            }
            catch (ArgumentNullException)
            {
                Logger.Error(Properties.Errors.NoData);
            }
            catch (FormatException)
            {
                Logger.Error(Properties.Errors.DataNotAvailable);
            }
            catch (WebException)
            {
                Logger.Error(Properties.Errors.ConnectionError);
            }
            catch (XmlException)
            {
                Logger.Error(Properties.Errors.NotValidXml);
            }
        }
    }
}