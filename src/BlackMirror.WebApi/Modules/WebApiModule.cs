namespace BlackMirror.WebApi.Modules
{
    using System;
    using System.Security.Authentication;
    using BlackMirror.Configuration.SerilogSupport;
    using BlackMirror.Models;
    using Nancy;
    using TradeR.Web.Extensions.Authentication;

    public abstract class WebApiModule : NancyModule
    {
        protected readonly IAuthenticationProvider authenticationProvider;

        protected UserIdentity CurrentUser
        {
            get { return this.authenticationProvider.CurrentUser(this); }
        }

        protected UserId CurrentUserId
        {
            get { return new UserId(this.CurrentUser.Id); }
        }

        protected WebApiModule(IAuthenticationProvider authenticationProvider)
        {
            this.authenticationProvider = authenticationProvider;
        }

        protected WebApiModule(IAuthenticationProvider authenticationProvider, string modulePath)
            : base(modulePath)
        {
            this.authenticationProvider = authenticationProvider;
        }

        public void SetRequestCacheTimeout(int timeout)
        {
            this.Context.Items.Add("CacheTimeout", timeout);
        }

       protected Response ResponseFromException(Exception exc)
        {
            Logging.Log().Warning(exc, "Exception thrown {ExceptionMessage}", exc.Message);

            if (exc is AuthenticationException)
            {
                Logging.Log().Warning(exc, "Failed to authenticate user - access was forbidden, returning unauthorized response. {ExceptionMessage}", exc.Message);
                return new Response { StatusCode = HttpStatusCode.Unauthorized, ReasonPhrase = exc.Message };
            }

            if (exc is FormatException)
            {
                Logging.Log().Warning(exc, "Exception: {ExceptionMessage}", exc.Message);
                return HttpStatusCode.BadRequest;
            }
            

            Logging.Log().Error(exc, "Error: {ExceptionMessage}", exc.Message);
            return new Response
                       {
                           StatusCode = HttpStatusCode.InternalServerError,
                           ReasonPhrase = exc.Message
                       };
        }

        protected void LogRequest()
        {
            var handler = this.Request.Path + "(" + this.Request.Method + ")";
            Logging.Log().Information("DestoroyahModule: Invoked handler: {Handler}", handler);
        }
    }
}