using Batteries.Dal;
using Batteries.Helpers;
using Batteries.Models;
using Batteries.Models.Responses;
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
    public partial class Insert : System.Web.UI.Page
    {
        public int projectId;
        protected void Page_Load(object sender, EventArgs e)
        {
            //LoadResearchGroups();
            if (IsPostBack) return;
        }

        //private void LoadResearchGroups()
        //{
        //    List<ResearchGroupExt> researchGroupList = Dal.ResearchGroupDa.GetAllResearchGroups();

        //    int index = 0;

        //    foreach (ResearchGroup researchGroup in researchGroupList)
        //    {
        //        DdlRGroup.Items.Insert(index, new ListItem(researchGroup.researchGroupName, researchGroup.researchGroupId.ToString()));
        //        index++;
        //    }

        //    DdlRGroup.Items.Insert(0, new ListItem("", ""));
        //}

        protected void BtnInsert_Click(object sender, EventArgs e)
        {

            try
            {                
                var project = new Project
                {
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
                    freeKeywords = TxtFreeKey.Text,
                    startProject = (TxtStartDate.Text != "") ? DateTime.ParseExact(TxtStartDate.Text, ConfigurationManager.AppSettings["dateFormat"], CultureInfo.InvariantCulture) : (DateTime?)null,
                    endProject = (TxtEndDate.Text != "") ? DateTime.ParseExact(TxtEndDate.Text, ConfigurationManager.AppSettings["dateFormat"], CultureInfo.InvariantCulture) : (DateTime?)null,
                    projectDescription = TxtGoal.Text,
                    listOfPartners = Request.Form["DdlTestGroup"] != null ? int.Parse(Request.Form["DdlTestGroup"]) : (int?)null,
                    //listOfPartners = int.Parse(DdlTestGroup.SelectedValue),

                    fkResearchGroup = UserHelper.GetCurrentUser().fkResearchGroup,
                    fkOperator = UserHelper.GetCurrentUser().userId

                };

                int returnetProjectId = ProjectDa.AddProject(project);

                projectId = returnetProjectId;

                if (returnetProjectId != 0)
                {
                    var projectResearchGroup = new ProjectResearchGroup
                    {
                        fkProject = returnetProjectId,
                        fkResearchGroup = UserHelper.GetCurrentUser().fkResearchGroup,
                        fkUser = UserHelper.GetCurrentUser().userId
                    };

                    var result = ProjectResearchGroupDa.AddProjectResearchGroup(projectResearchGroup);
                    if (result == 0)
                    {
                        NotifyHelper.Notify("Success", NotifyHelper.NotifyType.success, "");
                        RedirectHelper.RedirectToReturnUrl(ResolveUrl("Default.aspx"), Response);
                    }
                    else
                        NotifyHelper.Notify("ProjectRg not inserted", NotifyHelper.NotifyType.danger, "");
                }

                //var result = ProjectDa.AddProject(project);
                //if (result == 0)
                //{
                //    NotifyHelper.Notify("Success", NotifyHelper.NotifyType.success, "");
                //    RedirectHelper.RedirectToReturnUrl(ResolveUrl("Default.aspx"), Response);
                //}
                //else
                //    NotifyHelper.Notify("Project not inserted", NotifyHelper.NotifyType.danger, "");
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                NotifyHelper.Notify(ex.Message, NotifyHelper.NotifyType.danger, "");
            }
        }
        protected void BtnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Projects/Default");
        }
    }
}