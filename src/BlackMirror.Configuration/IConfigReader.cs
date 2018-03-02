namespace BlackMirror.Configuration
{
    public interface IConfigReader
    {
        T Fetch<T>(string key);
        string Name { get; }
    }
}
