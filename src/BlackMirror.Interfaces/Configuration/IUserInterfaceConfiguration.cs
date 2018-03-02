namespace BlackMirror.Interfaces.Configuration
{
    public interface IUserInterfaceConfiguration
    {
        string[] UrlNamespace { get; }
        string ApiUrl { get; }
        string AuthenticationProvider { get; }
    }
}