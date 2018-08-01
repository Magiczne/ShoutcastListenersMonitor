using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Caliburn.Micro;
using DataAnalyzer.Models;

namespace DataAnalyzer.ViewModels
{
    public class MainWindowViewModel : Screen
    {
        private readonly Dictionary<DateTime, Dictionary<TimeSpan, int>> _rawData =
            new Dictionary<DateTime, Dictionary<TimeSpan, int>>();

        public Dictionary<DateTime, AnalyzedData> AnalyzedData { get; set; } = new Dictionary<DateTime, AnalyzedData>();

        /// <summary>
        ///     Directory where data files are
        /// </summary>
        public string DataDirectory { get; set; } = AppDomain.CurrentDomain.BaseDirectory + "data";

        /// <summary>
        ///     File prefix before date
        /// </summary>
        public string FilePrefix { get; set; } = "text-";

        public void Analyze()
        {
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