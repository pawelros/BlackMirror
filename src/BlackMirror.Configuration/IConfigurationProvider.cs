namespace BlackMirror.Configuration
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IConfigurationProvider
    {
        T GetSetting<T>(string key);
        Task<T> GetSettingAsync<T>(string key);
        Dictionary<string, string[]> GetAllSettings(string environement);
    }
}
