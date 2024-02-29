using Batteries.Dal;
using Batteries.Helpers;
using Batteries.Models;
using Batteries.Models.Responses;
using Microsoft.AspNet.FriendlyUrls;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Batteries.Projects
{
    public partial class Edit : System.Web.UI.Page
    {

        public int projectId = 0;
        public int currentRG;
        public int projectRgId;
        protected void Page_Load(object sender, EventArgs e)
        {
            var currentUser = UserHelper.GetCurrentUser();
            currentRG = (int)currentUser.fkResearchGroup;

            var project = GetProject(GetProjectIdFromUrl());
            projectId = project.projectId;
            projectRgId = (int)project.fkResearchGroup;

            if (!ProjectDa.IsParticipant(projectId, (int)currentUser.fkResearchGroup))
            {
                RedirectHelper.RedirectToReturnUrl(ResolveUrl("~/Projects/"), Response);
            }

            if (project.fkResearchGroup != currentUser.fkResearchGroup)
            {
                UpdateButton.Visible = false;
            }

            if (IsPostBack) return;
            Fill(project);
        }
        private int GetProjectIdFromUrl()
        {
            IList<string> segments = Request.GetFriendlyUrlSegments();
            int pId = -1;
            if (segments.Count != 0)
                int.TryParse(segments[0], out pId);
            return pId;
        }
        private Project GetProject(int projectId)
        {
            return ProjectDa.GetProjectById(projectId);
        }
        private void Fill(Project project)
        {
            TxtName.Text = project.projectName;
            TxtAcronym.Text = project.projectAcronym;
            TxtAdminCoor.Text = project.administrativeCoordinator;
            TxtAdminContact.Text = project.administrativeCoordinatorContact;
            TxtAdminContactMail.Text = project.administrativeCoordinatorEmail;
            TxtTechCoor.Text = project.technicalCoordinator;
            TxtTechCoorContact.Text = project.technicalCoordinatorContact;
            TxtTechCoorMail.Text = project.technicalCoordinatorEmail;
            TxtInnManager.Text = project.innovationManager;
            TxtInnManagerContact.Text = project.innovationManagerContact;
            TxtDissCoor.Text = project.disseminationCoordinator;
            TxtDissCoorContact.Text = project.disseminationCoordinatorContact;
            TxtGrantFund.Text = project.grantFundingOrganisation;
            TxtFundProg.Text = project.fundingProgramme;
            TxtCallIden.Text = project.callIdentifier;
            TxtCallTop.Text = project.callTopic;
            DdlFixedKey.Items.Insert(0, new ListItem("", "0"));
            DdlFixedKey.SelectedItem.Text = project.fixedKeywords;
            //DdlTestGroup.SelectedValue = project.listOfPartners != (int?)null ? project.listOfPartners.ToString() : "";
            TxtFreeKey.Text = project.freeKeywords;
            TxtStartDate.Text = project.startProject.ToString() != "" ? DateTime.Parse(project.startProject.ToString()).ToString(ConfigurationManager.AppSettings["dateFormat"]) : "";
            TxtEndDate.Text = project.endProject.ToString() != "" ? DateTime.Parse(project.endProject.ToString()).ToString(ConfigurationManager.AppSettings["dateFormat"]) : "";
            TxtGoal.Text = project.projectDescription;
        }


        //private void LoadTestGroups()
        //{          

        //    var currentUser = UserHelper.GetCurrentUser();
        //    int researchGroupId = (int)currentUser.fkResearchGroup;

        //    List<TestGroupExt> testGroupList = Dal.TestGroupDa.GetAllTestGroups(researchGroupId);

        //    int index = 0;

        //    foreach (TestGroup testGroup in testGroupList)
        //    {
        //        DdlTestGroup.Items.Insert(index, new ListItem(testGroup.testGroupName, testGroup.testGroupId.ToString()));
        //        index++;
        //    }

        //    DdlTestGroup.Items.Insert(0, new ListItem("", ""));
        //}

        protected void UpdateButton_OnClick(object sender, EventArgs e)
        {
            try
            {
                var project = new Project
                {
                    projectId = GetProjectIdFromUrl(),
                    projectName = TxtName.Text,
                    projectAcronym = TxtAcronym.Text,
                    administrativeCoordinator = TxtAdminCoor.Text,
                    administrativeCoordinatorContact = TxtAdminContact.Text,
                    administrativeCoordinatorEmail = TxtAdminContactMail.Text,
                    technicalCoordinator = TxtTechCoor.Text,
                    technicalCoordinatorContact = TxtTechCoorContact.Text,
                    technicalCoordinatorEmail = TxtTechCoorMail.Text,
                    innovationManager = TxtInnManager.Text,
                    innovationManagerContact = TxtInnManagerContact.Text,
                    disseminationCoordinator = TxtDissCoor.Text,
                    disseminationCoordinatorContact = TxtDissCoorContact.Text,
                    grantFundingOrganisation = TxtGrantFund.Text,
                    fundingProgramme = TxtFundProg.Text,
                    callIdentifier = TxtCallIden.Text,
                    callTopic = TxtCallTop.Text,
                    fixedKeywords = DdlFixedKey.SelectedItem.Text,
                    //listOfPartners = (DdlTestGroup.SelectedValue != "") ? int.Parse(DdlTestGroup.SelectedValue) : (int?)null,
                    //listOfPartners = Request.Form["DdlTestGroup"] != null ? int.Parse(Request.Form["DdlTestGroup"]) : (int?)null,
                    freeKeywords = TxtFreeKey.Text,
                    startProject = (TxtStartDate.Text != "") ? DateTime.ParseExact(TxtStartDate.Text, ConfigurationManager.AppSettings["dateFormat"], CultureInfo.InvariantCulture) : (DateTime?)null,
                    endProject = (TxtEndDate.Text != "") ? DateTime.ParseExact(TxtEndDate.Text, ConfigurationManager.AppSettings["dateFormat"], CultureInfo.InvariantCulture) : (DateTime?)null,
                    projectDescription = TxtGoal.Text,
                    fkOperator = UserHelper.GetCurrentUser().userId,
                    fkResearchGroup = UserHelper.GetCurrentUser().fkResearchGroup,
                    fkEditedBy = UserHelper.GetCurrentUser().userId
                };
                var result = ProjectDa.UpdateProject(project);
                if (result == 0)
                {
                    NotifyHelper.Notify("Success", NotifyHelper.NotifyType.success, "");
                    RedirectHelper.RedirectToReturnUrl(ResolveUrl("Default.aspx"), Response);
                }
                else
                    NotifyHelper.Notify("Project info not updated", NotifyHelper.NotifyType.danger, "");
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                NotifyHelper.Notify(ex.Message, NotifyHelper.NotifyType.danger, "");
            }
        }
        protected void CancelButton_OnClick(object sender, EventArgs e)
        {
            RedirectHelper.RedirectToReturnUrl(ResolveUrl("~/Projects/Default"), Response);
        }
        //protected void BtnCancel_Click(object sender, EventArgs e)
        //{
        //    Response.Redirect("~/Projects/Default");
        //}
    }
}