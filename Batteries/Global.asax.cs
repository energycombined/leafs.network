using Batteries.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace Batteries
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        void Application_BeginRequest(object sender, EventArgs e)
        {
            if (Request.AppRelativeCurrentExecutionFilePath == "~/")
                HttpContext.Current.RewritePath("default.aspx");
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            if (HttpContext.Current.User == null) return;
            if (!HttpContext.Current.User.Identity.IsAuthenticated) return;
            var id = HttpContext.Current.User.Identity as FormsIdentity;
            if (id == null) return;
            var user = JsonConvert.DeserializeObject<User>(id.Ticket.UserData);
            var roles = new[] { user.userRole.roleName };
            HttpContext.Current.User = new GenericPrincipal(id, roles);
        }
    }
}