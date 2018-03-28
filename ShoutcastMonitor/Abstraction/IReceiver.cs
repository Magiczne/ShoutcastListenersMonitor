namespace ShoutcastMonitor.Abstraction
{
    /// <summary>
    ///     Data receiver interface
    /// </summary>
    internal interface IReceiver
    {
        /// <summary>
        ///     Data logger instance
        /// </summary>
        IDataLogger Logger { get; set; }

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