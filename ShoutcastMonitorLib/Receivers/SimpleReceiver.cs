using System;
using System.Net;
using System.Timers;
using System.Xml;
using ShoutcastMonitorLib.Abstraction;

namespace ShoutcastMonitorLib.Receivers
{
    public class SimpleReceiver : IReceiver
    {
        /// <summary>
        ///     URL address
        /// </summary>
        private readonly string _url;

        /// <summary>
        ///     Timer instance
        /// </summary>
        private readonly Timer _timer;

        /// <inheritdoc cref="IReceiver"/>
        public IDataLogger Logger { get; set; }

        /// <summary>
        ///     Create instance of SimpleReceiver
        /// </summary>
        /// <param name="url">Stats url to monitor</param>
        /// <param name="interval">Timer interval (in seconds)</param>
        /// <param name="logger">Logger instance</param>
        public SimpleReceiver(string url, int interval, IDataLogger logger)
        {
            _url = url;
            Logger = logger;

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
                xmlString = client.DownloadString(_url);
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

            _timer.Start();
        }

        /// <inheritdoc cref="IReceiver"/>
        public void Stop()
        {
            _timer.Start();
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
                Logger.Error("There was no data to process!");
            }
            catch (FormatException)
            {
                Logger.Error("Number of listeners is not available");
            }
            catch (WebException)
            {
                Logger.Error("Cannot connect to the specified url");
            }
            catch (XmlException)
            {
                Logger.Error("Stats file is not a valid XML");
            }
        }
    }
}