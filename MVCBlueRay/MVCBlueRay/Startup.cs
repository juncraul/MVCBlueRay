using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MVCBlueRay.App_Start.Startup))]
namespace MVCBlueRay.App_Start
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}