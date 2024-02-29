using System;
using System.Web;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using Batteries.Models;
using Batteries.Dal;
using System.Web.Security;
using Newtonsoft.Json;
using Batteries.Helpers;

namespace Batteries.Account
{
    public partial class Login : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //RegisterHyperLink.NavigateUrl = "Register";
            //// Enable this once you have account confirmation enabled for password reset functionality
            ////ForgotPasswordHyperLink.NavigateUrl = "Forgot";
            //OpenAuthLogin.ReturnUrl = Request.QueryString["ReturnUrl"];
            //var returnUrl = HttpUtility.UrlEncode(Request.QueryString["ReturnUrl"]);
            //if (!String.IsNullOrEmpty(returnUrl))
            //{
            //    RegisterHyperLink.NavigateUrl += "?ReturnUrl=" + returnUrl;
            //}


            if (UserHelper.UserIsLoggedIn())
            {
                //var pageUrl = Request.QueryString["ReturnUrl"];
                //IdentityHelper.RedirectToReturnUrl(
                //    pageUrl != null ? Request.QueryString["ReturnUrl"] : "/Dashboard", Response);
                //IdentityHelper.RedirectToReturnUrl(
                //pageUrl != null ? Request.QueryString["ReturnUrl"] : "/Experiments/", Response);
                IdentityHelper.RedirectToReturnUrl(
                     "/Experiments/", Response);
            }
            if (!IsPostBack)
            {
                // Render success message
                var message = Request.QueryString["m"];
                if (message != null)
                {
                    // Strip the query string from action
                    Form.Action = ResolveUrl("~/Account/Login");

                    SuccessMessage = String.Empty;

                    if (message == "ChangePwdSuccess")
                    {
                        SuccessMessage = "The password was changed! Please log back in!";
                    }
                    else if (message == "ResetPwdSuccess")
                    {
                        SuccessMessage = "The password was reset! Please log back in!";
                    }
                    successMessage.Visible = !String.IsNullOrEmpty(SuccessMessage);
                }
            }

            Username.Focus();
        }
        protected string SuccessMessage
        {
            get;
            private set;
        }
        protected void LogIn(object sender, EventArgs e)
        {
            if (!IsValid) return;
            var user = new UserDa().ValidateUser(Username.Text.Trim(), Password.Text.Trim());
            if (user != null)
            {
                var tkt = new FormsAuthenticationTicket(1, Username.Text.Trim(), DateTime.Now,
                    DateTime.Now.AddMinutes(2880), false, JsonConvert.SerializeObject(user));
                var cookiestr = FormsAuthentication.Encrypt(tkt);
                var ck = new HttpCookie(FormsAuthentication.FormsCookieName, cookiestr)
                {
                    Path = FormsAuthentication.FormsCookiePath,
                    Expires = tkt.Expiration
                };
                Response.Cookies.Add(ck);
                var pageUrl = Request.QueryString["ReturnUrl"];
                //IdentityHelper.RedirectToReturnUrl(
                //    pageUrl != null ? Request.QueryString["ReturnUrl"] : "/Dashboard", Response);
                IdentityHelper.RedirectToReturnUrl(
                    pageUrl != null ? Request.QueryString["ReturnUrl"] : "/Experiments/", Response);
            }
            else
            {
                FailureText.Text = "Wrong username or password";
                ErrorMessage.Visible = true;
            }
        }

        //protected void LogIn(object sender, EventArgs e)
        //{
        //    if (IsValid)
        //    {
        //        // Validate the user password
        //        var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
        //        var signinManager = Context.GetOwinContext().GetUserManager<ApplicationSignInManager>();

        //        // This doen't count login failures towards account lockout
        //        // To enable password failures to trigger lockout, change to shouldLockout: true
        //        var result = signinManager.PasswordSignIn(Email.Text, Password.Text, RememberMe.Checked, shouldLockout: false);

        //        switch (result)
        //        {
        //            case SignInStatus.Success:
        //                IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
        //                break;
        //            case SignInStatus.LockedOut:
        //                Response.Redirect("/Account/Lockout");
        //                break;
        //            case SignInStatus.RequiresVerification:
        //                Response.Redirect(String.Format("/Account/TwoFactorAuthenticationSignIn?ReturnUrl={0}&RememberMe={1}", 
        //                                                Request.QueryString["ReturnUrl"],
        //                                                RememberMe.Checked),
        //                                  true);
        //                break;
        //            case SignInStatus.Failure:
        //            default:
        //                FailureText.Text = "Invalid login attempt";
        //                ErrorMessage.Visible = true;
        //                break;
        //        }
        //    }
        //}
    }
}