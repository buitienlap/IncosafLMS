using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(IncosafCMS.Web.Startup))]
namespace IncosafCMS.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
