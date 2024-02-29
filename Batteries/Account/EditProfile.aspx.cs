using Batteries.Bll;
using Batteries.Dal;
using Batteries.Helpers;
using Batteries.Models;
using Batteries.Models.Responses;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Batteries.Account
{
    public partial class EditProfile : System.Web.UI.Page
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private static User currentUser;
        private static int currentUserId;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            currentUser = UserHelper.GetCurrentUser();
            currentUserId = currentUser.userId;
            //GetResearchGroups();
            
            var user = GetUser(currentUserId);
            Fill(user);
        }
        private UserExt GetUser(int userId)
        {
            var user = Bl.GetUser(userId);
            return user;
        }
        //private void GetResearchGroups()
        //{
        //    List<ResearchGroupExt> researchGroupsList = ResearchGroupDa.GetAllResearchGroups();
        //    int index = 0;
        //    if (researchGroupsList != null)
        //    {
        //        foreach (ResearchGroup researchGroup in researchGroupsList)
        //        {
        //            DdlResearchGroup.Items.Insert(index, new ListItem(researchGroup.researchGroupName, researchGroup.researchGroupId.ToString()));
        //            index++;
        //        }
        //    }
        //    DdlResearchGroup.Items.Insert(0, new ListItem("", ""));
        //}
        private void Fill(UserExt user)
        {
            /*List<ResearchGroupExt> researchGroupsList = ResearchGroupDa.GetAllResearchGroups();
            ResearchGroupExt resGroup = new ResearchGroupExt();
            resGroup = researchGroupsList[0];*/

            TxtUsername.Text = user.userName;
            TxtFirstname.Text = user.firstName;
            TxtLastname.Text = user.lastName;
            TxtPhone.Text = user.phone;
            TxtEmail.Text = user.email;
            TxtRGroup.Text = user.researchGroupName;
            //CbActive.Checked = user.active;
            //DdlResearchGroup.SelectedValue = user.fkResearchGroup.ToString();
        }

        protected void UpdateButton_OnClick(object sender, EventArgs e)
        {
            try
            {
                var result = Bl.UpdateUser(currentUser.userId, TxtUsername.Text, TxtFirstname.Text, TxtLastname.Text, TxtPhone.Text,
                    TxtEmail.Text, true);
                if (result)
                {
                    NotifyHelper.Notify("Saved", NotifyHelper.NotifyType.success, "");
                    //Response.Redirect("~/Experiments");
                    Response.Redirect("~/Account/EditProfile");
                }
                else
                    SetErrorMessage("User not saved");
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                //SetErrorMessage("Грешка при менување на корисник");
                SetErrorMessage(ex.Message);
            }
        }
        protected void CancelButton_OnClick(object sender, EventArgs e)
        {
            Response.Redirect("~/Experiments");
        }

        private void SetErrorMessage(string output)
        {
            ErrorLabel.Visible = true;
            ErrorLabel.Text = output;
        }

    }
}