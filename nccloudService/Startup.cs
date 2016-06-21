using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(nccloudService.Startup))]

namespace nccloudService
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureMobileApp(app);
        }
    }
}