namespace TradeR.Web.Extensions.Authentication
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using BlackMirror.Configuration.SerilogSupport;
    using LiteGuard;
    using Nancy;
    using Spg.Configuration;

    public class IpListAuthenticationProvider : AbstractAuthenticationProvider
    {
        private readonly IConfigurationProvider configurationProvider;
        private readonly string configKey;
        private IDictionary<string, string> ipMap;

        public IpListAuthenticationProvider(IConfigurationProvider configurationProvider, string configKey, string[] trustedServiceAccounts)
            : base(trustedServiceAccounts)
        {
            Guard.AgainstNullArgument("configurationProvider", configurationProvider);

            this.configurationProvider = configurationProvider;
            this.configKey = configKey;
        }

        private IDictionary<string, string> IpMap
        {
            get
            {
                if (this.ipMap == null)
                {
                    try
                    {
                        this.ipMap = this.configurationProvider
                            .GetAppSetting(this.configKey)
                            .Split('|')
                            .ToDictionary(
                                entry => entry.Split(';').First(),
                                entry => entry.Split(';').Last()
                            );
                    }
                    catch (Exception exception)
                    {
                        Logging.Log().Error(
                            "Unable to load ip map for config key: " + configKey,
                            exception
                        );

                        this.ipMap = new Dictionary<string, string>();
                    }
                }

                return this.ipMap;
            }
        }

        protected override UserIdentity GetCurrentUser(INancyModule module)
        {
            string user = this.IpMap[module.Request.UserHostAddress];
            string pid = user.Split('\\')[1];

            return new UserIdentity { Id = user, Pid = pid };
        }

        public override AuthenticationSchemes AuthenticationSchemeForClient(HttpListenerRequest request)
        {
            return AuthenticationSchemes.Anonymous;
        }
    }
}
