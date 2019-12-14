using Microsoft.Owin;
using Owin;

//[assembly: OwinStartupAttribute(typeof(TheCloudHealth.Startup))]
namespace TheCloudHealth
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}