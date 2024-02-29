using Batteries.Dal;
using Batteries.Helpers;
using Batteries.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Batteries.Account
{
    public partial class Manage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                changePasswordHolder.Visible = true;

                // Render success message
                var message = Request.QueryString["m"];
                if (message != null)
                {
                    // Strip the query string from action
                    Form.Action = ResolveUrl("~/Account/Manage");

                    SuccessMessage =
                        message == "ChangePwdSuccess" ? "The password was changed!" : String.Empty;
                    successMessage.Visible = !String.IsNullOrEmpty(SuccessMessage);

                    if (message == "ChangePwdSuccess")
                    {
                        //NotifyHelper.Notify("Password changed. Log back in.", NotifyHelper.NotifyType.success, "");
                        HttpContext.Current.Session.Abandon();
                        FormsAuthentication.SignOut();
                        Response.Redirect("~/Account/Login?m=ChangePwdSuccess");
                    }
                }
            }
        }

        protected string SuccessMessage
        {
            get;
            private set;
        }

        protected void UpdateButton_OnClick(object sender, EventArgs e)
        {
            if (!IsValid) return;
            var id = (FormsIdentity)User.Identity;
            var user = JsonConvert.DeserializeObject<User>(id.Ticket.UserData);
            try
            {
                var result = new UserDa().ChangeUserPassword(user.userId, CurrentPassword.Text.Trim(), NewPassword.Text.Trim());
                if (result)
                {
                    Response.Redirect("~/Account/Manage?m=ChangePwdSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "The password was not changed");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
        }
    }
}