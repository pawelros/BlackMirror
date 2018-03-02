namespace TradeR.Web.Extensions.Authentication
{
    using System.Net;
    using Nancy;

    public interface IAuthenticationProvider
    {
        UserIdentity CurrentUser(INancyModule module);

        AuthenticationSchemes AuthenticationSchemeForClient(HttpListenerRequest request);
    }
}
