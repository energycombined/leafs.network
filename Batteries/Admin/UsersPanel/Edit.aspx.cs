using Batteries.Bll;
using Batteries.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.FriendlyUrls;
using Batteries.Models.Responses;
using Batteries.Dal;
using Batteries.Helpers;

namespace Batteries.Admin.UsersPanel
{
    public partial class Edit : System.Web.UI.Page
    {
        public int? userId = 0;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            var user = GetUser(GetUserIdFromUrl());
            //GetResearchGroups();
            Fill(user);
        }

        private int GetUserIdFromUrl()
        {
            IList<string> segments = Request.GetFriendlyUrlSegments();
            int pId = -1;
            if (segments.Count != 0)
                int.TryParse(segments[0], out pId);
            return pId;
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
        //        //foreach (ResearchGroup researchGroup in researchGroupsList)
        //        //{
        //        //    DdlResearchGroup.Items.Insert(index, new ListItem(researchGroup.researchGroupName, researchGroup.researchGroupId.ToString()));
        //        //    index++;
        //        //}
        //    }
        //    //DdlResearchGroup.Items.Insert(0, new ListItem("", ""));
        //}

        private void Fill(UserExt user)
        {
            /*int? researchGroupId = user.fkResearchGroup;
            List<ResearchGroupExt> researchGroupsList = ResearchGroupDa.GetAllResearchGroups(researchGroupId);
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
                var result = Bl.UpdateUser(GetUserIdFromUrl(), TxtUsername.Text, TxtFirstname.Text, TxtLastname.Text, TxtPhone.Text,
                    TxtEmail.Text, true);
                if (result)
                {
                    NotifyHelper.Notify("Saved", NotifyHelper.NotifyType.success, "");
                    Response.Redirect("../Default");
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

        private void SetErrorMessage(string output)
        {
            ErrorLabel.Visible = true;
            ErrorLabel.Text = output;
        }

        protected void CancelButton_OnClick(object sender, EventArgs e)
        {
            Response.Redirect("../Default");
        }
    }
}