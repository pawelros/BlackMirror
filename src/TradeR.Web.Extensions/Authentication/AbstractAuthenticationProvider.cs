namespace TradeR.Web.Extensions.Authentication
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Security.Authentication;
    using BlackMirror.Configuration.SerilogSupport;
    using Nancy;
    using TradeR.Web.Extensions.Headers;

    public abstract class AbstractAuthenticationProvider : IAuthenticationProvider
    {
        private readonly string[] trustedServiceAccounts;

        protected AbstractAuthenticationProvider(string[] trustedServiceAccounts)
        {
            this.trustedServiceAccounts = trustedServiceAccounts;
        }

        public UserIdentity CurrentUser(INancyModule module)
        {
            try
            {
                var user = this.GetCurrentUser(module);

                if (user == null)
                {
                    Logging.Log().Error("Unknown user. Throwing AuthenticationException...");

                    throw new AuthenticationException("Cannot authenticate user.");
                }

                Logging.Log().Information("User claims he is {0}", user.Id);

                if (module != null
                    && ForwardedFor.Exist(module.Request.Headers))
                {
                    ForwardedFor header = ForwardedFor.Create(module.Request.Headers);
                    if (this.UserIsTrustedServiceAccount(user))
                    {
                        Logging.Log().Information("User {0} is on the list of trusted service accounts. User {0} acts on behalf of the following user: id={1}, pid={2}, name={3}.", user.Id, header.User.Id, header.User.Pid, header.User.Name);

                        return header.User;
                    }
                    else
                    {
                        Logging.Log().Warning("User {0} is NOT on the list of trusted service accounts, but sent Forwarded header.", user.Id);
                    }
                }

                return user;
            }
            catch (Exception exception)
            {
                Logging.Log().Error("Unknown user. Throwing AuthenticationException...");

                throw new AuthenticationException("Cannot authenticate user.", exception);
            }
        }

        private bool UserIsTrustedServiceAccount(UserIdentity user)
        {
            return this.trustedServiceAccounts.Contains(user.Id, StringComparer.InvariantCultureIgnoreCase);
        }

        public abstract AuthenticationSchemes AuthenticationSchemeForClient(HttpListenerRequest request);

        protected abstract UserIdentity GetCurrentUser(INancyModule module);
    }
}
