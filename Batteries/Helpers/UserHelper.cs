using Batteries.Models;
using Batteries.Models.Responses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Batteries.Helpers
{
    /// <summary>
    /// The current application user helper methods.
    /// </summary>
    public class UserHelper
    {
        public static UserExt GetCurrentUser()
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated == false)
                HttpContext.Current.Response.Redirect("/Account/Login");

            FormsIdentity FormId = (FormsIdentity)HttpContext.Current.User.Identity;
            FormsAuthenticationTicket ticket = FormId.Ticket;
            return JsonConvert.DeserializeObject<UserExt>(ticket.UserData);
        }
        public static bool UserIsLoggedIn()
        {
            if (HttpContext.Current.User != null && HttpContext.Current.User.Identity.IsAuthenticated)
            {
                return true;
            }
            return false;
        }

        public static bool IsAdmin()
        {
            return GetCurrentUser().userRole.roleId == 1;
        }

        /*
        public static bool IsSuperAdmin()
        {
            return GetCurrentUser().roleId == 1;
        }

        public static bool IsAdmin()
        {
            return GetCurrentUser().roleId == 2;
        }
        public static bool IsUser()
        {
            return GetCurrentUser().roleId == 3;
        }

        public static string GetFullName()
        {
            return GetCurrentUser().fullName;
        }
       
        public static List<Language> GetUserSelectedLanguages()
        {
            var user = GetCurrentUser();
            if (user.preferredLanguages != null)
            {
                List<Language> langs = Global.ActiveLanguages.Where(x => user.preferredLanguages.Contains(x.languageLocale) || x.languageId == Global.DefaultLanguageId).ToList();
                return langs;
            }

            return Global.ActiveLanguages.Where(x => x.languageId == Global.DefaultLanguageId).ToList();
        }
         
        */
    }
}