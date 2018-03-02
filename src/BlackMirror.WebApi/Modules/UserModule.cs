namespace BlackMirror.WebApi.Modules
{
    using System;
    using System.Security.Cryptography;
    using BlackMirror.Configuration.SerilogSupport;
    using BlackMirror.Dto;
    using BlackMirror.Interfaces;
    using BlackMirror.Models;
    using BlackMirror.Svc.UserStore.MongoDB;
    using Nancy;
    using Nancy.ModelBinding;
    using TradeR.Web.Extensions.Authentication;

    public class UserModule : WebApiModule
    {
        public UserModule(IAuthenticationProvider authenticationProvider, IUserStore userStore)
            : base(authenticationProvider, "/user")
        {
            this.Get["/"] = parameters =>
                {
                    try
                    {
                        var users = userStore.GetAll();

                        return this.Response.AsJson(users);
                    }
                    catch (Exception e)
                    {
                        return this.ResponseFromException(e);
                    }
                };

            this.Get["/self"] = parameters =>
                {
                    try
                    {
                        var user = this.CurrentUser;

                        if (user == null)
                        {
                            return HttpStatusCode.NotFound;
                        }

                        IUser dbUser = null;

                        try
                        {
                            dbUser = userStore.GetById(user.Id);
                        }
                        catch (Exception ex)
                        {
                            
                        }

                        return this.Response.AsJson(new
                        {
                            Auth = user,
                            Exists = dbUser != null,
                            Identity = dbUser
                        });
                    }
                    catch (Exception e)
                    {
                        return this.ResponseFromException(e);
                    }
                };

            this.Get["{id}"] = parameters =>
                {
                    try
                    {
                        var userX = this.CurrentUser;
                        string id = parameters.id;
                        var user = userStore.GetById(id);

                        if (user == null)
                        {
                            return HttpStatusCode.NotFound;
                        }

                        return this.Response.AsJson(user);
                    }
                    catch (Exception e)
                    {
                        return this.ResponseFromException(e);
                    }
                };

            this.Get["{id}/credentials/{repositoryType}/password"] = parameters =>
                {
                    try
                    {
                        var userX = this.CurrentUser;
                        string id = parameters.id;
                        string repositoryType = parameters.repositoryType;

                        SvcRepositoryType rt;

                        if (!Enum.TryParse(repositoryType, out rt))
                        {
                            return this.Negotiate
                                .WithStatusCode(HttpStatusCode.BadRequest)
                                .WithReasonPhrase("Invalid repository type.");
                        }

                        string passphrase = this.Request.Query.passphrase;

                        var user = userStore.GetById(id);

                        if (user == null)
                        {
                            return HttpStatusCode.NotFound;
                        }

                        try
                        {
                            string password = userStore.GetPassword(id, rt, passphrase);
                            return this.Response.AsJson(new { Password = password });
                        }
                        catch (CryptographicException cex)
                        {
                            Logging.Log().Error(cex, $"Failed to retrieve user {id} password.");
                            return HttpStatusCode.Unauthorized;
                        }
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
                        var userX = this.CurrentUser;
                        var dto = this.Bind<UserWithPasswordsDto>();

                        var user = userStore.Add(dto);

                        return this.Response.AsJson(user);
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
                        var dto = this.Bind<UserWithPasswordsDto>();

                        var x = userStore.GetById(id);

                        if (x == null)
                        {
                            return HttpStatusCode.NotFound;
                        }

                        userStore.Update(id, dto);

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
                        string id = parameters.id;

                        userStore.Delete(id);

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