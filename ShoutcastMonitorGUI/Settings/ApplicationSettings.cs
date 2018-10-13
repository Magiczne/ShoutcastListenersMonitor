namespace ShoutcastMonitorGUI.Settings
{
    public class ApplicationSettings : ISettings
    {
        /// <inheritdoc />
        public object this[string propertyName]
        {
            get => Properties.Settings.Default[propertyName];
            set => Properties.Settings.Default[propertyName] = value;
        }

        /// <inheritdoc />
        public void Save()
        {
            Properties.Settings.Default.Save();
        }
    }
}