using System;

namespace DataAnalyzer.Models
{
    public class AnalyzedData
    {
        /// <summary>
        ///     Date
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        ///     Average listeners count
        /// </summary>
        public double Average { get; set; }

        /// <summary>
        ///     Time without listeners
        /// </summary>
        public double TimeWithoutListeners { get; set; }

        /// <summary>
        ///     Listeners peak
        /// </summary>
        public int ListenersPeak { get; set; }
    }
}