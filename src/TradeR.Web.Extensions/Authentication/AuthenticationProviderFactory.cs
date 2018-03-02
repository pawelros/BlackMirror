namespace TradeR.Web.Extensions.Authentication
{
    using System;
    using Owin;

    public class AuthenticationProviderFactory
    {
        public static IAuthenticationProvider Create(IAppBuilder app, string providerName, string[] trustedServiceAccounts)
        {
            switch (providerName)
            {
                case "ClientCertificateAuthenticationProvider":
                    return new ReverseProxyClientCertificateAuthenticationProvider(trustedServiceAccounts);
                case "OwinClientCertificateAuthenticationProvider":
                    return new OwinClientCertificateAuthenticationProvider(app, trustedServiceAccounts);
                case "IpListAuthenticationProvider":
                    throw new NotSupportedException("IpListAuthenticationProvider is not supported yet.");
                case "MultiAuthenticationProvider":
                    throw new NotSupportedException("MultiAuthenticationProvider is not supported yet.");
                case "NtlmAuthenticationProvider":
                    return new NtlmAuthenticationProvider(trustedServiceAccounts);
                default:
                    throw new NotSupportedException(string.Format("The given authentication provider: {0} is not supported.", providerName));
            }
        }
    }
}
