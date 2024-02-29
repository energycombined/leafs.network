using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.FriendlyUrls;
using Batteries.Models;
using Batteries.Helpers;
using Batteries.Dal;
using Batteries.Models.Responses;
using System.Web.Services;
using System.Web.Script.Services;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System.Dynamic;

namespace Batteries.TestGroups
{
    public partial class Edit : System.Web.UI.Page
    {
        public int testGroupId = 0;
        public int? projectId = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            var currentUser = UserHelper.GetCurrentUser();

            TestGroupExt testGroup = GetTestGroup(GetTestGroupIdFromUrl());
            testGroupId = testGroup.testGroupId;

            projectId = testGroup.fkProject;

            if (!ProjectDa.IsParticipant(projectId, (int)currentUser.fkResearchGroup))
            {
                RedirectHelper.RedirectToReturnUrl(ResolveUrl("Default.aspx"), Response);
            }

            if (testGroup.fkResearchGroup != currentUser.fkResearchGroup)
            {
                UpdateButton.Visible = false;
            }

            if (IsPostBack) return;
            Fill(testGroup);
        }
        private int GetTestGroupIdFromUrl()
        {
            IList<string> segments = Request.GetFriendlyUrlSegments();
            int pId = -1;
            if (segments.Count != 0)
                int.TryParse(segments[0], out pId);
            return pId;
        }
        private TestGroupExt GetTestGroup(int testGroupId)
        {
            var testGroup = TestGroupDa.GetAllTestGroups(testGroupId);
            return testGroup[0];
        }
        private void Fill(TestGroupExt testGroup)
        {
            TxtName.Text = testGroup.testGroupName;
            TxtGoal.Text = testGroup.testGroupGoal;
            TxtProject.Text = testGroup.projectAcronym;
            txtProjectId.Text = testGroup.fkProject.ToString();
        }

        protected void UpdateButton_OnClick(object sender, EventArgs e)
        {
            try
            {
                var testGroup = new TestGroup
                {
                    testGroupId = GetTestGroupIdFromUrl(),
                    testGroupGoal = TxtGoal.Text,
                    testGroupName = TxtName.Text,
                    fkProject = (txtProjectId.Text != "") ? int.Parse(txtProjectId.Text) : (int?)null,
                    //fkProject = Request.Form["DdlProject"] != null ? int.Parse(Request.Form["DdlProject"]) : (int?)null,
                    fkResearchGroup = UserHelper.GetCurrentUser().fkResearchGroup
                };
                var result = TestGroupDa.UpdateTestGroup(testGroup);
                if (result == 0)
                {
                    NotifyHelper.Notify("Success", NotifyHelper.NotifyType.success, "");
                    RedirectHelper.RedirectToReturnUrl(ResolveUrl("Default.aspx"), Response);
                }
                else
                    NotifyHelper.Notify("Test Group info not updated", NotifyHelper.NotifyType.danger, "");
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
            RedirectHelper.RedirectToReturnUrl(ResolveUrl("Default.aspx"), Response);
        }
    }
}