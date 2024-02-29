using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models.Responses
{
    public class TestGroupExperimentExt : TestGroupExperiment
    {
        public string experimentOperatorUsername { get; set; }
        public string experimentSystemLabel { get; set; }
        public string experimentPersonalLabel { get; set; }
        public string experimentResearchGroupName { get; set; }
        public string testGroupName { get; set; }
        public string testGroupGoal { get; set; }
        public DateTime? dateCreatedExperiment { get; set; }

        public TestGroupExperimentExt(TestGroupExperiment e = null)
        {
            if (e != null)
            {
                this.testGroupExperimentId = e.testGroupExperimentId;
                this.fkTestGroup = e.fkTestGroup;
                this.fkExperiment = e.fkExperiment;
                this.fkProject = e.fkProject;
                this.fkUser = e.fkUser;
                this.experimentHypothesis = e.experimentHypothesis;
                this.conclusion = e.conclusion;
                this.dateCreated = e.dateCreated;
            }
        }
    }
}