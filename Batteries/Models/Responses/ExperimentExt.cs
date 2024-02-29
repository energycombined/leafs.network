using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models.Responses
{
    public class ExperimentExt : Experiment
    {
        public string operatorUsername { get; set; }
        public string researchGroupName { get; set; }        
        public string researchGroupAcronym { get; set; }        
        public string projectName { get; set; }
        public string projectAcronym { get; set; }
        public string testGroupName { get; set; }
        public string editingOperatorUsername { get; set; }
        public string anodeTotalActiveMaterials { get; set; }
        public string anodeActiveMaterials { get; set; }
        public string anodeActivePercentages { get; set; }
        public string cathodeTotalActiveMaterials { get; set; }
        public string cathodeActiveMaterials { get; set; }
        public string cathodeActivePercentages { get; set; }
        public ExperimentExt(Experiment e = null)
        {
            if (e != null)
            {
                this.experimentId = e.experimentId;
                this.experimentSystemLabel = e.experimentSystemLabel;
                this.experimentPersonalLabel = e.experimentPersonalLabel;
                this.fkUser = e.fkUser;
                this.fkResearchGroup = e.fkResearchGroup;
                this.fkProject = e.fkProject;
                this.experimentDescription = e.experimentDescription;
                this.dateCreated = e.dateCreated;
                this.dateModified = e.dateModified;
                this.isComplete = e.isComplete;
                this.fkTemplate = e.fkTemplate;
                this.hasTestResultsDoc = e.hasTestResultsDoc;
                this.fkEditedBy = e.fkEditedBy;
                this.fkSharingType = e.fkSharingType;
            }
        }
    }
}