﻿using Microsoft.Owin;
using Owin;
using Tattoo.Web;

[assembly: OwinStartup(typeof(Startup))]
namespace Tattoo.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
