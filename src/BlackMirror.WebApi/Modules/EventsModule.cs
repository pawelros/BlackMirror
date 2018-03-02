namespace BlackMirror.WebApi.Modules
{
    using BlackMirror.Dto;
    using BlackMirror.WebApi.Events;
    using Nancy;
    using Nancy.Extensions;
    using Nancy.ModelBinding;

    public class EventsModule : NancyModule
    {
        public EventsModule(EventHandler eventHandler)
            : base("/events")
        {
            this.Post["/push"] = parameters =>
                {
                    var dto = this.Bind<GitLabPushEventDto>();
                    eventHandler.Handle(dto);

                    return HttpStatusCode.OK;
                };
        }
    }
}