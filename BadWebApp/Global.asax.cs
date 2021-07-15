using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace BadWebApp
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
#if DEBUG
            
            foreach (var setting in WebConfigurationManager.AppSettings.AllKeys)
            {
                Environment.SetEnvironmentVariable(setting, WebConfigurationManager.AppSettings[setting], EnvironmentVariableTarget.Process);
            }
#endif
        }
    }
}