namespace ShoutcastMonitorGUI.Settings
{
    public interface ISettings
    {
        /// <summary>
        ///     Setting property
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        object this[string propertyName] { get; set; }

        /// <summary>
        ///     Save Settings
        /// </summary>
        void Save();
    }
}