using Batteries.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Batteries
{
    public partial class Public : System.Web.UI.MasterPage
    {
        //public string userFullName;
        //public string userRoleName;
        //public bool userIsLoggedIn;

        protected void Page_Load(object sender, EventArgs e)
        {
            //userIsLoggedIn = UserHelper.UserIsLoggedIn();
            //if (userIsLoggedIn)
            //{
            //    var currentUser = UserHelper.GetCurrentUser();
            //    userFullName = currentUser.FirstName + " " + currentUser.LastName;
            //    //userRoleName = currentUser.RoleName;
            //}
        }
    }
}