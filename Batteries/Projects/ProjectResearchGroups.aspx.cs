using Batteries.Dal;
using Batteries.Helpers;
using Batteries.Models;
using Batteries.Models.Responses;
using Microsoft.AspNet.FriendlyUrls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Batteries.Projects
{
    public partial class ProjectResearchGroups : System.Web.UI.Page
    {
        public string projectName;
        public string projectAcronym;
        public int projectId = 0;
        public int projectRG;
        protected void Page_Load(object sender, EventArgs e)
        {
            var currentUser = UserHelper.GetCurrentUser();
            
            var project = GetProject(GetProjectIdFromUrl());
            projectId = project.projectId;
            projectRG = (int)project.fkResearchGroup;
            if (!ProjectDa.IsParticipant(projectId, (int)currentUser.fkResearchGroup))
            {
                RedirectHelper.RedirectToReturnUrl(ResolveUrl("~/Projects/"), Response);
            }
            if (IsPostBack) return;

            projectName = project.projectName;
            projectAcronym = project.projectAcronym;
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
        protected void CancelButton_OnClick(object sender, EventArgs e)
        {
            RedirectHelper.RedirectToReturnUrl(ResolveUrl("~/Projects/Default"), Response);
        }
    }
}