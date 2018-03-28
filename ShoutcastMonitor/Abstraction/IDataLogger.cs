﻿namespace ShoutcastMonitor.Abstraction
{
    /// <summary>
    ///     Data Writer interface
    /// </summary>
    internal interface IDataLogger
    {
        /// <summary>
        ///     Log data
        /// <param name="listeners">Number of listeners to log</param>
        /// </summary>
        void Log(int listeners);

        /// <summary>
        ///     Log error message
        /// </summary>
        /// <param name="message">Error message</param>
        void Error(string message);
    }
}