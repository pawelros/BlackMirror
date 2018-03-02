namespace BlackMirror.WebApi.Host
{
    using System;
    using System.Diagnostics;
    using Topshelf;

    public static class Host
    {
        public static void Run<T>(Func<T> applicationFactory)
            where T : class, IApplication
        {
            var processName = Process.GetCurrentProcess().ProcessName;

            HostFactory.Run(
                host =>
                    {
                        host.UseSerilog();
                        host.ApplyCommandLine(String.Empty);
                        host.SetServiceName(processName);
                        host.SetDescription(processName);
                        host.SetDisplayName(processName);

                        host.Service<T>(
                            s =>
                                {
                                    s.ConstructUsing(applicationFactory);
                                    s.WhenStarted(app => app.Start());
                                    s.WhenStopped(app => app.Stop());
                                });
                    });
        }
    }
}