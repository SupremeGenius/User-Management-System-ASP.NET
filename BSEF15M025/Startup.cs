using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BSEF15M025.Startup))]
namespace BSEF15M025
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
