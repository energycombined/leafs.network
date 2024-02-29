using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Batteries.Startup))]
namespace Batteries
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
