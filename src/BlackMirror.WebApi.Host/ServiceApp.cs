namespace BlackMirror.WebApi.Host
{
    using System;
    using System.Net;
    using BlackMirror.Configuration.SerilogSupport;
    using BlackMirror.Interfaces;
    using BlackMirror.Interfaces.Configuration;
    using Microsoft.Owin.Hosting;
    using Owin;
    using TradeR.Web.Extensions.Authentication;

    public sealed class ServiceApp : IDisposable, IApplication
    {
        private readonly IWebApiConfiguration webApiConfiguration;
        private IDisposable webApp;

        public ServiceApp(IWebApiConfiguration webApiConfiguration)
        {
            this.webApiConfiguration = webApiConfiguration;
        }

        public void Start()
        {
            var options = new StartOptions();
            foreach (var url in this.webApiConfiguration.UrlNamespace)
            {
                options.Urls.Add(url);
            }

            this.webApp = WebApp.Start(
                options,
                app =>
                    {
                        var authenticationProvider = AuthenticationProviderFactory.Create(
                            app,
                            this.webApiConfiguration.AuthenticationProvider,
                            new string[0]);
                        var dependencyBootstrapper = DependencyBootstrapperFactory.Create(this.webApiConfiguration, authenticationProvider);

                        var listener = (HttpListener)app.Properties["System.Net.HttpListener"];
                        listener.AuthenticationSchemeSelectorDelegate = authenticationProvider.AuthenticationSchemeForClient;

                        app.UseNancy(x => x.Bootstrapper = new CustomBootstrapper(dependencyBootstrapper));

                        Logging.Log().Information("Started listening on url: {ServiceUrl}", string.Join(", ", this.webApiConfiguration.UrlNamespace));
                    });
        }

        public void Stop()
        {
            this.webApp.Dispose();
            this.webApp = null;

            Logging.Log().Information("Stopped listening on url: {ServiceUrl}", string.Join(", ", this.webApiConfiguration.UrlNamespace));
        }

        public void Dispose()
        {
            if (this.webApp != null)
            {
                this.webApp.Dispose();
                this.webApp = null;
            }
        }
    }
}