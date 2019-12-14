using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Twilio.Jwt.AccessToken;

namespace TheCloudHealth.Domain
{
    public interface ITokenGenerator
    {
        string Generate(string identity, string endpointId);
    }

    public class TokenGenerator : ITokenGenerator
    {
        [Obsolete]
        public string Generate(string identity, string endpointId)
        {
            var grants = new HashSet<IGrant>
            {
                new IpMessagingGrant {EndpointId = endpointId, ServiceSid = Configuration.IpmServiceSID}
            };

            var token = new Token(
                Configuration.AccountSID,
                Configuration.ApiKey,
                Configuration.ApiSecret,
                identity,
                grants: grants);

            return token.ToJwt();
        }
    }
}