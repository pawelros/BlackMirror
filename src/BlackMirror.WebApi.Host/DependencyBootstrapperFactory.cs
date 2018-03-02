namespace BlackMirror.WebApi.Host
{
    using BlackMirror.Interfaces;
    using BlackMirror.Interfaces.Configuration;
    using TradeR.Web.Extensions.Authentication;

    public static class DependencyBootstrapperFactory
    {
        public static IDependencyBootstrapper Create(IWebApiConfiguration webApiConfiguration, IAuthenticationProvider authenticationProvider)
        {
            return new DependencyBootstrapper(webApiConfiguration, authenticationProvider);
        }
    }
}