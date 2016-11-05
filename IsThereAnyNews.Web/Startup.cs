using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(IsThereAnyNews.Web.Startup))]
namespace IsThereAnyNews.Web
{
    using IsThereAnyNews.Infrastructure.ConfigurationReader.Implementation;

    public partial class Startup
    {

        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
