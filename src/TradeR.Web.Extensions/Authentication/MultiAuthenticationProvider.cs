namespace TradeR.Web.Extensions.Authentication
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Security.Authentication;
    using LiteGuard;
    using Nancy;

    public class MultiAuthenticationProvider : AbstractAuthenticationProvider
    {
        private readonly List<IAuthenticationProvider> providers;

        public MultiAuthenticationProvider(string[] trustedServiceAccounts, params IAuthenticationProvider[] providers)
            : this(providers.AsEnumerable(), trustedServiceAccounts)
        {
        }

        public MultiAuthenticationProvider(IEnumerable<IAuthenticationProvider> providers, string[] trustedServiceAccounts)
            : base(trustedServiceAccounts)
        {
            Guard.AgainstNullArgument("providers", providers);

            this.providers = providers.ToList();
        }

        protected override UserIdentity GetCurrentUser(INancyModule module)
        {
            foreach (var provider in this.providers)
            {
                try
                {
                    var user = provider.CurrentUser(module);
                    if (user != null)
                    {
                        return user;
                    }
                }
                catch (AuthenticationException)
                {
                }
            }

            return null;
        }

        public override AuthenticationSchemes AuthenticationSchemeForClient(HttpListenerRequest request)
        {
            return this.providers.Aggregate(AuthenticationSchemes.Anonymous, (current, authenticationProvider)
                => current | authenticationProvider.AuthenticationSchemeForClient(request));
        }
    }
}
