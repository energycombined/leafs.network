using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Routing;
using Microsoft.AspNet.FriendlyUrls;
using System.Web.UI;

namespace Batteries
{
    public static class RouteConfig
    {
        /// <summary>
        /// This is a hack to force no mobile URL resolution in FriendlyUrls.  There's some kind of bug in the current version that
        /// causes it to do an internal failed resolve of a mobile master even though there is none.
        /// </summary>
        public class BugFixFriendlyUrlResolver : Microsoft.AspNet.FriendlyUrls.Resolvers.WebFormsFriendlyUrlResolver
        {
            protected override bool TrySetMobileMasterPage(HttpContextBase httpContext, Page page, string mobileSuffix)
            {
                if (mobileSuffix == "Mobile")
                {
                    return false;
                }
                else
                {
                    return TrySetMobileMasterPage(httpContext, page, mobileSuffix);
                }

                //return base.TrySetMobileMasterPage(httpContext, page, mobileSuffix);
            }
        }
        public static void RegisterRoutes(RouteCollection routes)
        {
            var settings = new FriendlyUrlSettings();
            //settings.AutoRedirectMode = RedirectMode.Permanent;
            //routes.EnableFriendlyUrls(settings);
            settings.AutoRedirectMode = RedirectMode.Off;
            routes.EnableFriendlyUrls(settings, new BugFixFriendlyUrlResolver());
        }
    }
}
