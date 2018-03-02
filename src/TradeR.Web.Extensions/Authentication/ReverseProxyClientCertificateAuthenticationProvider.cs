namespace TradeR.Web.Extensions.Authentication
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Security.Cryptography.X509Certificates;
    using System.Text.RegularExpressions;
    using BlackMirror.Configuration.SerilogSupport;
    using Nancy;

    public class ReverseProxyClientCertificateAuthenticationProvider : AbstractAuthenticationProvider
    {
        private const string ClientCertificateHttpHeaderKey = "X-ARR-ClientCert";

        public ReverseProxyClientCertificateAuthenticationProvider(string[] trustedServiceAccounts)
            : base(trustedServiceAccounts)
        {
        }

        protected override UserIdentity GetCurrentUser(INancyModule module)
        {
            string base64ClientCert = module.Request.Headers[ClientCertificateHttpHeaderKey].First();
            byte[] certData = Convert.FromBase64String(base64ClientCert);

            var certificate = new X509Certificate2(certData);

            Logging.Log().Information(
                "Using the following client certificate to authenticate: {0}", certificate);

            // Don't have to valid the certificate. Validation is performed by IIS.

            var subject = certificate.SubjectName.Name.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(i => i.Trim());
            var cn = subject.Where(i => i.StartsWith("CN=", StringComparison.InvariantCultureIgnoreCase)).Select(i => i.Remove(0, 3)).First();

            string pattern = @"^(.*)\s\((.*)\)$";

            var result = Regex.Match(cn, pattern);

            var identity = new UserIdentity
            {
                Id = cn,
                Name = result.Groups[1].Value,
                Pid = result.Groups[2].Value
            };

            return identity;
        }

        public override AuthenticationSchemes AuthenticationSchemeForClient(HttpListenerRequest request)
        {
            return AuthenticationSchemes.Anonymous;
        }
    }
}
