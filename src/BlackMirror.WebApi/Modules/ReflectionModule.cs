namespace BlackMirror.WebApi.Modules
{
    using System;
    using BlackMirror.Dto;
    using BlackMirror.ReflectionStore.MongoDB;
    using Nancy;
    using Nancy.ModelBinding;
    using TradeR.Web.Extensions.Authentication;

    public class ReflectionModule : WebApiModule
    {
        public ReflectionModule(IAuthenticationProvider authenticationProvider, IReflectionStore store)
            : base(authenticationProvider, "/reflection")
        {
            this.Get["/"] = parameters =>
                {
                    try
                    {
                        var user = this.CurrentUser;
                        var all = store.GetAll();

                        return this.Response.AsJson(all);
                    }
                    catch (Exception e)
                    {
                        return this.ResponseFromException(e);
                    }
                };

            this.Get["/{id}"] = parameters =>
                {
                    try
                    {
                        var user = this.CurrentUser;
                        string id = parameters.id;
                        var model = store.GetById(id);

                        return this.Response.AsJson(model);
                    }
                    catch (Exception e)
                    {
                        return this.ResponseFromException(e);
                    }
                };

            this.Post["/"] = parameters =>
                {
                    try
                    {
                        var user = this.CurrentUser;
                        var dto = this.Bind<ReflectionDto>();
                        var model = store.Add(dto);

                        return this.Response.AsJson(model);
                    }
                    catch (Exception e)
                    {
                        return this.ResponseFromException(e);
                    }
                };

            this.Delete["/{id}"] = parameters =>
                {
                    try
                    {
                        var user = this.CurrentUser;
                        string id = parameters.id;
                        store.Delete(id);

                        return HttpStatusCode.OK;
                    }
                    catch (Exception e)
                    {
                        return this.ResponseFromException(e);
                    }
                };
        }
    }
}