using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Forum.Startup))]
namespace Forum
{

    using System.Globalization;

    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            ConfigureAuth(app);
        }
    }
}
