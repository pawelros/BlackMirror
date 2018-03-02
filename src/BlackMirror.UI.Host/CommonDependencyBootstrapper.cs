namespace BlackMirror.UI.Host
{
    using BlackMirror.Interfaces;
    using BlackMirror.Interfaces.Configuration;
    using Nancy.TinyIoc;
    using TradeR.Web.Extensions.Authentication;

    public abstract class CommonDependencyBootstrapper : IDependencyBootstrapper
    {
        private readonly IUserInterfaceConfiguration uiConfig;
        private readonly IAuthenticationProvider authenticationProvider;

        protected CommonDependencyBootstrapper(IUserInterfaceConfiguration uiConfig, IAuthenticationProvider authenticationProvider)
        {
            this.uiConfig = uiConfig;
            this.authenticationProvider = authenticationProvider;
        }

        public virtual void ConfigureApplicationContainer(TinyIoCContainer container)
        {
          
        }
    }
}