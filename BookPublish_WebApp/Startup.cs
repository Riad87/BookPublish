using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BookPublish_WebApp.Startup))]
namespace BookPublish_WebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
