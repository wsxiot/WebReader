using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebReader.Startup))]
namespace WebReader
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
