namespace BlackMirror.WebApi.Modules
{
    using System;
    using BlackMirror.Dto;
    using BlackMirror.MirrorStore.MongoDB;
    using BlackMirror.ReflectionStore.MongoDB;
    using BlackMirror.SyncStore.MongoDB;
    using Nancy;
    using Nancy.ModelBinding;
    using TradeR.Web.Extensions.Authentication;

    public class MirrorModule : WebApiModule
    {
        public MirrorModule(IAuthenticationProvider authenticationProvider, IMirrorStore mirrorStore, ISyncStore syncStore, IReflectionStore reflectionStore)
            : base(authenticationProvider, "/mirror")
        {
            this.Get["/"] = parameters =>
                {
                    try
                    {
                        var userId = this.CurrentUser.Id;
                        var response = mirrorStore.GetAll();

                        if (response == null)
                        {
                            return HttpStatusCode.NotFound;
                        }

                        return this.Response.AsJson(response);
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
                        var mirror = mirrorStore.GetById(id);

                        return this.Response.AsJson(mirror);
                    }
                    catch (Exception e)
                    {
                        return this.ResponseFromException(e);
                    }
                };

            this.Get["/{id}/reflections"] = parameters =>
                {
                    try
                    {
                        var user = this.CurrentUser;
                        string id = parameters.id;
                        int take;

                        var mirror = mirrorStore.GetById(id);

                        var reflections = Int32.TryParse(this.Request.Query["take"].ToString(), out take)
                                                      ? reflectionStore.GetByMirror(mirror, take)
                                                      : reflectionStore.GetByMirror(mirror);

                        return this.Response.AsJson(reflections);
                    }
                    catch (Exception e)
                    {
                        return this.ResponseFromException(e);
                    }
                };

            this.Get["/{id}/sync"] = parameters =>
                {
                    try
                    {
                        var user = this.CurrentUser;
                        string id = parameters.id;
                        //Thread.Sleep(1000);
                        int take;

                        var mirror = mirrorStore.GetById(id);

                        var reflections = Int32.TryParse(this.Request.Query["take"].ToString(), out take)
                                              ? syncStore.GetByMirror(mirror, take)
                                              : syncStore.GetByMirror(mirror);

                        return this.Response.AsJson(reflections);
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
                        var dto = this.Bind<MirrorDto>();
                        if (string.IsNullOrEmpty(dto.OwnerId))
                        {
                            dto.OwnerId = user.Id;
                        }

                        var mirror = mirrorStore.Add(dto);

                        return this.Response.AsJson(mirror);
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
                        var dto = this.Bind<MirrorDto>();

                        if (string.IsNullOrEmpty(dto.SourceRepositoryId))
                        {
                            dto.SourceRepositoryId = dto.SourceRepository.Id;
                        }

                        if (string.IsNullOrEmpty(dto.TargetRepositoryId))
                        {
                            dto.TargetRepositoryId = dto.TargetRepository.Id;
                        }

                        if (string.IsNullOrEmpty(dto.OwnerId))
                        {
                            dto.OwnerId = dto.Owner.Id;
                        }

                        var mirror = mirrorStore.GetById(id);

                        if (mirror == null)
                        {
                            return HttpStatusCode.NotFound;
                        }

                        mirrorStore.Update(id, dto);

                        return HttpStatusCode.OK;
                    }
                    catch (Exception e)
                    {
                        return this.ResponseFromException(e);
                    }
                };

            this.Delete["/{id}"] = parameters =>
                {
                    var user = this.CurrentUser;

                    string id = parameters.id;

                    try
                    {
                        mirrorStore.Delete(id);
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