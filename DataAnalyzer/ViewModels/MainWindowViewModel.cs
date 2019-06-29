using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Caliburn.Micro;
using DataAnalyzer.Models;
using OxyPlot;
using OxyPlot.Axes;

namespace DataAnalyzer.ViewModels
{
    public class MainWindowViewModel : Screen
    {
        #region Fields

        /// <summary>
        ///     Raw data from files
        /// </summary>
        private readonly Dictionary<DateTime, Dictionary<TimeSpan, int>> _rawData =
            new Dictionary<DateTime, Dictionary<TimeSpan, int>>();

        #endregion

        /// <summary>
        ///     First load then analyze data
        /// </summary>
        public void Analyze()
        {
            _rawData.Clear();
            AnalyzedData.Clear();

            LoadData();

            foreach (var dayData in _rawData)
            {
                var analyzed = new AnalyzedData
                {
                    Date = dayData.Key,
                    Average = dayData.Value.Average(entry => entry.Value),
                    ListenersPeak = dayData.Value.Max(entry => entry.Value),
                    TimeWithoutListeners = dayData.Value.Count(entry => entry.Value == 0) / (double) dayData.Value.Count
                };

                AnalyzedData.Add(analyzed);
            }

            NotifyOfPropertyChange(nameof(AverageListenersPlotMaxScale));
            NotifyOfPropertyChange(nameof(TimeWithoutListenersPlotMaxScale));
            NotifyOfPropertyChange(nameof(ListenersPeakPlotMaxScale));

            NotifyOfPropertyChange(nameof(AverageListeners));
            NotifyOfPropertyChange(nameof(TimeWithoutListeners));
            NotifyOfPropertyChange(nameof(ListenersPeak));

            AverageListenersData.Clear();
            TimeWithoutListenersData.Clear();
            ListenersPeakData.Clear();

            AverageListenersData.AddRange(AnalyzedData.Select(entry =>
                new DataPoint(DateTimeAxis.ToDouble(entry.Date), entry.Average)));
            TimeWithoutListenersData.AddRange(AnalyzedData.Select(entry =>
                new DataPoint(DateTimeAxis.ToDouble(entry.Date), entry.TimeWithoutListeners * 100)));
            ListenersPeakData.AddRange(AnalyzedData.Select(entry =>
                new DataPoint(DateTimeAxis.ToDouble(entry.Date), entry.ListenersPeak)));

            IsAnalyzed = true;
        }

        /// <summary>
        ///     Load data from all files
        /// </summary>
        private void LoadData()
        {
            foreach (var file in Directory.GetFiles(DataDirectory))
            {
                var fileName = Path.GetFileNameWithoutExtension(file);
                var date = DateTime.Parse(fileName.Substring(FilePrefix.Length));

                _rawData.Add(date, ParseDataFile(file));
            }

            Dates.AddRange(_rawData.Keys);
        }

        /// <summary>
        ///     Parse data file
        /// </summary>
        /// <param name="fullPath">Path to the file</param>
        /// <returns>Dictionary containing time and listeners at given time</returns>
        private Dictionary<TimeSpan, int> ParseDataFile(string fullPath)
        {
            var dict = new Dictionary<TimeSpan, int>();

            foreach (var line in File.ReadAllLines(fullPath))
            {
                var data = line.Split('\t');

                try
                {
                    var timespan = TimeSpan.Parse(data[0]);
                    var listeners = int.Parse(data[1]);

                    dict.Add(timespan, listeners);
                }
                catch (Exception)
                {
                    // Just continue to next iteration.
                    // Do not add any data to the return dictionary
                }
            }

            return dict;
        }

        #region Properties

        #region Global data

        /// <summary>
        ///     Average listeners count for all selected data
        /// </summary>
        public double AverageListeners => AnalyzedData.Average(entry => entry.Average);

        /// <summary>
        ///     Sum of time without listeners
        /// </summary>
        public double TimeWithoutListeners => AnalyzedData.Average(entry => entry.TimeWithoutListeners) * 100;

        /// <summary>
        ///     Global listeners peak
        /// </summary>
        public int ListenersPeak => AnalyzedData.Max(entry => entry.ListenersPeak);

        #endregion

        #region Summary data

        /// <summary>
        ///     Analyzed data in form on dictionary
        /// </summary>
        public BindableCollection<AnalyzedData> AnalyzedData { get; set; } = new BindableCollection<AnalyzedData>();

        /// <summary>
        ///     Data for the average listeners plot
        /// </summary>
        public BindableCollection<DataPoint> AverageListenersData { get; set; } = new BindableCollection<DataPoint>();

        /// <summary>
        ///     Data for the time without listeners plot
        /// </summary>
        public BindableCollection<DataPoint> TimeWithoutListenersData { get; set; } =
            new BindableCollection<DataPoint>();

        /// <summary>
        ///     Data for the listeners peak plot
        /// </summary>
        public BindableCollection<DataPoint> ListenersPeakData { get; set; } = new BindableCollection<DataPoint>();

        /// <summary>
        ///     Average listeners plot maximum scale
        /// </summary>
        public int AverageListenersPlotMaxScale => AnalyzedData.Count > 0
            ? (int) AnalyzedData.Max(data => data.Average) + 3
            : 100;

        /// <summary>
        ///     Time without listeners plot maximum scale
        /// </summary>
        public int TimeWithoutListenersPlotMaxScale => AnalyzedData.Count > 0
            ? (int) AnalyzedData.Max(data => data.TimeWithoutListeners) * 100 + 3
            : 100;

        /// <summary>
        ///     Listeners peak plot maximum scale
        /// </summary>
        public int ListenersPeakPlotMaxScale => AnalyzedData.Count > 0
            ? AnalyzedData.Max(data => data.ListenersPeak) + 3
            : 100;

        #endregion

        #region Daily data

        /// <summary>
        ///     List of dates for data
        /// </summary>
        public BindableCollection<DateTime> Dates { get; set; } = new BindableCollection<DateTime>();

        /// <summary>
        ///     Selected date
        /// </summary>
        public DateTime SelectedDate { get; set; }

        /// <summary>
        ///     Data for the selected day listeners plot
        /// </summary>
        public BindableCollection<DataPoint> SelectedDayData
        {
            get
            {
                if (!_rawData.ContainsKey(SelectedDate)) return new BindableCollection<DataPoint>();

                return new BindableCollection<DataPoint>(_rawData[SelectedDate]
                    .Select(pair => new DataPoint(TimeSpanAxis.ToDouble(pair.Key), pair.Value)).ToList());
            }
        }

        /// <summary>
        ///     Selected day plot maximum scale
        /// </summary>
        public int SelectedDayPlotMaxScale => !_rawData.ContainsKey(SelectedDate)
            ? 100
            : _rawData[SelectedDate].Max(pair => pair.Value) + 3;

        #endregion

        /// <summary>
        ///     Directory where data files are
        /// </summary>
        public string DataDirectory { get; set; } = AppDomain.CurrentDomain.BaseDirectory + "logs";

        /// <summary>
        ///     File prefix before date
        /// </summary>
        public string FilePrefix { get; set; } = "log-";

        /// <summary>
        ///     Determine if analyze has been performed already
        /// </summary>
        public bool IsAnalyzed { get; private set; }

        #endregion
    }
}