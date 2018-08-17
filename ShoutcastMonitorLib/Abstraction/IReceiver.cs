namespace ShoutcastMonitorLib.Abstraction
{
    /// <summary>
    ///     Data receiver interface
    /// </summary>
    public interface IReceiver
    {
        #region Properties

        /// <summary>
        ///     Data logger instance
        /// </summary>
        IDataLogger Logger { get; set; }

        /// <summary>
        ///     Time interval
        /// </summary>
        int TimeInterval { get; set; }

        /// <summary>
        ///     URL address
        /// </summary>
        string StatsUrl { get; set; }

        #endregion

        /// <summary>
        ///     Receive number of listeners
        /// </summary>
        /// <returns>Number of listeners</returns>
        int ReceiveListeners();

        /// <summary>
        ///     Start receiving data
        /// </summary>
        void Start();

        /// <summary>
        ///     Stop receiving data
        /// </summary>
        void Stop();
    }
}