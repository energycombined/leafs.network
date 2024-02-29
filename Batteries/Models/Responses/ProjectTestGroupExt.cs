using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models.Responses
{
    public class ProjectTestGroupExt : ProjectTestGroup
    {
        public string operatorUsername { get; set; }
        public string testGroupName { get; set; }
        public string projectName { get; set; }
        public ProjectTestGroupExt(ProjectTestGroup e = null)
        {
            if (e != null)
            {
                this.projectTestGroupId = e.projectTestGroupId;
                this.fkProject = e.fkProject;
                this.fkTestGroup = e.fkTestGroup;
                this.fkUser = e.fkUser;
                this.dateCreated = e.dateCreated;
            }
        }
    }
}