using System;
using System.Web;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using Batteries.Models;
using Batteries.Bll;

namespace Batteries.Account
{
    public partial class ForgotPassword : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Forgot_Click(object sender, EventArgs e)
        {
            var result = Bl.CreateResetPasswordToken(Email.Text);
            if (!result)
            {
                DisplayErrorMessage("The user does not exist!");
                return;
            }
            HideLoginForm();
        }

        private void DisplayErrorMessage(string error)
        {
            FailureText.Text = error;
            ErrorMessage.Visible = true;
        }

        private void HideLoginForm()
        {
            loginForm.Visible = false;
            DisplayEmail.Visible = true;
        }
    }
}