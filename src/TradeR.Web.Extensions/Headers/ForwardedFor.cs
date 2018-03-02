namespace TradeR.Web.Extensions.Headers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Authentication;
    using Nancy;
    using TradeR.Web.Extensions.Authentication;

    public class ForwardedFor
    {
        private const string Key = "Forwarded";
        private UserIdentity userIdentity;

        private ForwardedFor()
        {
        }

        public UserIdentity User
        {
            get
            {
                return this.userIdentity;
            }
        }

        public static ForwardedFor Create(RequestHeaders requestHeaders)
        {
            string header = requestHeaders[Key].First();

            //acceptable formats:
            // string header = "for=192.0.2.43, for=\"[2001:db8:cafe::17]\", for=unknown, id=\"pawel.rosinski (M065440)\", pid=\"M065440\", name=\"pawel.rosinski\"";
            // string header = "for=192.0.2.43, for=[2001:db8:cafe::17], for=unknown, id=pawel.rosinski (M065440), pid=M065440, name=pawel.rosinski";

            string[] values = header.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(v => v.Trim()).ToArray();

            Dictionary<string, List<string>> r = new Dictionary<string, List<string>>();

            foreach (var value in values)
            {
                string[] wtf = value.Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries);

                if (wtf.Length != 2) continue;

                string key = wtf[0].ToLower().Trim();
                string v = wtf[1].Trim();

                if (v.Length > 2 && v[0] == '"' && v[v.Length - 1] == '"')
                    v = v.Substring(1, v.Length - 2);

                if (!r.ContainsKey(key))
                {
                    r.Add(key, new List<string>());
                }

                if (!r[key].Contains(v))
                { r[key].Add(v); }
            }

            if (!r.ContainsKey("id") || string.IsNullOrWhiteSpace(r["id"].FirstOrDefault()))
            {
                throw new AuthenticationException(string.Format("Forwarded http header '{0}' does not contain user id", header));
            }

            if (!r.ContainsKey("pid") || string.IsNullOrWhiteSpace(r["pid"].FirstOrDefault()))
            {
                throw new AuthenticationException(string.Format("Forwarded http header '{0}' does not contain user pid", header));
            }

            if (!r.ContainsKey("name") || string.IsNullOrWhiteSpace(r["name"].FirstOrDefault()))
            {
                throw new AuthenticationException(string.Format("Forwarded http header '{0}' does not contain user name", header));
            }

            return new ForwardedFor
            {
                userIdentity = new UserIdentity
                {
                    Id = r["id"].FirstOrDefault(),
                    Pid = r["pid"].FirstOrDefault(),
                    Name = r["name"].FirstOrDefault()
                }
            };
        }

        public static bool Exist(RequestHeaders requestHeaders)
        {
            return requestHeaders.Keys.Contains(Key);
        }
    }
}
