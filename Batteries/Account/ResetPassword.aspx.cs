using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using Batteries.Models;
using Batteries.Bll;

namespace Batteries.Account
{
    public partial class ResetPassword : Page
    {
        protected string StatusMessage
        {
            get;
            private set;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
        }
        protected void Reset_Click(object sender, EventArgs e)
        {
            var code = Request.QueryString["token"];
            if (code != null)
            {
                //var result = Bl.ResetPassword(Email.Text, code, Password.Text.Trim());
                var result = Bl.ResetPassword(null, code, Password.Text.Trim());
                if (result)
                {
                    RedirectToLoginPage();
                    return;
                }

                DisplayErrorMessage("Error!");
                return;
            }
            DisplayErrorMessage("Error!");
        }

        private void RedirectToLoginPage()
        {
            Response.Redirect("~/Account/Login?m=ResetPwdSuccess");
        }
        private void DisplayErrorMessage(string error)
        {
            ErrorMessage.Text = "Error!";
        }
    }
}