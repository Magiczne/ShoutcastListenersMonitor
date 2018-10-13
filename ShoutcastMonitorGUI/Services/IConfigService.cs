namespace ShoutcastMonitorGUI.Services
{
    public interface IConfigService
    {
        /// <summary>
        ///     Update setting
        /// </summary>
        /// <param name="settingName"></param>
        /// <param name="value"></param>
        void Update(string settingName, object value);

        /// <summary>
        ///     Get setting value
        /// </summary>
        /// <param name="settingName"></param>
        /// <returns></returns>
        object Get(string settingName);
    }
}