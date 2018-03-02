namespace BlackMirror.WebApi.Modules
{
    using System;
    using BlackMirror.Dto;
    using BlackMirror.SvcRepositoryStore.MongoDB;
    using Nancy;
    using Nancy.ModelBinding;
    using TradeR.Web.Extensions.Authentication;

    public class RepositoryModule : WebApiModule
    {
        public RepositoryModule(IAuthenticationProvider authenticationProvider, ISvcRepositoryStore store)
            : base(authenticationProvider, "/repository")
        {
            this.Get["/"] = parameters =>
                {
                    try
                    {
                        var user = this.CurrentUser;
                        var repositories = store.GetAll();

                        return this.Response.AsJson(repositories);
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
                        string id = (string)parameters.id;

                        var repository = store.GetById(id);

                        if (repository == null)
                        {
                            return HttpStatusCode.NotFound;
                        }

                        return this.Response.AsJson(repository);
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
                        var dto = this.Bind<SvcRepositoryDto>();

                        var model = store.Add(dto);

                        return this.Response.AsJson(model);
                    }
                    catch (Exception e)
                    {
                        return this.ResponseFromException(e);
                    }
                };

            this.Put["/{id}"] = parameters =>
                {
                    try
                    {
                        var user = this.CurrentUser;
                        string id = parameters.id;
                        var dto = this.Bind<SvcRepositoryDto>();

                        if (string.IsNullOrEmpty(dto.CheckoutUserId))
                        {
                            dto.CheckoutUserId = dto.CheckoutUser.Id;
                        }

                        if (string.IsNullOrEmpty(dto.PushUserId))
                        {
                            dto.PushUserId = dto.PushUser.Id;
                        }

                        var repository = store.GetById(id);

                        if (repository == null)
                        {
                            return HttpStatusCode.NotFound;
                        }

                        store.Update(id, dto);

                        return HttpStatusCode.OK;
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
                        string id = (string)parameters.id;

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