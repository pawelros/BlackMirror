namespace BlackMirror.UI.Host.Modules
{
    using Nancy;
    public class MainModule : NancyModule
    {
        public MainModule()
        {
            this.Get["/"] = parameters => this.RenderView();
            //this.Get["/index.html"] = parameters => this.View["Content/index.html"];
            //this.Get["/index.html"] = parameters => this.View["Content/index.html"];
            //this.Get["/index.html"] = parameters => this.View["Content/index.html"];

            this.Get["/{name*}"] = parameters =>
                {
                    // TODO: surely this can be done better
                    string[] ignoredRoutes = new string[] { "content/", "directives/", "js/", "ngViews/", "partials/" };

                    string name = parameters.name.ToString();

                    foreach (var ignoredRoute in ignoredRoutes)
                    {
                        if (name.StartsWith(ignoredRoute))
                        {
                            return HttpStatusCode.NotFound;
                        }
                    }

                    return this.RenderView();
                };
        }

        private object RenderView()
        {
            return this.View["Content/index.html"];
        }
    }
}