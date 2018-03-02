namespace BlackMirror.UI.Host
{
    using BlackMirror.Interfaces;
    using BlackMirror.Interfaces.Configuration;
    using Nancy.TinyIoc;
    using Newtonsoft.Json;
    using TradeR.Web.Extensions.Authentication;

    public class DependencyBootstrapper : CommonDependencyBootstrapper
    {
        private readonly IUserInterfaceConfiguration uiConfig;
        private readonly IAuthenticationProvider authenticationProvider;

        public DependencyBootstrapper(IUserInterfaceConfiguration uiConfig, IAuthenticationProvider authenticationProvider)
            : base(uiConfig, authenticationProvider)
        {
            this.uiConfig = uiConfig;
            this.authenticationProvider = authenticationProvider;
        }

        public override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            container.Register<JsonSerializer, CustomJsonSerializer>();
            container.Register<IUserInterfaceConfiguration>(this.uiConfig);

            container.Register<IAuthenticationProvider>(this.authenticationProvider);
            base.ConfigureApplicationContainer(container);
        }
    }
}