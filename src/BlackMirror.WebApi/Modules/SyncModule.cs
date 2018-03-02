namespace BlackMirror.WebApi.Modules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BlackMirror.Configuration;
    using BlackMirror.Dto;
    using BlackMirror.LogStore.MongoDB;
    using BlackMirror.Models.Exceptions;
    using BlackMirror.ReflectionStore.MongoDB;
    using BlackMirror.SyncStore.MongoDB;
    using Nancy;
    using Nancy.ModelBinding;
    using TradeR.Web.Extensions.Authentication;

    public class QueryParams
    {
        public string Status { get; set; }
    }

    public class SyncModule : WebApiModule
    {
        public SyncModule(IAuthenticationProvider authenticationProvider, ISyncStore store, IReflectionStore reflectionStore, ILogStore log)
            : base(authenticationProvider, "/sync")
        {
            this.Post["/"] = parameters =>
                {
                    try
                    {
                        var user = this.CurrentUser;
                        var dto = this.Bind<SynchronizationDto>();

                        var sync = store.Add(dto);
                        return this.Response.AsJson(sync);
                    }
                    catch (Exception e)
                    {
                        return this.ResponseFromException(e);
                    }
                };

            this.Get["/"] = parameters =>
                {
                    try
                    {
                        var user = this.CurrentUser;
                        DynamicDictionary q = this.Request.Query;
                        Dictionary<string, string> filters = q.ToDictionary().ToDictionary(t => t.Key, t => t.Value.ToString());

                        var result = store.GetAll(filters);

                        return this.Response.AsJson(result);
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
                        var dto = store.GetById(id);

                        return this.Response.AsJson(dto);
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
                        string syncId = parameters.id;
                        var sync = store.GetById(syncId);
                        int take;
                        var reflections = Int32.TryParse(this.Request.Query["take"].ToString(), out take)
                                              ? reflectionStore.GetBySynchronization(sync, take)
                                              : reflectionStore.GetBySynchronization(sync);

                        return this.Response.AsJson(reflections);
                    }
                    catch (Exception e)
                    {
                        return this.ResponseFromException(e);
                    }
                };

            this.Put["/{id}/status"] = parameters =>
                {
                    try
                    {
                        var user = this.CurrentUser;
                        var dto = this.Bind<UpdateStatusDto>();

                        if (dto != null && dto.Status.HasValue)
                        {
                            try
                            {
                                string id = parameters.id;
                                store.UpdateStatus(id, dto.Status.Value);
                                return HttpStatusCode.OK;
                            }
                            catch (StatusNotChangedException)
                            {
                                return this.Negotiate
                                    .WithReasonPhrase(ReasonPhrases.StatusAlreadyUpdated)
                                    .WithStatusCode(HttpStatusCode.Conflict);
                            }
                        }

                        else
                        {
                            return this.Negotiate
                                .WithReasonPhrase(ReasonPhrases.UpdateStatusBadRequest)
                                .WithStatusCode(HttpStatusCode.BadRequest);
                        }
                    }
                    catch (Exception e)
                    {
                        return this.ResponseFromException(e);
                    }
                };

            this.Put["{id}/logs"] = parameters =>
                {
                    try
                    {
                        var user = this.CurrentUser;
                        var dto = this.Bind<SyncLogDto>();

                        if (dto != null && !string.IsNullOrEmpty(dto.Text))
                        {
                            try
                            {
                                string syncId = parameters.id;
                                var sync = store.GetById(syncId);
                                log.LogSync(sync, dto.Text);
                                return HttpStatusCode.OK;
                            }
                            catch (StatusNotChangedException)
                            {
                                return this.Negotiate
                                    .WithReasonPhrase("Log bad request.")
                                    .WithStatusCode(HttpStatusCode.BadRequest);
                            }
                        }
                        else
                        {
                            return this.Negotiate
                                .WithReasonPhrase("Log bad request.")
                                .WithStatusCode(HttpStatusCode.BadRequest);
                        }
                    }
                    catch (Exception e)
                    {
                        return this.ResponseFromException(e);
                    }
                };

            this.Get["{id}/logs"] = parameters =>
                {
                    try
                    {
                        var user = this.CurrentUser;
                        try
                        {
                            string syncId = parameters.id;
                            var sync = store.GetById(syncId);

                            if (sync == null)
                            {
                                return this.Negotiate
                                    .WithReasonPhrase("Sync not found.")
                                    .WithStatusCode(HttpStatusCode.NotFound);
                            }

                            var l = log.Get(sync);

                            if (l == null)
                            {
                                return this.Negotiate
                                    .WithReasonPhrase("Log not found.")
                                    .WithStatusCode(HttpStatusCode.NotFound);
                            }

                            return this.Response.AsJson(l);
                        }
                        catch (StatusNotChangedException)
                        {
                            return this.Negotiate
                                .WithReasonPhrase("Log bad request.")
                                .WithStatusCode(HttpStatusCode.BadRequest);
                        }
                    }
                    catch (Exception e)
                    {
                        return this.ResponseFromException(e);
                    }
                };
        }

    }
}