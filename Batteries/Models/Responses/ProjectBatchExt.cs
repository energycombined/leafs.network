using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models.Responses
{
    public class ProjectBatchExt : ProjectBatch
    {
        public string projectName { get; set; }
        public string batchName { get; set; }
        public string experimentName { get; set; }
        public ProjectBatchExt(ProjectBatch e = null)
        {
            if (e != null)
            {
                this.projectBatchId = e.projectBatchId;
                this.fkProject = e.fkProject;
                this.fkBatch = e.fkBatch;
                this.fkComingExperiment = e.fkComingExperiment;
                this.fkComingBatch = e.fkComingBatch;
                this.dateCreated = e.dateCreated;
            }
        }
    }
}