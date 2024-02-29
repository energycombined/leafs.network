using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models.Responses
{
    public class ResearchGroupExt : ResearchGroup
    {
        public string operatorUsername { get; set; }
        public ResearchGroupExt(ResearchGroup e = null)
        {
            if (e != null)
            {
                this.researchGroupId = e.researchGroupId;
                this.researchGroupName = e.researchGroupName;
                this.acronym = e.acronym;
                this.dateCreated = e.dateCreated;
                this.lastChange = e.lastChange;
                this.fkOperator = e.fkOperator;
                this.lastExperimentNumber = e.lastExperimentNumber;
                this.lastBatchNumber = e.lastBatchNumber;
            }
        }
    }
}