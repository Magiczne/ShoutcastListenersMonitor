using System;
using ShoutcastMonitorGUI.Settings;

namespace ShoutcastMonitorGUI.Services
{
    public class ConfigService : IConfigService
    {
        /// <summary>
        ///     Settings instance
        /// </summary>
        private readonly ISettings _settings;

        /// <summary>
        ///     Create new instance of ConfigService
        /// </summary>
        public ConfigService(ISettings settings)
        {
            _settings = settings;
        }

        /// <inheritdoc />
        public void Update(string settingName, object value)
        {
            if (string.IsNullOrEmpty(settingName))
            {
                throw new ArgumentNullException("Setting name must be provided");
            }

            var setting = _settings[settingName];

            if (setting == null)
            {
                throw new ArgumentException($"Setting {settingName} does not exist.");
            }

            if (setting.GetType() != value.GetType())
            {
                throw new InvalidCastException($"Unable to cast value to {setting.GetType()}");
            }

            _settings[settingName] = value;
            _settings.Save();
        }

        /// <inheritdoc />
        public object Get(string settingName)
        {
            if (string.IsNullOrEmpty(settingName))
            {
                throw new ArgumentNullException("Setting name must be provided");
            }

            return _settings[settingName];
        }
    }
}