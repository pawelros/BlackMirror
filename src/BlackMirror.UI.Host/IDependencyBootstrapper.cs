namespace BlackMirror.UI.Host
{
    using Nancy.TinyIoc;

    public interface IDependencyBootstrapper
    {
        void ConfigureApplicationContainer(TinyIoCContainer container);
    }
}