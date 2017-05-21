using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(mt.Startup))]

namespace mt
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
