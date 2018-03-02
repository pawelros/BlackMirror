namespace BlackMirror.WebApi.Host
{
    using BlackMirror.Interfaces;
    using BlackMirror.Interfaces.Configuration;
    using BlackMirror.LogStore.MongoDB;
    using BlackMirror.MirrorStore.MongoDB;
    using BlackMirror.ReflectionStore.MongoDB;
    using BlackMirror.Svc.UserStore.MongoDB;
    using BlackMirror.SvcRepositoryStore.MongoDB;
    using BlackMirror.Sync;
    using BlackMirror.SyncStore.MongoDB;
    using BlackMirror.WebApi.Events;
    using Nancy.TinyIoc;
    using Newtonsoft.Json;
    using TradeR.Web.Extensions.Authentication;

    public class DependencyBootstrapper : CommonDependencyBootstrapper
    {
        private readonly IWebApiConfiguration webApiConfiguration;
        private readonly IAuthenticationProvider authenticationProvider;

        public DependencyBootstrapper(IWebApiConfiguration webApiConfiguration, IAuthenticationProvider authenticationProvider)
            : base(webApiConfiguration, authenticationProvider)
        {
            this.webApiConfiguration = webApiConfiguration;
            this.authenticationProvider = authenticationProvider;
        }

        public override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            container.Register<JsonSerializer, CustomJsonSerializer>();
            container.Register<IWebApiConfiguration>(this.webApiConfiguration);

            container.Register<IUserStore>(new UserStore(container.Resolve<IWebApiConfiguration>()));
            container.Register<ISvcRepositoryStore>(new SvcRepositoryStore(
                container.Resolve<IWebApiConfiguration>(),
                container.Resolve<IUserStore>()));
            container.Register<IMirrorStore>(new MirrorStore(
                container.Resolve<IWebApiConfiguration>(),
                container.Resolve<ISvcRepositoryStore>(),
                container.Resolve<IUserStore>()));

            //container.Register<IGit>(new Git());
            //container.Register<ISvn>(new Svn());

            container.Register<ISyncService>(new SyncService(container.Resolve<IMirrorStore>()));
            container.Register<ISyncStore>(
                new SyncStore(container.Resolve<IWebApiConfiguration>(), container.Resolve<IMirrorStore>()));
            container.Register<IReflectionStore>(new ReflectionStore(container.Resolve<IWebApiConfiguration>(), container.Resolve<ISyncStore>()));

            container.Register<ILogStore>(new LogStore(container.Resolve<IWebApiConfiguration>()));

            container.Register<EventHandler>(
                new EventHandler(
                    container.Resolve<ISvcRepositoryStore>(),
                    container.Resolve<IMirrorStore>(),
                    container.Resolve<ISyncStore>()));

            container.Register<IAuthenticationProvider>(this.authenticationProvider);
            base.ConfigureApplicationContainer(container);
        }
    }
}