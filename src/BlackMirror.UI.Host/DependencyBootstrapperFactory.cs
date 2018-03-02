namespace BlackMirror.UI.Host
{
    using BlackMirror.Interfaces;
    using BlackMirror.Interfaces.Configuration;
    using TradeR.Web.Extensions.Authentication;

    public static class DependencyBootstrapperFactory
    {
        public static IDependencyBootstrapper Create(IUserInterfaceConfiguration uiConfig, IAuthenticationProvider authenticationProvider)
        {
            return new DependencyBootstrapper(uiConfig, authenticationProvider);
        }
    }
}