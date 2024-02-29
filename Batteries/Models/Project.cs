using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Batteries.Models
{
    public class Project
    {
        public int projectId { get; set; }        
        [Required]
        public string projectName { get; set; }

        [Required]
        public string projectAcronym { get; set; }
        [Required]
        public string administrativeCoordinator  { get; set; }
        [Required]
        public string administrativeCoordinatorContact { get; set; }
        [Required]
        public string administrativeCoordinatorEmail { get; set; }
        [Required]
        public string technicalCoordinator { get; set; }
        [Required]
        public string technicalCoordinatorContact { get; set; }
        [Required]
        public string technicalCoordinatorEmail { get; set; }

        public string innovationManager { get; set; }
        public string innovationManagerContact { get; set; }
        public string disseminationCoordinator { get; set; }
        public string disseminationCoordinatorContact  { get; set; }
        [Required]
        public string grantFundingOrganisation { get; set; }
        [Required]
        public string fundingProgramme { get; set; }
        [Required]
        public string callIdentifier { get; set; }
        [Required]
        public string callTopic { get; set; }
        [Required]
        public string fixedKeywords { get; set; }
        [Required]
        public string freeKeywords { get; set; }
        [Required]
        public DateTime? startProject { get; set; }
        [Required]
        public DateTime? endProject { get; set; }

        [Required]
        [MaxLength(2000)]
        public string projectDescription { get; set; }
        [Required(ErrorMessage ="Test group is required")]
        public int? listOfPartners { get; set; }
        public int? fkResearchGroup { get; set; }
        public DateTime? dateCreated { get; set; }
        public int? fkOperator { get; set; }
        public DateTime? lastChange { get; set; }
        public int? fkEditedBy { get; set; }
    }
}