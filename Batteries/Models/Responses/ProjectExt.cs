using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models.Responses
{
    public class ProjectExt : Project
    {
        public string operatorUsername { get; set; }
        public string editingOperatorUsername { get; set; }
        public string researchGroupName { get; set; }
        public string researchGroupAcronym { get; set; }
        public ProjectExt(Project e = null)
        {
            if (e != null)
            {
                this.projectId = e.projectId;
                this.projectDescription = e.projectDescription;
                this.projectName = e.projectName;
                this.projectAcronym = e.projectAcronym;
                this.administrativeCoordinator = e.administrativeCoordinator;
                this.administrativeCoordinatorContact = e.administrativeCoordinatorContact;
                this.administrativeCoordinatorEmail = e.administrativeCoordinatorEmail;
                this.technicalCoordinator = e.technicalCoordinator;
                this.technicalCoordinatorContact = e.technicalCoordinatorContact;
                this.technicalCoordinatorEmail = e.technicalCoordinatorEmail;
                this.innovationManager = e.innovationManager;
                this.innovationManagerContact = e.innovationManagerContact;
                this.disseminationCoordinator = e.disseminationCoordinator;
                this.disseminationCoordinatorContact = e.disseminationCoordinatorContact;
                this.grantFundingOrganisation = e.grantFundingOrganisation;
                this.fundingProgramme = e.fundingProgramme;
                this.callIdentifier = e.callIdentifier;
                this.callTopic = e.callTopic;
                this.fixedKeywords = e.fixedKeywords;
                this.freeKeywords = e.freeKeywords;
                this.startProject = e.startProject;
                this.endProject = e.endProject;
                this.projectDescription = e.projectDescription;
                this.listOfPartners = e.listOfPartners;
                this.fkResearchGroup = e.fkResearchGroup;
                this.dateCreated = e.dateCreated;
                this.fkOperator = e.fkOperator;
                this.lastChange = e.lastChange;
                this.fkEditedBy = e.fkEditedBy;
            }
        }
    }
}