using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Helpers
{
    /// <summary>
    /// Helper for page redirection 
    /// </summary>
    public class RedirectHelper
    {
        public static void RedirectToReturnUrl(string returnUrl, HttpResponse response)
        {
            if (!String.IsNullOrEmpty(returnUrl) && IsLocalUrl(returnUrl))
            {
                response.Redirect(returnUrl);
            }
            else
            {
                response.Redirect("~/");
            }

            //HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        private static bool IsLocalUrl(string url)
        {
            return !string.IsNullOrEmpty(url) &&
                   ((url[0] == '/' && (url.Length == 1 || (url[1] != '/' && url[1] != '\\'))) ||
                    (url.Length > 1 && url[0] == '~' && url[1] == '/'));
        }

        public const string CodeKey = "token";
        public static string GetCodeFromRequest(HttpRequest request)
        {
            return request.QueryString[CodeKey];
        }
    }
}