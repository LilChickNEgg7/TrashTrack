using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Capstone.Startup))]

namespace Capstone
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}