using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models.Responses
{
    public class TestGroupExt : TestGroup
    {
        public string operatorUsername { get; set; }
        public string researchGroupName { get; set; }
        public string projectName { get; set; }
        public string projectAcronym { get; set; }
        public TestGroupExt(TestGroup e = null)
        {
            if (e != null)
            {
                this.testGroupId = e.testGroupId;
                this.testGroupGoal = e.testGroupGoal;
                this.testGroupName = e.testGroupName;
                this.fkResearchGroup = e.fkResearchGroup;
                this.fkProject = e.fkProject;
                this.dateCreated = e.dateCreated;
                this.lastChange = e.lastChange;
                this.fkUser = e.fkUser;
            }
        }
    }
}