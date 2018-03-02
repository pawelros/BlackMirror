namespace BlackMirror.WebApi.Host
{
    using Nancy.TinyIoc;

    public interface IDependencyBootstrapper
    {
        void ConfigureApplicationContainer(TinyIoCContainer container);
    }
}