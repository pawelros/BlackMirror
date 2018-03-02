namespace TradeR.Web.Extensions.Authentication
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Security.Claims;
    using System.Security.Cryptography.X509Certificates;
    using System.Text.RegularExpressions;
    using BlackMirror.Configuration.SerilogSupport;
    using Nancy;
    using Nancy.Owin;
    using Owin;
    public class OwinClientCertificateAuthenticationProvider : AbstractAuthenticationProvider
    {
        public OwinClientCertificateAuthenticationProvider(IAppBuilder app, string[] trustedServiceAccounts)
            : base(trustedServiceAccounts)
        {
            app.UseClientCertificateAuthentication();
        }
        protected override UserIdentity GetCurrentUser(INancyModule module)
        {
            var env = module.Context.GetOwinEnvironment();

            // server.User will be set only if certificate validation will succeed
            if (!env.ContainsKey("server.User") || env["server.User"] == null || !env.ContainsKey("ssl.ClientCertificate") || env["ssl.ClientCertificate"] == null)
            {
                return null;
            }

            var clientCertificate = env["ssl.ClientCertificate"] as X509Certificate2;
            var identity = (ClaimsPrincipal)env["server.User"];

            Logging.Log().Information("Using the following client certificate to authenticate: {0}", clientCertificate);

            var disinguishedNameClaim =
                identity.Claims.First(
                    claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/x500distinguishedname");

            var subjectName = disinguishedNameClaim.Value;
            var subject = subjectName.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(i => i.Trim());
            var cn = subject.Where(i => i.StartsWith("CN=", StringComparison.InvariantCultureIgnoreCase)).Select(i => i.Remove(0, 3)).First();

            string pattern = @"^(.*)\s\((.*)\)$";

            var result = Regex.Match(cn, pattern);

            var userIdentity = new UserIdentity
            {
                Id = cn,
                Name = result.Groups[1].Value,
                Pid = result.Groups[2].Value
            };

            return userIdentity;
        }

        public override AuthenticationSchemes AuthenticationSchemeForClient(HttpListenerRequest request)
        {
            return AuthenticationSchemes.Anonymous;
        }
    }
}
