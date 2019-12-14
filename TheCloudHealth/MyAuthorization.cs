using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace TheCloudHealth
{
    public class MyAuthorization : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            if (context.UserName == "SAdmin" && context.Password == "123456")
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, "SAdmin"));
                identity.AddClaim(new Claim("Username", "SAdmin"));
                identity.AddClaim(new Claim(ClaimTypes.Name, "Test"));
                context.Validated();
            }
            else if (context.UserName == "Test" && context.Password == "123")
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, "Admin"));
                identity.AddClaim(new Claim("username", "Test"));
                identity.AddClaim(new Claim(ClaimTypes.Name, "Test"));
                context.Validated();
            }
            else
            {
                context.SetError("Invalid Grant", "Provied Username and Password correct.");
                return;
            }
        }
    }
}