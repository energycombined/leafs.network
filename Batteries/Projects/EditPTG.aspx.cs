using Batteries.Dal;
using Batteries.Helpers;
using Batteries.Models;
using Batteries.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Batteries.Projects
{
    public partial class EditPTG : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            LoadProjects();
            //LoadTestGroups();
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

        //private void LoadTestGroups()
        //{
        //    List<TestGroupExt> testGroupList = Dal.TestGroupDa.GetAllTestGroups();

        //    int index = 0;

        //    foreach (TestGroup testGroup in testGroupList)
        //    {
        //        DdlTGroup.Items.Insert(index, new ListItem(testGroup.testGroupName, testGroup.testGroupId.ToString()));
        //        index++;
        //    }

        //    DdlTGroup.Items.Insert(0, new ListItem("", ""));
        //}

        protected void UpdateButton_OnClick(object sender, EventArgs e)
        {
            try
            {
                var projectTestGroup = new ProjectTestGroup
                {
                    fkProject = int.Parse(DdlProject.SelectedItem.Value),
                    fkTestGroup = Request.Form["DdlTestGroup"] != null ? int.Parse(Request.Form["DdlTestGroup"]) : (int?)null,
                    fkUser = UserHelper.GetCurrentUser().userId
                };

                var result = ProjectTestGroupDa.AddProjectTG(projectTestGroup);
                if (result == 0)
                {
                    NotifyHelper.Notify("Success", NotifyHelper.NotifyType.success, "");
                    RedirectHelper.RedirectToReturnUrl(ResolveUrl("AddTestGroupToProject.aspx"), Response);
                }
                else
                    NotifyHelper.Notify("ProjectTG not inserted", NotifyHelper.NotifyType.danger, "");
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
            Response.Redirect("~/Projects/AddTestGroupToProject");
        }
    }
}