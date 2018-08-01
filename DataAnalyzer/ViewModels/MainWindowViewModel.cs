using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Caliburn.Micro;
using DataAnalyzer.Models;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace DataAnalyzer.ViewModels
{
    public class MainWindowViewModel : Screen
    {
        private readonly Dictionary<DateTime, Dictionary<TimeSpan, int>> _rawData =
            new Dictionary<DateTime, Dictionary<TimeSpan, int>>();

        /// <summary>
        ///     Analyzed data to be displayed in tabular form
        /// </summary>
        public Dictionary<DateTime, AnalyzedData> AnalyzedData { get; set; } = new Dictionary<DateTime, AnalyzedData>();

        /// <summary>
        ///     Directory where data files are
        /// </summary>
        public string DataDirectory { get; set; } = AppDomain.CurrentDomain.BaseDirectory + "data";

        /// <summary>
        ///     File prefix before date
        /// </summary>
        public string FilePrefix { get; set; } = "text-";

        #region Plots data

        private readonly LineSeries _averagePlotSeries = new LineSeries();

        private readonly StairStepSeries _withoutListenerSeries = new StairStepSeries();

        private readonly LineSeries _listenersPeakSeries = new LineSeries();

        #endregion

        #region Plots models

        public PlotModel AveragePlotModel { get; set; }

        public PlotModel WithoutListenersPlotModel { get; set; }

        public PlotModel ListenersPeakPlotModel { get; set; }

        #endregion

        public MainWindowViewModel()
        {
            AveragePlotModel = new PlotModel
            {
                Axes =
                {
                    new DateTimeAxis
                    {
                        Position = AxisPosition.Bottom,
                        StringFormat = "dd/MM/yyyy",
                        Title = "Date",
                        MajorGridlineStyle = LineStyle.Solid,
                        MinorGridlineStyle = LineStyle.None
                    },
                    new LinearAxis
                    {
                        Position = AxisPosition.Left,
                        Title = "Average listeners",
                        Minimum = 0,
                        MajorGridlineStyle = LineStyle.Solid,
                        MinorGridlineStyle = LineStyle.Dash
                    }
                },
                Series =
                {
                    _averagePlotSeries
                }
            };

            WithoutListenersPlotModel = new PlotModel
            {
                Axes =
                {
                    new DateTimeAxis
                    {
                        Position = AxisPosition.Bottom,
                        StringFormat = "dd/MM/yyyy",
                        Title = "Date",
                        MajorGridlineStyle = LineStyle.Solid,
                        MinorGridlineStyle = LineStyle.None
                    },
                    new LinearAxis
                    {
                        Position = AxisPosition.Left,
                        Title = "Time without listeners (%)",
                        Minimum = 0, Maximum = 100,
                        MajorGridlineStyle = LineStyle.Solid,
                        MinorGridlineStyle = LineStyle.Dash
                    }
                },
                Series =
                {
                    _withoutListenerSeries
                }
            };

            ListenersPeakPlotModel = new PlotModel
            {
                Axes =
                {
                    new DateTimeAxis
                    {
                        Position = AxisPosition.Bottom,
                        StringFormat = "dd/MM/yyyy",
                        Title = "Date",
                        MajorGridlineStyle = LineStyle.Solid,
                        MinorGridlineStyle = LineStyle.None
                    },
                    new LinearAxis
                    {
                        Position = AxisPosition.Left,
                        Title = "Listeners peak",
                        Minimum = 0, Maximum = 100,
                        MajorGridlineStyle = LineStyle.Solid,
                        MinorGridlineStyle = LineStyle.Dash
                    }
                },
                Series =
                {
                    _listenersPeakSeries
                }
            };
        }

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

            _averagePlotSeries.Points.Clear();
            _withoutListenerSeries.Points.Clear();
            _listenersPeakSeries.Points.Clear();

            _averagePlotSeries.Points.AddRange(AnalyzedData.Select(entry => new DataPoint(DateTimeAxis.ToDouble(entry.Key), entry.Value.Average)));
            _withoutListenerSeries.Points.AddRange(AnalyzedData.Select(entry => new DataPoint(DateTimeAxis.ToDouble(entry.Key), entry.Value.TimeWithoutListeners * 100)));
            _listenersPeakSeries.Points.AddRange(AnalyzedData.Select(entry => new DataPoint(DateTimeAxis.ToDouble(entry.Key), entry.Value.ListenersPeak)));

            AveragePlotModel.InvalidatePlot(true);
            WithoutListenersPlotModel.InvalidatePlot(true);
            ListenersPeakPlotModel.InvalidatePlot(true);
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