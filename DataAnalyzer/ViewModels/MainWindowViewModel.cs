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

        private readonly Dictionary<DateTime, Dictionary<TimeSpan, int>> _rawData =
            new Dictionary<DateTime, Dictionary<TimeSpan, int>>();

        #endregion

        #region Properties

        /// <summary>
        ///     Analyzed data to be displayed in tabular form
        /// </summary>
        public Dictionary<DateTime, AnalyzedData> AnalyzedData { get; set; } = new Dictionary<DateTime, AnalyzedData>();

        /// <summary>
        ///     Data for the average listeners plot
        /// </summary>
        public BindableCollection<DataPoint> AverageListenersData { get; set; } = new BindableCollection<DataPoint>();

        /// <summary>
        ///     Data for the time without listeners plot
        /// </summary>
        public BindableCollection<DataPoint> TimeWithoutListenersData { get; set; } = new BindableCollection<DataPoint>();

        /// <summary>
        ///     Data for the listeners peak plot
        /// </summary>
        public BindableCollection<DataPoint> ListenersPeakData { get; set; } = new BindableCollection<DataPoint>();

        /// <summary>
        ///     Directory where data files are
        /// </summary>
        public string DataDirectory { get; set; } = AppDomain.CurrentDomain.BaseDirectory + "data";

        /// <summary>
        ///     File prefix before date
        /// </summary>
        public string FilePrefix { get; set; } = "text-";

        #endregion

        public void Analyze()
        {
            _rawData.Clear();
            AnalyzedData.Clear();

            LoadData();

            foreach (var dayData in _rawData)
            {
                var analyzed = new AnalyzedData
                {
                    Average = dayData.Value.Average(entry => entry.Value),
                    ListenersPeak = dayData.Value.Max(entry => entry.Value),
                    TimeWithoutListeners = dayData.Value.Count(entry => entry.Value == 0) / (double) dayData.Value.Count,
                };

                AnalyzedData.Add(dayData.Key, analyzed);
            }

            AverageListenersData.Clear();
            TimeWithoutListenersData.Clear();
            ListenersPeakData.Clear();

            AverageListenersData.AddRange(AnalyzedData.Select(entry => new DataPoint(DateTimeAxis.ToDouble(entry.Key), entry.Value.Average)));
            TimeWithoutListenersData.AddRange(AnalyzedData.Select(entry => new DataPoint(DateTimeAxis.ToDouble(entry.Key), entry.Value.TimeWithoutListeners * 100)));
            ListenersPeakData.AddRange(AnalyzedData.Select(entry => new DataPoint(DateTimeAxis.ToDouble(entry.Key), entry.Value.ListenersPeak)));
        }

        private void LoadData()
        {
            foreach (var file in Directory.GetFiles(DataDirectory))
            {
                var fileName = Path.GetFileNameWithoutExtension(file);
                var date = DateTime.Parse(fileName.Substring(FilePrefix.Length));

                _rawData.Add(date, ParseDataFile(file));
            }
        }

        private Dictionary<TimeSpan, int> ParseDataFile(string fullPath)
        {
            return File.ReadAllLines(fullPath).Select(i =>
            {
                var data = i.Split('\t');
                return new KeyValuePair<TimeSpan, int>(TimeSpan.Parse(data[0]), int.Parse(data[1]));
            }).ToDictionary(i => i.Key, i => i.Value);
        }
    }
}