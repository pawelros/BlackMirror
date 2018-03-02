namespace TradeR.Web.Extensions.Authentication
{
    using System.Net;
    using Nancy;
    using Nancy.Security;

    public class NtlmAuthenticationProvider : AbstractAuthenticationProvider
    {
        public NtlmAuthenticationProvider(string[] trustedServiceAccounts)
            : base(trustedServiceAccounts)
        {
        }

        protected override UserIdentity GetCurrentUser(INancyModule module)
        {
            var user = module.Context.GetMSOwinUser().Identity.Name;

            var pid = user.Split('\\')[1];

            return new UserIdentity
            {
                Id = user,
                Pid = pid
            };
        }

        public override AuthenticationSchemes AuthenticationSchemeForClient(HttpListenerRequest request)
        {
            return request.HttpMethod.Equals("OPTIONS") ? AuthenticationSchemes.Anonymous : AuthenticationSchemes.Ntlm;
        }
    }
}
