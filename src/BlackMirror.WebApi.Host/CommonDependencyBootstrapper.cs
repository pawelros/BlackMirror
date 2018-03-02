namespace BlackMirror.WebApi.Host
{
    using BlackMirror.Interfaces;
    using BlackMirror.Interfaces.Configuration;
    using Nancy.TinyIoc;
    using TradeR.Web.Extensions.Authentication;

    public abstract class CommonDependencyBootstrapper : IDependencyBootstrapper
    {
        private readonly IWebApiConfiguration webApiConfiguration;
        private readonly IAuthenticationProvider authenticationProvider;

        protected CommonDependencyBootstrapper(IWebApiConfiguration webApiConfiguration, IAuthenticationProvider authenticationProvider)
        {
            this.webApiConfiguration = webApiConfiguration;
            this.authenticationProvider = authenticationProvider;
        }

        public virtual void ConfigureApplicationContainer(TinyIoCContainer container)
        {
          
        }
    }
}