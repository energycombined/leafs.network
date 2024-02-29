using Batteries.Dal;
using Batteries.Helpers;
using Batteries.Models;
using Batteries.Models.Responses;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Batteries.Projects
{
    public partial class EditPRG : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            LoadProjects();
            LoadResearchGroups();
        }

       
        private void LoadProjects()
        {

            List<ProjectExt> projectList = Dal.ProjectDa.GetAllProjectsBrisi();

            int index = 0;

            foreach (Project projects in projectList)
            {
                DdlProject.Items.Insert(index, new ListItem(projects.projectName, projects.projectId.ToString()));
                index++;
            }

            DdlProject.Items.Insert(0, new ListItem("", ""));
        }

        private void LoadResearchGroups()
        {
            List<ResearchGroupExt> researchGroupList = Dal.ResearchGroupDa.GetAllResearchGroups();

            int index = 0;

            foreach (ResearchGroup researchGroup in researchGroupList)
            {
                DdlRGroup.Items.Insert(index, new ListItem(researchGroup.researchGroupName, researchGroup.researchGroupId.ToString()));
                index++;
            }

            DdlRGroup.Items.Insert(0, new ListItem("", ""));
        }

        protected void UpdateButton_OnClick(object sender, EventArgs e)
        {
            try
            {
                var projectResearchGroup = new ProjectResearchGroup
                {
                    fkProject = int.Parse(DdlProject.SelectedItem.Value),
                    fkResearchGroup = int.Parse(DdlRGroup.SelectedItem.Value),
                    fkUser = UserHelper.GetCurrentUser().userId,
                    fkResearchGroupCreator = UserHelper.GetCurrentUser().fkResearchGroup
                };
                
                var result = ProjectResearchGroupDa.AddProjectRG(projectResearchGroup);
                if (result == 0)
                {
                    NotifyHelper.Notify("Success", NotifyHelper.NotifyType.success, "");
                    RedirectHelper.RedirectToReturnUrl(ResolveUrl("AddResearchGroupToProject.aspx"), Response);
                }
                else
                    NotifyHelper.Notify("ProjectRg not inserted", NotifyHelper.NotifyType.danger, "");
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
            Response.Redirect("~/Projects/AddResearchGroupToProject");
        }
    }
}