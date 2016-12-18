using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Bus_company.Startup))]
namespace Bus_company
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
