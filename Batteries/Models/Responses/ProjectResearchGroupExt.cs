using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models.Responses
{
    public class ProjectResearchGroupExt : ProjectResearchGroup
    {
        public string operatorUsername { get; set; }
        public string researchGroupName { get; set; }
        public string researchGroupAcronym { get; set; }
        public string researchGroupCreator { get; set; }
        public string projectName { get; set; }
        public ProjectResearchGroupExt(ProjectResearchGroup e = null)
        {
            if (e != null)
            {
                this.projectResearchGroupId = e.projectResearchGroupId;
                this.fkProject = e.fkProject;
                this.fkResearchGroup = e.fkResearchGroup;
                this.fkUser = e.fkUser;
                this.dateCreated = e.dateCreated;
            }
        }
    }
}