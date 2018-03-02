namespace BlackMirror.WebApi.Host
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text.RegularExpressions;
    using BlackMirror.Configuration.SerilogSupport;
    using Nancy;
    using Nancy.Bootstrapper;
    using Nancy.TinyIoc;

    public class CustomBootstrapper : DefaultNancyBootstrapper
    {
        private readonly IDependencyBootstrapper dependencyBootstrapper;

        public CustomBootstrapper(IDependencyBootstrapper dependencyBootstrapper)
        {
            this.dependencyBootstrapper = dependencyBootstrapper;
            this.corsMethodsPatterns = new Lazy<Tuple<Regex, string[]>[]>(this.GetCorsMethodsPatterns);
        }

        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            this.dependencyBootstrapper.ConfigureApplicationContainer(container);
        }

        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);

            pipelines.BeforeRequest.AddItemToStartOfPipeline(
                context =>
                    {
                        Logging.Log().Information("Received request {request} ({method})", context.Request.Path, context.Request.Method);
                        return null;
                    });

            pipelines.OnError.AddItemToEndOfPipeline((context, exception) =>
                {
                    Logging.Log().Error(exception, "Unhandled exception.");
                    return null;
                });

            pipelines.AfterRequest += context =>
                {
                    if (!string.IsNullOrEmpty(context.Request.Headers.Referrer))
                    {
                        var uri = new Uri(context.Request.Headers.Referrer);
                        context.Response.Headers["Access-Control-Allow-Origin"]
                            = string.Format(CultureInfo.InvariantCulture, @"{0}://{1}", uri.Scheme, uri.Authority);
                    }
                    else
                    {
                        string origin = context.Request.Headers["Origin"]?.FirstOrDefault();
                        if (!string.IsNullOrWhiteSpace(origin))
                        {
                            context.Response.Headers["Access-Control-Allow-Origin"] = origin;
                        }
                    }

                    if (context.Request.Headers["Access-Control-Request-Headers"].Any())
                    {
                        context.Response.Headers["Access-Control-Allow-Headers"] = string.Join(", ", context.Request.Headers["Access-Control-Request-Headers"]);
                    }

                    var methods = this.GetAccessControlAllowMethodsHeader(context).ToArray();

                    if (methods.Any())
                    {
                        context.Response.Headers["Access-Control-Allow-Methods"] = string.Join(", ", methods);
                    }

                    context.Response.Headers["Machine-Name"] = Environment.MachineName;
                    context.Response.Headers["Access-Control-Allow-Credentials"] = "true";
                };

            pipelines.AfterRequest.AddItemToEndOfPipeline(
                context =>
                    {
                        Logging.Log().Information("Processed request {request} ({method}). Returned response {response}",
                            context.Request.Path, context.Request.Method, context.Response.StatusCode);
                    });
        }

        private Tuple<Regex, string[]>[] GetCorsMethodsPatterns()
        {
            var modules = this.Modules
                .Select(x => x.ModuleType)
                .Select(this.ApplicationContainer.Resolve)
                .Cast<NancyModule>();

            var corsMethods = modules.SelectMany(x => x.Routes).Select(
                    x =>
                        {
                            var pattern = Regex.Replace(x.Description.Path.ToUpperInvariant(), "{.+?}", "[^/]+");
                            return Tuple.Create("^" + pattern + "$", x.Description.Method);
                        })
                .GroupBy(
                    x => x.Item1,
                    (x, y) => Tuple.Create(new Regex(x, RegexOptions.IgnoreCase), y.Select(z => z.Item2).ToArray()))
                .ToArray();

            return corsMethods;
        }

        private readonly Lazy<Tuple<Regex, string[]>[]> corsMethodsPatterns;

        private IEnumerable<string> GetAccessControlAllowMethodsHeader(NancyContext context)
        {
            foreach (var methodPattern in this.corsMethodsPatterns.Value)
            {
                if (methodPattern.Item1.IsMatch(context.Request.Path))
                {
                    var methods = methodPattern.Item2.Concat(new string[] { "OPTIONS" });
                    return methods;
                }
            }

            return new string[0];
        }
    }
}