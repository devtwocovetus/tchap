using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Web.Http;
using TheCloudHealth.App_Start;

namespace TheCloudHealth
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {

            Environment.SetEnvironmentVariable("http_proxy", "https://localhost:44357/");
            //app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

            //var myProvieder = new MyAuthorization();
            //OAuthAuthorizationServerOptions options = new OAuthAuthorizationServerOptions
            //{
            //    //AllowInsecureHttp = true,
            //    TokenEndpointPath = new Microsoft.Owin.PathString("/oauth/token"),
            //    AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
            //    Provider = myProvieder
            //};
            //app.UseOAuthAuthorizationServer(options);
            //app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

            //HttpConfiguration config = new HttpConfiguration();
            //WebApiConfig.Register(config);
        }
    }
}