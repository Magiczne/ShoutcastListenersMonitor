using System;

namespace ShoutcastMonitorGUI.Models
{
    public class ListenerData
    {
        /// <summary>
        ///     Data acquistition time
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        ///     Listeners count
        /// </summary>
        public int Listeners { get; set; }

        /// <summary>
        ///     Additional message
        /// </summary>
        public string Message { get; set; }
    }
}