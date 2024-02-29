using Batteries.Bll;
using Batteries.Dal;
using Batteries.Helpers;
using Batteries.Models;
using Batteries.Models.Responses;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Batteries.Admin.UsersPanel
{
    public partial class Insert : System.Web.UI.Page
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            GetRoles();
            //GetResearchGroups();
        }

        private void GetRoles()
        {
            var roles = Bl.GetAllRoles();
            DdlRoles.DataSource = roles;
            DdlRoles.DataBind();
        }
        //private void GetResearchGroups()
        //{
        //    List<ResearchGroupExt> researchGroupsList = ResearchGroupDa.GetAllResearchGroups();
        //    int index = 0;
        //    if (researchGroupsList != null)
        //    {
        //        foreach (ResearchGroup researchGroup in researchGroupsList)
        //        {
        //            DdlResearchGroup.Items.Insert(index, new ListItem(researchGroup.acronym + " - " + researchGroup.researchGroupName, researchGroup.researchGroupId.ToString()));
        //            index++;
        //        }
        //    }
        //    DdlResearchGroup.Items.Insert(0, new ListItem("", ""));
        //}

        protected void BtnInsert_Click(object sender, EventArgs e)
        {
            int? researchGroupId = null;
            try
            {
                if (HfResearchGroupSelectedValue.Value != "")
                {
                    researchGroupId = int.Parse(HfResearchGroupSelectedValue.Value);
                }
                var result = Bl.InsertUser(int.Parse(DdlRoles.SelectedValue), TxtUsername.Text, TxtPassword.Text,
                    TxtFirstname.Text, TxtLastname.Text, TxtPhone.Text, TxtEmail.Text, (int)researchGroupId);
                if (result)
                {
                    string msgBody = String.Format(
                        "An account has been created for you." + "<br>" + "Username: {0} Password: {1}" + "<br>" +
                        "<a href=\"{2}\">{3}</a>",
                        HttpUtility.HtmlEncode(TxtUsername.Text), HttpUtility.HtmlEncode(TxtPassword.Text),
                        HttpUtility.HtmlEncode(
                            "http://" + ConfigurationManager.AppSettings["server"] + ":" +
                            ConfigurationManager.AppSettings["httpPort"] + "/Account/Login"),
                            HttpUtility.HtmlEncode("Login here"));
                    Mail.SendMail(TxtEmail.Text, ConfigurationManager.AppSettings["mailUser"], "Account created", msgBody);
                    NotifyHelper.Notify("User successfully created", NotifyHelper.NotifyType.success, "");
                    Response.Redirect("Default");
                }

                else
                {
                    //SetErrorMessage("User not inserted");
                    NotifyHelper.Notify("Some error occured", NotifyHelper.NotifyType.danger, "");
                }
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                //SetErrorMessage(ex.Message);
                NotifyHelper.Notify(ex.Message, NotifyHelper.NotifyType.danger, "");
            }
        }

        protected void BtnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("Default");
        }

        //private void SetErrorMessage(string output)
        //{
        //    ErrorLabel.Visible = true;
        //    ErrorLabel.Text = output;
        //}
    }
}